import yaml
import os
import socket
import sys
import json


def read_yaml(file_path):
    with open(file_path, 'r') as file:
        return yaml.load(file, Loader=yaml.FullLoader)


def write_yaml(data, file_path):
    with open(file_path, 'w') as file:
        yaml.dump(data, file, default_flow_style=False, sort_keys=False)


def convert_obstacles(obstacles):
    return [tuple(obstacle['coordinate']) for obstacle in obstacles]


def extract_coordinates(data):
    return [data['x'], data['y']]


def convert_agents(init_agents, agent_data):
    return [
        {
            'name': agent['name'],
            'start': agent['start'],
            'goal': extract_coordinates(agent_data[i]['pickupPoint']),
            'next': extract_coordinates(agent_data[i]['agentgoPosition'])
        }
        for i, agent in enumerate(init_agents)
    ]


def remove_agent_goals_from_obstacles(obstacles, agents):
    agent_positions = set(
        tuple(agent['goal']) for agent in agents
    ).union(
        tuple(agent['next']) for agent in agents
    )
    return [obs for obs in obstacles if tuple(obs) not in agent_positions]


def main(agent_data):
    base_dir = os.path.dirname(os.path.abspath(__file__))
    # 计算init.yaml和input.yaml的路径
    init_yaml_path = os.path.join(base_dir, '../../StreamingAssets/init.yaml')
    input_yaml_path = os.path.join(base_dir, 'input.yaml')

    init_data = read_yaml(init_yaml_path)

    agents = convert_agents(init_data['agents'], agent_data)
    obstacles = convert_obstacles(init_data['map']['simple_obstacles']) + \
                convert_obstacles(init_data['map']['goods']) + \
                convert_obstacles(init_data['map']['shelf'])

    filtered_obstacles = remove_agent_goals_from_obstacles(obstacles, agents)

    input_data = {
        'agents': agents,
        'map': {
            'dimensions': init_data['map']['dimensions'],
            'obstacles': filtered_obstacles
        }
    }

    write_yaml(input_data, input_yaml_path)
    print("Conversion complete. Data written to input.yaml.")


HOST = '127.0.0.1'
PORT = 31414
with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
    s.bind((HOST, PORT))
    print('Listening on', (HOST, PORT))
    sys.stdout.flush()

    while True:
        sys.stdout.flush()
        data, addr = s.recvfrom(2048)
        agent_data = json.loads(data.decode())
        # 执行init程序
        main(agent_data)
        sys.stdout.flush()
