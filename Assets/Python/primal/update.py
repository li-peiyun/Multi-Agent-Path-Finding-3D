import socket
import sys
import os
import yaml
import json


current_path = os.path.abspath(__file__)
update_dir = os.path.dirname(current_path)


def update_yaml(data):
    input_yaml_path = os.path.join(update_dir, "input.yaml")
    with open(input_yaml_path, 'r') as file:
        documents = yaml.load(file, Loader=yaml.FullLoader)

    agent_arrived_list = data["agentIndex"]
    positions = data["positions"]
    # print("agent_arrived:", agent_arrived)
    # print("positions:", positions)

    # 更新每个 agent 的 start 值
    for i, position in enumerate(positions):
        documents["agents"][i]["start"] = position

    # 将第 agent_arrived 号 agent 的 goal 值改为其 next 值
    for agent in agent_arrived_list:
        # print("agent: ", agent)
        documents["agents"][agent]["goal"] = documents["agents"][agent]["next"]

    # 将更新后的内容写回 YAML 文件
    with open(input_yaml_path, 'w') as file:
        yaml.dump(documents, file)

    print("Update Done")


HOST = '127.0.0.1'
PORT = 31418
with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as s:
    s.bind((HOST, PORT))
    print('Listening on', (HOST, PORT))
    sys.stdout.flush()

    while True:
        sys.stdout.flush()
        data, addr = s.recvfrom(2048)
        decoded_data = json.loads(data.decode())
        update_yaml(decoded_data)
        sys.stdout.flush()

