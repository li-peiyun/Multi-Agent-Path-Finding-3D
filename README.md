# Multi-Agent-Path-Finding-3D
## 一、conda虚拟环境准备

#### 1. cbs虚拟环境配置

**创建虚拟环境：**

```
conda create -n cbs python=3.9
```

**切换虚拟环境：**

```
conda activate cbs
```

**安装依赖：**

```
pip install -r cbs_requirements.txt
```

**查看环境依赖：**

```
conda list
```

检查所需依赖是否成功安装

#### 2. primal虚拟环境配置

**创建虚拟环境：**

```
conda create -n primal python=3.10
```

**切换虚拟环境：**

```
conda activate primal
```

**安装依赖：**

```
pip install -r primal_requirements.txt
```

**查看环境依赖：**

```
conda list
```

检查所需依赖是否成功安装

## 二、unity安装与配置

#### 安装unity

自行在官网安装，统一unity版本为2022.3.17

#### 创建unity项目

创建名为mapf的3D项目

#### 替换Assets

替换Assets文件夹的内容，Assets文件夹见GitHub，此后我们项目管理也用这种方式

## 三、运行项目与测试

#### 运行项目

看见控制台显示

```
Listening on ('127.0.0.1', 31415)
Listening on ('127.0.0.1', 31416)
```

其中31415是与cbs程序通信的端口号，31415是与primal程序通信的端口号

#### 按下c键（运行cbs程序）

看见控制台显示

```
Sent message: Recognizing
Solution found. All agents reached the goal!
```

代表运行成功，可以看到cbs文件夹下的output.yaml中生成结果

#### 按下p键（运行primal程序）

看见控制台显示

```
Sent message: Recognizing
Solution found. All agents reached the goal!
```

代表运行成功，可以看到primal文件夹下的output.yaml中生成结果

（控制台会显示一些error，是tensorflow导致的，不用管，只要能正确生成结果就行）

#### 结束项目运行

看见控制台显示

```
应用程序即将退出，清理所有Python进程
```

