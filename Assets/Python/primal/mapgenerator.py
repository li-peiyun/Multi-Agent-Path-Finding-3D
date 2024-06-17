import numpy as np
import os
import yaml
import socket
import sys

DYNAMIC_TESTING = True
GOALS = True
current_path = os.path.abspath(__file__)
primal_dir = os.path.dirname(current_path)  # mapgenerator.py的绝对路径
output_path = os.path.join(primal_dir, "environments")
model_path = os.path.join(primal_dir, "model_primal")
dirDict = {0: (0, 0), 1: (0, 1), 2: (1, 0), 3: (0, -1), 4: (-1, 0), 5: (1, 1), 6: (1, -1), 7: (-1, -1), 8: (-1, 1)}

if DYNAMIC_TESTING:
    import tensorflow as tf
    from ACNet import ACNet


# 初始化
def init(data):
    # 读入input.yaml文件
    input_yaml_path = os.path.join(primal_dir, "input.yaml")
    with open(input_yaml_path, 'r') as input_file:
        try:
            input = yaml.load(input_file, Loader=yaml.FullLoader)
        except yaml.YAMLError as exc:
            print(exc)

    dimension = input["map"]["dimensions"][0]
    agents = input['agents']
    obstacles = input["map"]["obstacles"]

    # 设置地图尺寸
    data.size = dimension

    # state[r,c]=0为空，=-1为障碍物，=x(x>0)为智能体(智能体编号从1开始)
    data.state = np.zeros((data.size, data.size)).astype(int)
    # data.goal为记录目标位置的二维数组
    data.goals = np.zeros((data.size, data.size)).astype(int)

    # data.agent_counter代表智能体数目+1（即当有1个智能体时，data.agent_counter=2）
    data.agent_counter = 1
    # data.primed_goal是目标选择模式下记录选择状态的bool变量
    # data.primed_goal=0代表此时未选择需要设置目标的智能体，=x(x>0)代表已选择x号智能体
    data.primed_goal = 0
    data.ID = 0
    # data.paused = True
    data.blocking_confidences = []
    data.agent_goals = []
    data.agent_positions = []

    data.rnn_states = []
    data.sess = tf.compat.v1.Session()
    data.network = ACNet("global", 5, None, False, 10, "global")
    # load the weights from the checkpoint (only the global ones!)
    ckpt = tf.compat.v1.train.get_checkpoint_state(model_path)
    saver = tf.compat.v1.train.Saver()
    saver.restore(data.sess, ckpt.model_checkpoint_path)

    # 设置障碍物位置
    # r为行数，c为列数，行列数从0开始
    for obstacle in obstacles:
        [obstacle_r, obstacle_c] = obstacle
        data.state[obstacle_r, obstacle_c] = -1

    # 设置智能体初始和目标位置
    for agent in agents:
        # 设置智能体的初始位置
        [start_r, start_c] = agent['start']
        if data.state[start_r, start_c] == 0:
            data.state[start_r, start_c] = data.agent_counter
            data.goals[start_r, start_c] = data.agent_counter
            data.agent_positions.append((start_r, start_c))

            agent_key = f'agent{data.agent_counter - 1}'
            data.output["schedule"][agent_key] = [{'t': 0, 'x': int(start_r), 'y': int(start_c)}]

            data.blocking_confidences.append(0)
            data.rnn_states.append(data.network.state_init)
            data.agent_goals.append((start_r, start_c))
            data.agent_counter += 1
        # 设置该智能体的目标位置
        [goal_r, goal_c] = agent['goal']
        data.primed_goal = data.state[start_r, start_c]
        removeGoal(data, data.primed_goal)
        data.agent_goals[data.primed_goal - 1] = (goal_r, goal_c)
        data.goals[goal_r, goal_c] = data.primed_goal
        data.primed_goal = 0

    data.time_record = np.ones(data.agent_counter - 1)

    if not os.path.exists(output_path):
        os.makedirs(output_path)
    if os.path.exists(output_path):
        for (_, _, files) in os.walk(output_path):
            for f in files:
                if ".npy" in f:
                    try:
                        ID = int(f[:f.find(".npy")])
                    except Exception:
                        continue
                    if ID > data.ID:
                        data.ID = ID
    data.ID += 1

def getDir(action):
    return dirDict[action]

def removeGoal(data, agent):
    for i in range(data.state.shape[0]):
        for j in range(data.state.shape[1]):
            if data.goals[i, j] == agent:
                data.goals[i, j] = 0


