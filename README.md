# Multi-Agent-Path-Finding-3D
## 一、conda虚拟环境准备

#### 安装与配置教程见：

[Anaconda安装与Python虚拟环境配置保姆级图文教程(附速查字典)_anaconda配置python环境-CSDN博客](https://blog.csdn.net/FRIGIDWINTER/article/details/124078674)

#### 配置我们需要的虚拟环境：

**创建虚拟环境：**

```
conda create -n mapf python=3.9
```

必须保证虚拟环境名称和python版本与上述一致

**切换虚拟环境：**

```
conda activate mapf
```

**安装依赖：**

```
pip install -r requirements.txt
```

requirements.txt见GitHub仓库

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

运行项目，看见控制台显示

```
Listening on ('127.0.0.1', 31415)
UnityEngine.Debug:Log (object)
```

按下空格键，看见控制台显示

```
Sent message: Recognizing
UnityEngine.Debug:Log (object)
```

```
solution found
UnityEngine.Debug:Log (object)
```

结束项目运行，看见控制台显示

```
应用程序即将退出，清理所有Python进程
UnityEngine.Debug:Log (object)
```

如果所有显示一致，说明配置成功！

此外，还可以删除output.yaml的内容，如果运行程序并按下空格后产生内容，说明调用python程序成功！