def observe(data, agent_id, goals):
    assert (agent_id > 0)
    top_left = (data.agent_positions[agent_id - 1][0] - 10 // 2, data.agent_positions[agent_id - 1][1] - 10 // 2)
    bottom_right = (top_left[0] + 10, top_left[1] + 10)
    obs_shape = (10, 10)
    goal_map = np.zeros(obs_shape)
    poss_map = np.zeros(obs_shape)
    obs_map = np.zeros(obs_shape)
    goals_map = np.zeros(obs_shape)
    visible_agents = []
    for i in range(top_left[0], top_left[0] + 10):
        for j in range(top_left[1], top_left[1] + 10):
            if i >= data.state.shape[0] or i < 0 or j >= data.state.shape[1] or j < 0:
                # out of bounds, just treat as an obstacle
                obs_map[i - top_left[0], j - top_left[1]] = 1
                continue
            if data.state[i, j] == -1:
                # obstacles
                obs_map[i - top_left[0], j - top_left[1]] = 1
            if data.state[i, j] == agent_id:
                # agent's position
                # pos_map[i-top_left[0],j-top_left[1]]=1
                poss_map[i - top_left[0], j - top_left[1]] = 1
            elif data.goals[i, j] == agent_id:
                # agent's goal
                goal_map[i - top_left[0], j - top_left[1]] = 1
            if data.state[i, j] > 0 and data.state[i, j] != agent_id:
                # other agents' positions
                poss_map[i - top_left[0], j - top_left[1]] = 1
                visible_agents.append(data.state[i, j])
    dx = data.agent_goals[agent_id - 1][0] - data.agent_positions[agent_id - 1][0]
    dy = data.agent_goals[agent_id - 1][1] - data.agent_positions[agent_id - 1][1]
    mag = (dx ** 2 + dy ** 2) ** .5
    if mag != 0:
        dx = dx / mag
        dy = dy / mag
    if goals:
        distance = lambda x1, y1, x2, y2: ((x2 - x1) ** 2 + (y2 - y1) ** 2) ** .5
        for agent in visible_agents:
            x, y = data.agent_goals[agent - 1]
            if x < top_left[0] or x >= bottom_right[0] or y >= bottom_right[1] or y < top_left[1]:
                # out of observation
                min_node = (-1, -1)
                min_dist = 1000
                for i in range(top_left[0], top_left[0] + 10):
                    for j in range(top_left[1], top_left[1] + 10):
                        d = distance(i, j, x, y)
                        if d < min_dist:
                            min_node = (i, j)
                            min_dist = d
                goals_map[min_node[0] - top_left[0], min_node[1] - top_left[1]] = 1
            else:
                goals_map[x - top_left[0], y - top_left[1]] = 1
        return ([poss_map, goal_map, goals_map, obs_map], [dx, dy, mag])
    else:
        return ([poss_map, goal_map, obs_map], [dx, dy, mag])


def timerFired(data):
    #if DYNAMIC_TESTING and not data.paused:
    for (x, y) in data.agent_positions:
        ID = data.state[x, y]
        observation = observe(data, ID, GOALS)
        rnn_state = data.rnn_states[ID - 1]  # yes minus 1 is correct
        a_dist, v, rnn_state, blocking = data.sess.run(
            [data.network.policy, data.network.value, data.network.state_out, data.network.blocking],
            feed_dict={data.network.inputs: [observation[0]],
                       data.network.goal_pos: [observation[1]],
                       data.network.state_in[0]: rnn_state[0],
                       data.network.state_in[1]: rnn_state[1]})
        data.rnn_states[ID - 1] = rnn_state
        data.blocking_confidences[ID - 1] = np.ravel(blocking)[0]
        action = np.argmax(a_dist)
        dx, dy = getDir(action)
        ax, ay = data.agent_positions[ID - 1]
        if (ax + dx >= data.state.shape[0] or ax + dx < 0 or ay + dy >= data.state.shape[
            1] or ay + dy < 0):  # out of bounds
            continue
        if (data.state[ax + dx, ay + dy] < 0):  # collide with static obstacle
            continue
        if (data.state[ax + dx, ay + dy] > 0):  # collide with robot
            continue
        # No collision: we can carry out the action
        data.state[ax, ay] = 0
        data.state[ax + dx, ay + dy] = ID
        data.agent_positions[ID - 1] = (ax + dx, ay + dy)
        agent_key = f'agent{ID - 1}'
        if agent_key not in data.output["schedule"]:
            data.output["schedule"][agent_key] = []
        data.output["schedule"][agent_key].append({'t': int(data.time_record[ID - 1]), 'x': int(data.agent_positions[ID - 1][0]), 'y': int(data.agent_positions[ID - 1][1])})
        data.time_record[ID - 1] += 1

def all_agents_reached_goal(data):
    # 检查所有智能体是否都到达终点
    return all((data.agent_positions[agent_id - 1]==data.agent_goals[agent_id - 1]) for agent_id in range(1, data.agent_counter))

def run():
    def timerFiredWrapper(data):
        while True:
            if data.gameOver:
                return
            timerFired(data)
            # redrawAllWrapper(canvas, data)
            # 检查是否所有智能体都到达终点，如果是则设置游戏结束标志
            if all_agents_reached_goal(data):
                data.gameOver = True
                output_yaml_path = os.path.join(primal_dir, "output.yaml")
                with open(output_yaml_path, 'w') as output_yaml:
                    yaml.safe_dump(data.output, output_yaml)
                print("Solution found. All agents reached the goal!")

    # Set up data and call init
    class Struct(object): pass

    data = Struct()
    data.t = 0 # 计步器
    data.output = {'schedule': {}}  # 保存输出结果
    data.gameOver = False  # 游戏结束标志
    init(data)
    timerFiredWrapper(data)


def main():
    run()


HOST = '127.0.0.1'
PORT = 31417
with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
    s.bind((HOST, PORT))
    print('Listening on', (HOST, PORT))
    sys.stdout.flush()

    while True:
        sys.stdout.flush()
        data, addr = s.recvfrom(2048)
        if data.decode() == "Recognizing":
            # 执行primal程序
            main()
            sys.stdout.flush()
