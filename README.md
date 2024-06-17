# 多智能体路径规划——3D仓储应用
## 一、项目背景

### 群体智能背景调研

#### OpenAI hide and seek

特工们在训练过程中发现了多达六种独特的策略，每一种策略都迫使他们进入下一个游戏阶段。起初，躲藏者和寻道者只是逃跑并互相追逐，但经过大约2500万次捉迷藏，躲藏者学会了通过将箱子一起移动并靠在墙上来建造隐藏的庇护所。在又进行了 7500 万次火柴后，搜寻者移动并使用坡道跳过箱子进入隐藏者的庇护所，1000 万根火柴后，隐藏者开始将坡道带到游乐区的边缘并将它们锁定到位以防止搜寻者使用它们。最后，在总共进行了 3.8 亿场比赛之后，找球手们学会了将箱子带到游乐区的边缘，并有效地将它们“冲浪”到隐藏者的庇护所，利用游乐空间允许他们在不接触地面的情况下与箱子一起移动这一事实。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=YTQ1MTYyYWY1OWI3YTE5ZTZlYTI1OTI0MGEwNWRmYTFfeFhpR3VNS1ZrUmRsUDdlZjFkMlY3bTBsbHhhQlBhdklfVG9rZW46VmtKN2JPZTBKb3dJelp4S0hFZWNodm4zbm1kXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

#### 路径规划系统

群体智能支撑的路径规划技术被广泛应用于各种运动规划任务，极大地解决了多智能体间的群体协同决策问题。如自动驾驶、车路协同、群体机器人等场景。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NjBhYTg0NzA0NmQ0NjFmNGQzYjA3ZWViNTQwZGVjMzBfbFNtM2pYU05TeVdVUG5TMHJZZkNmUFF1VU9SUExtVjhfVG9rZW46TkJMb2JOWXl3b1pGYkN4NjJyUWNYVjd4bmNjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

#### 基于虚幻引擎的多智能体强化学习环境Unreal-HMAP

中科院自动化研究所，群体智能团队研发出多智能体游戏引擎，进攻方伺机占领夺控点，防守方负责驱赶。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NjgxN2QwZDVjY2M1OTBiYjM4M2Y2M2ExMTJiODg3YWFfNUI3cndNTXZQVWxzTXhzNFZNY1BDNTQyVFVyejc2bkNfVG9rZW46REdPQmJTRXByb0VuQnl4czVkMWM0SUJHblVjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

### 多智能体路径规划

最终，我们选定的应用场景为智能仓储系统。

#### 智慧仓储

“智慧仓储”是指采用先进的信息技术手段对仓储设备与仓储管理过程进行智能化改进，通过构建一套流程标准化的现代信息管理系统。

#### 应用价值

该技术是物流过程的一个环节。智能仓储的应用，保证了货物仓库管理的效率和准确性，合理调度资源，节约人力成本。

#### 研究意义

我们的项目聚焦于解决**智能车物流仓库**中多个智能车间协同控制的问题，通过高效的算法的实现来完成智慧仓库中**货物分拣**的问题。

## 二、项目介绍

### 项目简介

该项目基于两种**多智能体路径规划**算法——cbs算法和primal算法，实现了**物流仓储**环境下智能车自动分拣货物的应用场景。项目提供**交互控制界面**供用户选择智能车取货卸货点，并对**真实仓库场景**进行建模，模拟智能车送货情境。

### 交互控制实现

我们设计了一套完整的UI界面来为用户提供输入控制。在建立导入系统的仓库地图与初始无人车位置后，使用户可以实现选择**智能车取货点、货物终点（包括货架层数）、所用算法**的交互控制，来实现自定义的场景控制。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZGM0MjkwNTcwMjE4OTNiYzFhODlmMTUxNDU5M2M2ZjhfQ1ZGZThaT2xpUnc3MThKWVo2ZnpTR2Y2Q2V2ZGg5bXRfVG9rZW46Szl0OWJVVHRXb3Vvdmx4QjlBY2NPQk9nbkJiXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

### 仓储场景模拟

对真实仓库场景进行建模，仓库中包括**智能车、货物、货架、杂物**等元素。

另外，拥有四种可选视角，可通过键盘按键切换。以下是我们其中两个视角。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MzI4OGVhNzA1OGIxNDUyODFlNDcyNWExNzliMDJiZDdfYTJuMVMxUXBUY25oRFRjTGRUREdqQ1pJYTJRU1ZyNk5fVG9rZW46S25nbWJZOXRUbzVRRmV4azhXNGMwZmFGbmZkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZGM4ZGRkYjVkZDM5MmI0NjdlN2NlNzk2ZTJmY2E1NmVfeGIxOHcyZFFjRDVqdzFhelk4c3pKVXcxQ2JuSHdtakJfVG9rZW46Q2ZMZ2JJeE5Cb0p6WE94VTRIbmM4QUxpblVjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

### 小车动画

小车实现了以下动画效果：

- 小车移动

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MDAwYWIyNzQ5MjI4NGY2NGRkODllNWM4ZjVhNjdiZDZfbnhtMlZZVzNMcmlQZDBHSGtGalh5WGs1aVpEQW1kUnlfVG9rZW46S3N4QWJJaFZ0bzc0NTV4UG0yMmN2am1GbnhkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

- 小车拿取货物

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NjBkYjUxYWI4ZDk0YTNkYmQxM2JiZmNmNjhhYTM1NWFfRUVVTU90d2V6bHRhbjVHWUszWHNnOHprUENzbmpvT0FfVG9rZW46T3BTb2JlSjgzbzFZREJ4UGpTZGM3ZHFYbkVkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

- 小车卸货

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=Y2Q3NTczZTgyZGIyN2EzNjM4ZDFhOWNlMjMxNzZmNjZfYWNRZzB4b1FwY2NnRFZYOHcxQmlVcmphTHp2SnFQNG1fVG9rZW46TjZvTGJMZEJsb2xoZmx4NXFhN2NHdlpMbmxnXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

## 三、MAPF算法

### MAPF算法概述

按照 MAPF规划方式不同，MAPF算法被分为集中式（中心式）规划和分布式执行两种算法。

#### 中心式

中心式的mapf算法由一个中央控制器来为所有智能体规划路径，它的前提假设是中央规划器掌握了所有智能体的起始位置、目标位置和障碍物位置等信息。在求解的速度和质量上都达到较好的效果。我们调研的中心式mapf算法主要有SIPP和CBS两种。

#### SIPP——基于安全间隔的动态环境路径规划

这个算法将安全间隔定义为连续的、无冲突的时间步长（如果它向任何一个方向扩展一个时间步长，那么它就会发生碰撞）。如下图所示。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=OGQxZDYxMzU0NjE3YTYxNWIxYWRkMDU1ODY3YzM0NDZfQXRQb3psdjVzRkhsSHd1aER3WEdnRjNwNFI3dGlRM0dfVG9rZW46UWx5ZmI2NWtWb2l2NmN4eElzZWNoWWhZbmZGXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

虽然任何configuration中安全时间步的数量可能是无界的，但配置中安全时间间隔的数量是有限的，而且通常非常小。通过用安全间隔代替时间步构造搜索空间，将大大降低计算复杂度。此算法用这一观察结果，构造一个由其configuration和安全间隔定义的状态的搜索空间，从而生成一个通常每个configuration只有少数状态的图。

##### 图的构造

用动态障碍物的预计轨迹来为空间中每个“格子”创建一条生命线：

1. 遍历每个动态障碍物轨迹上的点
2. 更新处于该点碰撞距离内的所有“格子”的时间线（碰撞距离=障碍物半径+机器人半径，半径可能随点的不确定性适应性地变化）

伪代码如下：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=YWI5Y2FjZTgzZjVmNTU0ZDY3ZWRkZmUxODQxOTYyNzVfMjNzc0t4UVcwdTBOQXdlNDVGN2hJN3BvOVFOVE0wcFJfVG9rZW46R283MWJWa0djb2tYZWV4QkxDZmM2NUttbjg3XzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

g(s)：从起始状态到s状态，知道的最佳路径的代价

h(s)：从s状态到目标状态代价的估计

c(s,s')：从s到其一个后继者s‘转换的代价

getSuccessors(s)：返回s状态可能的下一个状态，及该后继状态最早能出现的时间

理论上，这个算法可以提供与以时间作为附加维度的规划相同的最优性和完整性保证。

> 参考文献：M. Phillips and M. Likhachev, "SIPP: Safe interval path planning for dynamic environments," 2011 IEEE International Conference on Robotics and Automation, Shanghai, China, 2011, pp. 5628-5635, doi: 10.1109/ICRA.2011.5980306.

#### CBS——Conflict-Base Search

##### 算法结构：

CBS是一个两层算法，分为高层和低层。

高层：遍历各智能体路径，检查智能体之间的冲突，并增加限制

- 在高层，CBS执行对**约束树（CT）的搜索**，其中每个节点N包含一组约束、一个解和总成本。
- CT的根包含一个空的约束集，而每个后继节点继承父节点的约束并为一个智能体添加新约束。
- 通过低层搜索找到一致解，并在验证过程中模拟智能体的移动，如果验证通过，则该节点是目标节点，返回解。
- 如果验证中存在冲突，则生成新的CT节点以解决冲突，添加新约束并插入OPEN列表。

低层：为每个智能体找到满足高层限制的最快路径

- 低层搜索为每个智能体找到满足限制条件的一致路径，可使用任何最优的单智能体路径规划算法。
- 通过验证各个智能体的路径，检测是否存在冲突。
- 如果存在冲突，生成新的CT节点解决冲突，添加相应的新约束，并将新节点插入OPEN列表。

**数据结构：**

CT树constraint tree，二叉树，其中每个节点包含：

1. 限制集(N.constraints)。其中的每条限制属于一个智能体；CT树的根节点限制集为空；子节点继承父节点的限制集的基础上，增加一条对某个智能体的限制
2. 一个解(N.solution)。经过低层搜索后，为每个智能体形成的满足限制条件的路径，共k条路径。
3. 总成本(N.cost)。每个智能体在当前解下的代价之和。该成本相当于A*算法中的f（总代价）。

**算法流程：**

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=YjY0NGI3OTNlNDI0N2VlYmE4OGE5YjNiOTU0ZGNjY2Vfbk5KWmMzdDV1M1ZnR3dZc1BSRHdUTUVGcmJYRGdBWTNfVG9rZW46Q0VaZmJaVGlYb3N3dEd4OVZsT2N4YWtabmNkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=N2UyNjhiZGI3MTExMDk4MDgzOTQyNmIwNTExZDE0MjNfRXRGOTRjRzFRZVZ6cmU2cEdoaWwwZFpsQVY4cHA3RnhfVG9rZW46SnJFd2JXbDBOb01iYnd4eGZvUGNBcUJYbnliXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

上图中老鼠需要到达他们各自的奶酪

上图为构建的CT树

算法伪代码

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MjM2YWE4N2Y1ZDY3ZDc4NjExN2NmYTBjNWFmNTJjYWFfb01kSGFzeU50bW15WUlSam1Lc1BjS1kycGQ4bFRoOW5fVG9rZW46SDJsRGJWOHF4b2hPWnl4dWMwWGNBM0N5blRkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

开始时，a1最优解为 <S1,A1,D,G1> ，对于a2 为<S2,B1,D,G2>2

→验证两个智能体的解决方案时，发现冲突 <a1,a2,D,2> 7

→根被声明为非目标，并生成左右两个子节点以解决冲突11

→为左子节点调用低层搜索，以找到一个满足新约束的最优路径：<S1,A1,A1,D,G1>，在      左子节点中， a2 的路径 <S2,B1,D,G2> 保持不变15

→以类似的方式生成右子节点。两个子节点都添加到OPEN集17

→最后，选择左子节点进行扩展，并验证底层路径。由于不存在冲突，左子节点被声明为目标节点，并返回其解9

\#表示该步骤对应算法伪代码第几行

此算法是最经典的中心式mapf算法，在众多中心式算法中效能最好，但在实际大型应用场景中，算法计算复杂度将随智能体数量的增多大大增高，因而在应用中具有局限性。

> 参考文献：Sturtevant,Nathan,R,et al.Conflict-based search for optimal multi-agent pathfinding[J].Artificial Intelligence: An International Journal, 2015, 219:40-66.

#### 分布式

在这种方法中，每个机器人都有责任找到可行的路径。每个机器人都将其他机器人视为动态障碍物，并试图计算一个控制速度，以避免与这些动态障碍物发生碰撞。

##### Velocity Obstacles

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MTIxMTM0YjcwZDZhNDdiYTcwZTY2NjI4MWI5NDlhYTJfbkJGMzNRcVB2ZTVYWk02NDV1dkloS01aa3Vla29OdGdfVG9rZW46WTVYM2J5VnBwb05kVFF4VktJRmMwbWE1bmplXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

两个机器人通过使用速度障碍物来选择振荡速度的场景。

（a） 机器人A和B每个人都选择最接近其当前速度的速度，该速度位于另一个机器人引起的速度障碍之外。

（b） 在下一个时间步长中，每个机器人都达到了新的速度，并且由于新速度将其先前的速度留在速度障碍之外，因此它在下一个时间步长中返回到该速度。

速度障碍物假设除自己外，其他机器人不会改变其速度，所以如果所有机器人都使用速度障碍来选择新的速度，那么机器人的轨迹必然会存在振荡。

在基础的 VO 算法中，产生抖动的原因是 A 在第 2 帧选择新速度之后，发现 B 的速度也变化了很多，A 就会认为改回最佳速度（即直接指向目的地的速度）似乎也不会碰撞了，因为 B 的新速度其实就是假设 A 保持最佳速度也能不碰撞的情况下改变的，所以 A 就会认为 B 允许他转回来，但同时 B 也是这么想的。

行动轨迹：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=Yzg2YWQ2Nzg5MzIxMDY4NTZjMzQ0MDRmNjdiZGVkNjBfdlZ4NkZzMlBuS1RPYUZnMjNta0I3d2VqbWhkTElxVzRfVG9rZW46THRCc2IxZnRjbzBxRUt4ZFNGWmNicW1rblBjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

##### Reciprocal Velocity Obstacles

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZjFhNDdkMmY5Yjg3YmEyZTE3ZTNhMDQ1ZmM4ODEyNWNfY2xUYTNtSEVKczdBMjRnOUhTNVQ0Z0JlREExNHAzbk1fVG9rZW46RlRwVGJGbW1ZbzBmb0h4ek13S2M1RWJkbkZkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

假设对方也会使用和我们相同的策略，而非保持匀速运动。然而在 RVO 中，A 把自己的速度只改变 `1/2`，也就是说，我们假设 A 和 B 想要错开，总共需要错开 `10cm`，VO 算法中 A 和 B 都会各自错开 `10cm`，而在 RVO 算法中 A 只错开一半，也就是 `5cm`，同时 A 假设 B 会错开另外一半，B 也是这么想的，因此两者不谋而合，第二帧的时候，两个人各自错开了一半，并且发现此时转回最佳速度依然是会碰撞的（因为每个人只转了一半嘛），因此有效避免了上述抖动的现象。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MzM1YzM0YzVkYTY5ODAzNzIwYTBhOTBkMDU2ZWE4NWNfZTd1aEtPRWc3WkNHN1FPd2lkTElmWXg2dHI2dEt1RlpfVG9rZW46Qzh3RWJVWU5Fb0dkM214cWpVY2N0Mkc4bkVFXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

假设下面一种情况，A 和 B 一左一右的与 C 相向而行，在 RVO 中会遇到什么情况呢？

很简单，A 认为 C 会向右转，因此自己向左转了 `1/2`，而 B 认为 C 会向左转，因此自己向右转了 `1/2`，而 C 实际上两种做法都没有选择，因为在 VO 图中，这实际上是两个锥体摆在自己的面前，所以 C 选择非常努力的向左或者向右转向。

这个时候 A、B、C 三个人就都炸了，因为没有一个的预料是正确的，所以他们在第 3 帧时就会根据一个预料之外的对方速度修改自己的速度，从而形成抖动。

其实原因也很简单，在 RVO 中，所有的智能体都假设对方只考虑自己。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MmEyNzZhZTYzMzgzNTJiZDJkMjdlN2FmOThhM2M3YTRfWVFRejdtd3BtU3VTRXhFdjhCQzJ4Snk1OWd2dklYR2pfVG9rZW46SlZodWJzNzNmbzNZQ0d4eTU5SWNuMWs3bkxoXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

行动轨迹：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=OGY1MzY5YTMyNWNlZDZjNjNhOGMyMjFhYmI4YTE0OTlfdVp3UjJlSHR5RTZZQjJxNDJuNWxUQVdGdlFxWmYyMmxfVG9rZW46S1NySWJuSVpabzZscEt4elh1dWN1M3Rrbm1kXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

##### Hybrid Reciprocal Velocity Obstacles

混合倒易速度障碍物扩大了机器人不应通过的一侧的倒易速度障碍物。因此，如果一个机器人试图从另一个机器人的错误一侧通过，那么机器人必须完全优先考虑另一个机器人，与速度障碍物一样。如果机器人选择了正确的一侧，那么它可以假设另一个机器人的合作并保持同等的优先级，如倒数速度障碍。这大大降低了振荡的可能性，同时不会过度限制每个机器人的运动。

行动轨迹：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NjYxOWQ1MzJiNzA5MjM3MTBkOTkxNDI2NjBiYzIwNWVfbUQycWJIdm9ub0ZQTzNZeGpZdFl4dmJyTTNxdU0zc2lfVG9rZW46QXYycWIxVmNDb0VZdVl4cnRlQWNiMDROblRkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

### 两种MAPF算法的实现

#### cbs算法

cbs算法原理及流程已在3.1.1中介绍，这里给出具体代码：

##### 冲突检测

```Python
def get_first_conflict(self, solution):
    # 计算解决方案中最长路径的时间步长
    max_t = max([len(plan) for plan in solution.values()])
    result = Conflict() # 冲突对象，用于存储冲突的详细信息
    # 遍历每一个时间步长
    for t in range(max_t):
        # 两两组合解决方案中的智能体
        for agent_1, agent_2 in combinations(solution.keys(), 2):
            # 获取智能体在时间步长t的状态
            ...

            # 检查两个智能体在相同时间步长是否在同一位置发生顶点冲突
            if state_1.is_equal_except_time(state_2):
                ...
                return result  # 返回冲突对象

        # 再次两两组合智能体，这次检查是否有边冲突
        for agent_1, agent_2 in combinations(solution.keys(), 2):
            ...
            # 检查是否有边冲突，即智能体1在时间步长t到t+1的路径
            # 与智能体2在相同时间步长的路径交叉
            if state_1a.is_equal_except_time(state_2b) and state_1b.is_equal_except_time(state_2a):
                ...
                return result  # 返回冲突对象
    return False # 如果在所有时间步长中都没有找到冲突，则返回False
```

##### **从冲突生成约束**

```Python
def create_constraints_from_conflict(self, conflict):
    # 初始化一个字典来存储每个智能体的约束条件
    constraint_dict = {}

    # 检查冲突类型是顶点冲突（两个智能体在同一时间占用同一位置）
    if conflict.type == Conflict.VERTEX:
        ...
        # 将顶点约束添加到约束对象中
        constraint.vertex_constraints |= {v_constraint}
        # 将相同的约束对象分配给发生冲突的两个智能体
        constraint_dict[conflict.agent_1] = constraint
        constraint_dict[conflict.agent_2] = constraint

    # 检查冲突类型是边冲突（两个智能体的路径在相邻时间步长交叉）
    elif conflict.type == Conflict.EDGE:
        ...

    # 返回包含所有智能体约束的字典
    return constraint_dict
```

##### **CBS搜索过程**

```Python
def search(self):
    start = HighLevelNode() # 根节点，搜索的起始点
    
    # 初始化起始点的约束字典，每个智能体开始时没有约束
    start.constraint_dict = {}
    for agent in self.env.agent_dict.keys():
        start.constraint_dict[agent] = Constraints()
    
    # 计算起始点的解决方案
    ...
    
    while self.open_set:
        P = min(self.open_set)
        # 将选中的节点从开放集合中移除，添加到封闭集合
        self.open_set -= {P}
        self.closed_set |= {P}

        # 更新环境的约束字典为当前节点的约束字典
        self.env.constraint_dict = P.constraint_dict
        
        # 检测当前解决方案中的第一个冲突
        conflict_dict = self.env.get_first_conflict(P.solution)
        # 如果没有冲突，说明找到了解决方案
        if not conflict_dict:
            print("Solution found. All agents reached the goal!")
            return self.generate_plan(P.solution)

        # 根据冲突生成新的约束字典
        constraint_dict = self.env.create_constraints_from_conflict(conflict_dict)

        # 遍历新生成的约束字典中的每个智能体
        for agent in constraint_dict.keys():
            ...
            # 尝试在新节点的约束条件下计算解决方案
            new_node.solution = self.env.compute_solution()
            if not new_node.solution:
                continue
            ...
            if new_node not in self.closed_set:
                self.open_set |= {new_node}

    return {} 
```

#### primal算法

各个agent的决策都是有益于全体的，单agent的决策靠的是RL + 模仿学习

##### observation space

代理只能在以自己为中心的有限FOV（视场角）中观察世界状态（实际上是10 x 10 FOV）

代理需要访问有关其目标的信息，该信息通常不在其FOV范围内。为此，它始终可以访问指向其目标的单位向量和到其目标的欧几里得距离

在有限的视野中，我们将可用信息分成不同的渠道以简化座席的学习任务（如图）：

- Obstacles:每个观察值均由代表障碍的二进制矩阵，将在FOV视野之外的所有位置都将添加障碍
- Agents':agent拥有FOV内其他agent的位置（如果位于视野内）组成
- Neighbors' goals:其他可观察agent目标的位置
- agent's goal:指向其目标的单位向量

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NGZkOWYzM2FiZmFlMDlmZTE0M2M4ODJjZDk1N2JjNjJfUWs4U0E2eFprTE9CYkpXdWQ3NkR0bFVWSm5EMXR4UmVfVG9rZW46S3dQdWJJMWRjb0RCd0N4YnZqNGNSbHFHbkxjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

##### action space

**action**：上下左右或者不动 存在无效action（比如移动到障碍物/其他agent，移回到上一个单元格），规定只在有效action中采取行动（比给无效action一个负反馈，效果更好）。 鼓励探索，禁止agent返回上一个位置，可以静止不动

##### reward structure

上下左右移动：-0.3

agent相互碰撞：-2.0

静止不动：已到达目标点，不扣分；未到达，-0.5（惩罚更多为了鼓励探索）

完成一次迭代（所有agent都顺利找到目标）：+20

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=Mzg3NmY3NDY1M2I1MjBhMTc5NzZmYjViYzUyNmY5MjVfQXRhYnN1eDBNN1hSNW9rUGc1QWZPUFVoczU2dXJvbU1fVG9rZW46VEs5a2JaNnJGb3BUMkZ4eFN5YWNvejVabldkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

##### actor-critic network异步优势算数（A3C）算法

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NGY1MThmOWI4MTAyMzExOTA2ZTY5MTEzOGIxNWE4ZmVfTkFPSHd1Qnh2N3BCNDdJMU9YZDhxcTFUbXIxUmh2S2dfVG9rZW46UXVibmIxODFXb2NPZWh4SUpsTWNJdjc2bkNoXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

神经网络的两个输入——本地观察和目标方向/距离——在进入神经网络的中间位置之前会被独立预处理，然后进行连接。

- 表示本地观察的四通道矩阵（observation space）通过三个卷积和最大池化的两个阶段，然后是最后一个卷积层。
- 与此同时，目标单位向量和大小（goal position）通过一个全连接层传递。
- 这两个预处理输入的连接然后通过两个全连接层，最终输入到一个输出大小为512的长短期记忆（LSTM）单元中。
- 一个残差捷径连接（resident shortcut）连接层的输出到LSTM的输入层。
- 输出层包括带有softmax激活的策略神经元、值输出（policy，value）以及用于训练每个智能体是否阻碍其他智能体到达目标的特征层（blocking）。

在训练过程中，每隔n = 256步或当一个episode结束时，policy、value和blocking输出会进行批量更新。通常情况下，通过最小化来更新价值，以匹配总折扣回报Rt。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=Yjg5ZGIzZDg2MDUzYWFhMjYxYTU1NzFlMmNjNjdlOTFfSVc0ODhWbHhkM1NhTkNQSkJPMWY4ZTBhakR1Z2ZpWWJfVG9rZW46T255SGJBM2ZXb0p6Y3p4cUt4S2NkYmVYblhlXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=Zjk1OThhMzdmNDdkMmY5ZGJlNmZkZTk0ZGQ4OGJhZDNfMjdsb3JXSnFGcVJYMUFVRXp6T1VMM1ZVelp0SDhWd25fVG9rZW46UGNGOGJKSjdmb0E4ZDh4WmpVcWNaclZDbnNiXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

为了更新策略，我们使用值函数通过自助法（bootstrapping)来近似优势函数。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MjUxNjRkZGUyY2EwMjdmNzBmOWY0MjVhMGExMmUzNDRfQzlKdGZlUjRZUnZoNHI5anpKQUkyNTVJV3dIZU5udFVfVG9rZW46QmtFQ2JrSDVkb0dzSXp4TjJWUmNER1hxbjliXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

同时，我们添加了一个熵项到policy损失中，这已经被证明通过惩罚总是选择相同行动的策略可以鼓励探索和阻止过早收敛。

策略损失:

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NWYxMTU1MWUyNDBmMWU5ODc1Yjk1MGFmYjA4MGE2ZjdfUk5RckFZU0trUWFBTG5iR2U2QXV1VzNQOWFxMzNianFfVG9rZW46T2FYQWJ2N3k5bzJ4SXR4cFlrQmNsb1ZzbjlnXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

策略损失中包括一个小的熵权重(在实践中值为0.01）。我们依赖于另外两个损失函数来引导和稳定训练。首先，blocking预测输出通过最小化 错误预测的对数似然进行更新。其次，我们定义了一个损失函数来最小化选择无效移动的对数似然。

##### Learning

##### blocking penalty

如果agent阻碍了别的agent到达目标或是延迟了其他agent的步数，惩罚扣分。

使用A*算法估计路程，如果说去掉所有的agent后单agent的路程变少了十步及以上，那就视为blocking

##### combining RL&IL

RL允许探索，IL能快速找到高质量区域

在episode的开始，随机抽签决定本次episode是基于 RL 还是基于 IL的，并相应地设置了“开关”（switch）。

对于RL，在每个时间步长，每个agent （1， ..， n） 从学习环境中提取其观察值 oi 和其先前动作的奖励值 ri，并使用观察值通过自己的神经网络副本选择要进行的动作 ai。

不同代理的操作以随机顺序依次执行。由于智能体经常从共享的神经网络中推拉权重，因此它们最终在各自的网络中共享相同的权重。

对于 IL ，专家集中计划器在事件期间协调所有智能体，智能体学习模仿其行为，使他们能够学习协调行为。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ODg2MmY3ZDdjOWM3ZDZkYjY5YTQ5NWY2ODk0MGIzODVfakhRSDQ2WU5ySkxiNWtLTW1qcTVtV1FWbkVKVEZobkVfVG9rZW46RVZNZ2JzMEdGb2dzaGV4UHp4SGNMbG1KbnVlXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

##### environment sampling

在训练期间，我们在每个episode开始时随机化世界的大小和障碍物密度。我们从一个偏好较小且较密集的环境的分布中进行采样，迫使智能体学习协调，因为它们更频繁地经历智能体-智能体交互。

##### 参数

折扣因子设定为0.95，这意味着未来奖励的折现程度相对较小，但仍然对当前奖励有一定的影响。代理更多地关注即时奖励，但仍会考虑未来奖励的贡献，以便更好地制定策略。这种设置在训练过程中可以促进代理更加注重当前的行动，同时保留了一定程度的长期规划能力。

每个情节（episode）的长度为256个时间步。情节的长度决定了每次训练中代理与环境互动的时间长度。

 在这里，批量大小设定为128，这意味着每个代理在每个情节中最多进行两次梯度更新。

观察到演示的概率为50%意味着在每个训练情节中，有一半的概率agent会观察到专家演示。通过观察专家的演示，代理可以学习到专家的高效行为策略，并尝试模仿这些策略以改进自己的性能。

使用Nadam优化器，学习率初始值为2 × 10^(-5)，并且随着情节数量的增加而按比例下降，下降速率与情节数量的倒数平方根成正比。这样的设置可以使学习率随着训练的进行逐渐减小，有助于更稳定地训练模型。

##### **训练过程可视化**

以下是我们训练到一万三千次的训练数据：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZmE3OTgwNTM2NjQ1NmU3OWY0OThjZTRiOWU0YzkxZTdfRGxNNnNHM05DSXBmVmhkc2RiMGd2VFF3b3YyWUtEUTFfVG9rZW46R0kxbmJWMklVb2E4Ynh4b3gwQWNuRnhjbkNjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MjgzMjgxZDljMDg3NWE0NzJkMGQ2Y2ZlZWI3MjE0NjdfeExkTVloaVFxTUtGQ1lSc1Q5RnZzcHNYQ2VXZFkwUTZfVG9rZW46Tkk1dGJ1d1lkbzRzTUl4UmlyQ2NONVV0bktiXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

可见随着训练次数的增多模型的loss递减，reward递增，但reward仍然没有到达正数，说明十万多次训练出的模型为智能体找到终点的次数仍然不多。这已经是我们训练了10天的结果，按照目前的趋势发展下去，达到预期效果至少还需要训练20天。后来我们想起有老师曾说过模型训练一般不从头开始，在已有的预训练模型上继续训练才有意义，于是我们采用primal预训练模型（训练了三十万次），在其基础上继续训练，并将训练后的模型投入我们最终的项目使用。

## 四、2D与3D可视化

#### 实现总流程

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NzljOGVkMTc1NjUxMjIxOWUzZDI1OTg0NmU0MTI3NTlfU0Y1dkxPTDFBem4zbnZSakRBajR3QWF3Zm91OUliNVJfVG9rZW46TEJaN2JvRzF2b0p2b0p4a1ZFRGNEUGNObmJjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

第一步：根据初始设置，初始化2D地图，显示在UI界面供用户进行选择

第二步：根据2D地图和用户通过UI界面进行的初始选择，对智能体和地图的数值进行初始化

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=M2RlMGFmZmUxOWZkNjM3Yzc5M2M2MTExMGVkZTJmNjJfYVNCd0VLQWVtMVRYTThiN2kwMkxuaFJtNlhwbmtVZlJfVG9rZW46QTVNamJ1bnVQbzkzSW54NGNodWNwbFdGbmljXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

第三步：根据智能体和地图信息，运用cbs或primal算法，计算多智能体移动路径

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MTEzMWNhZjg5ZWVjNDRhY2ZiYTEwY2I4MTEwMWFkOTVfMVBQZUtud0JnVUYzRDZtcFFjaENlYzk3QUl3YktOWEdfVG9rZW46VG55MGJWM2ZTb284TGR4T1FCeWM5RUtKbjBjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

第四步：根据智能体和地图信息，初始化3D场景；根据计算出的智能体路径，实现运动动画可视化演示

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZTg2MmY3OTBmNTU3NmQ5MGNjZTEyOWNhMWQ2ZWVlMWJfMUNPUkk3dDVJU01QQWc5MnJVSFFOOXJhdFBqSVo0VzZfVG9rZW46TGNBbWJRWjhQb2xod2p4UVFla2Nxc2lzbk1lXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

### 场景设置和交互选择

我们为项目设置了UI界面，这为用户提供选择场景设置的功能。用户可以自己选择每个智能体的取货点、送货点（包括货架层数）、路径规划算法，通过自定义的设置，扩大功能应用场景，使项目产品更容易被投入现实使用，提高了用户可操作性，赋予了项目真实的落地意义。该功能主要包括两个UI界面：主界面与用户选择页面。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ZjJlMjIzNGU4YTRjNTQ3NTkzNjFmZGU2OTVlMTRiYjJfdlpFQWdYdHBkTm1iSUFhT1hXZDJVUW1tY29MMnFrbmVfVG9rZW46TWZnSmJFOW9Eb2U4Q0p4c01nNmN6YlNCbkdlXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MDM3NTc1MzFhMWY1ODIwZDgyOTY2MTVhYTEzYWZlNmRfSTVRQmZiTXlPMVlWeXBySVoxV0pPTjROVDJORzRMMWZfVG9rZW46S0pzQmI1TGZMb1MwaDN4bU9USGNwU2wzbndjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

#### 场景设置说明：

- 场景模拟：我们实现了一个仓储地图的2D显示，使用方格化的显示方式来展示实际地图，每一个小格为一个实际物体单位。
- 障碍物设置：在场景中，包括三种障碍物：可被搬运障碍物（取货点）、不可移动杂物（固定障碍）、空闲货架位置（货物目的地），使用三种颜色来标识不同的障碍物，灰色为不可移动杂物，橙红色为可被搬运障碍物，黄色为可选的货架位置。
- 智能体设置：我们使用了小车图片来模拟智能体的位置，显示了可被调度的智能体起点位置。
- 取货点选择：当在按钮取选中“pickup”模式时，代表进入取货点布置模式。用户通过点击不同的取货点位置来设置取货点，每一个取货点被选中后会显示“旗子”标志。
- 目的地选择：当在按钮选中“destination”模式时，代表进入目的地布置模式，并且显示出可选的货架层数。用户通过点击不同的目的地位置来设置卸货点，选中卸货点后点击层数按钮，为每个卸货点定义具体的卸货位置。当具体层数被选中后，会显示在对应的格子上。
- 算法选择：提供“CBS”与“PRIMAL”两种算法，用户可以自行选择本次要用哪种算法来进行计算。
- 加载场景：当选择完所有的初始化信息后，点击“GO”即可切换至3D场景。

#### 用户选择界面详细实现步骤：

使用来自unity资源库的SLIMUI库构建基本的按键风格和界面样式，然后加入我们所需要的网格化地图与按钮功能。我们的主要功能脚本包括三个脚本文件，分别是UIMenuManager.cs（控制按钮交互与切换）、GridManager.cs（网格生成与现实）、CellClickHandler.cs（小格点击事件控制）。

- UIMenuManager.cs：定义画面中需要使用到的按钮，并为每个按钮绑定点击事件。将该脚本挂载在camera对象上，并将对应按钮与脚本文件中的public成员变量一一绑定，从而实现按键控制。在这里我们需要注意将层数选择按钮添加显示具体数字的函数，通过在改脚本中引用类CellClickHandler来获取被选中的终点位置来实现。
- GridManager.cs：该脚本需要与unity中的网格生成组件配合使用，我们可以只定义一个游戏对象，通过clone的方式复制所需要的个数（40*40）来完成网格化生成，并把每一个格子实例化，使其可以被操作。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NjQ1M2FmZTA1OWVhNDQ4NTNjYTExMDIzZDkyYjI3ZWRfMWU4TXVvZnk1Uk1CcXpncEpOWlFXZDJLWUpzSTJFZGlfVG9rZW46WGk1ZGI2UGFGb1ZKekR4cjFhQWN3UUlwbkhoXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

 在初始化网格生成时，我们需要读取已经设定好的init.yaml文件来读入障碍物与小车的初始信息。通过 InitializeObstaclesAndCars函数的实现，我们读入了初始化信息，并且将地图上的每一个位置设置成了对应类型，包括四种类型：0为空位置，1为可被选择的取货点，2为货架空位置即终点、3为固定障碍物。以读取障碍物3为例：

```C#
 // 提取simple_obstacles
 var simpleObstaclesList = map["simple_obstacles"] as List<object>;
 foreach (var obstacle in simpleObstaclesList)
 {
     var obstacleData = obstacle as Dictionary<object, object>;
     var coordinate = obstacleData["coordinate"] as List<object>;
     // 提取坐标并存储到obstacles列表中
     int x = Convert.ToInt32(coordinate[0]);
     int y = Convert.ToInt32(coordinate[1]);
     obstacles[x, y] = 3; // 3代表simple_obstacles
 }
```

- CellClickHandler.cs：实现点击网格地图中的任一格子触发对应事件的功能。该脚本类包括两个重要的功能函数，分别是OnPointerClick（实现点击放置功能）与OnLayerSelected（实现层数选择功能）。具体来说，在OnPointerClick函数中，首先我们需要判断点击的格子的不同类型，分别是“pickup”与“destination”，然后在不同的模式下，实现当获取到点击时显示对应图标或颜色标志，并将位置信息存入结果数组中。在OnLayerSelected函数中，需要实现层数标识显示，当选中层数后将其显示在对应网格位置上。

 在该脚本中定义一个存储所有智能体完整路径信息的类（AgentData），当所有选择完成后，将结果信息对应存储到该类的列表中，构成能够被保存到让python程序读取的input文件中的智能体完整信息列表。类的具体定义如下：

```C#
 // 定义一个类表示智能体数据
 public class AgentData
 {
     public Vector2Int pickupPoint;//取货点
     public Vector2Int endPoint;//货物要到达的目的地
     public Vector2Int agentgoPosition; //智能体实际要到达的位置
     public int layer; //货架层数
 }

 // 创建一个列表来存储智能体数据
 public List<AgentData> agentList = new List<AgentData>();
```

### unity与python通信

#### 通信思路

- **Unity：**C#脚本中对Python程序建立socket通信请求，调用cbs或primal算法的python程序
- **python：**接受来自unity的通信信息，执行相应程序
- **数据交互媒介：**
  - **yaml文本文件：**python程序的运算结果，可以保存在yaml文件中，unity可以直接访问该yaml文件读取运算结果。例如，unity通信请求执行cbs程序，cbs程序将计算出的智能体路径保存在output.yaml中，unity可以读取该文件内容，执行智能体移动动画。
  - **通信字段：**unity和python可以直接通信传递字符串信息。例如在有智能体到达取货地点后，unity请求primal程序重新计算各智能体路径，在请求primal程序执行的同时，可以通过通信传递智能体当前位置和到达取货点的智能体编号，供python程序更新信息。
  - **控制台信息：**python程序可以通过print输出信息到控制台，unity可以通过捕获控制台信息，确定python程序是否执行完成及执行结果。例如，当cbs程序运算出智能体路径后，输出"all agent arrived"，unity捕获到控制台显示该信息后，结束暂停状态，使智能体继续运动。

#### 通信主要步骤

- **设置Cmd命令：**
  - 激活Conda虚拟环境并调用Python文件
- **创建ProcessStartInfo：**
  - 设定执行cmd
  - 设置重定向进程的标准输出流，用于直接被Unity C#捕获，从而实现 Python -> Unity 的通信
  - 设置重定向这个进程的标准报错流，用于在Unity的C#中进行Debug Python里的bug
- **创建进程：**
  - 设置异步输出的回调函数，用于实时输出Python中的Print和报错内容到Unity的Console
  - 启动脚本Process，并且激活逐行读取输出与报错
- **捕获输出：**
  - 捕获标准输出和捕获报错
- **进行UDP通信：**
  - unity：创建UDP通信的Client并设置IP地址与端口号然后在Update()函数中可以添加发送指令的代码
  - python端：使用socket库设置监听端口，然后设置IP和端口并启动监听
- **进程查杀：**
  - 由于Unity退出后Python进程并不会自动退出，需要在C#中调用Python程序前和关闭Unity后都手动关闭Python进程。不然会造成端口占用，下一次无法打开端口进行监听。

#### 通信流程

- **初始化时的通信流程：**

暂时无法在飞书文档外展示此内容

- **智能体到达取货点时的通信流程：**
  - 当有智能体到达取货点时，unity通信请求执行update.py程序，并传递智能体当前位置和到达智能体的全部编号。
  - update.py根据通信消息对input.yaml的内容进行更新。
  - unity端从控制台接收到更新完成的信息，请求执行cbs.py或primal.py程序。
  - cbs.py或primal.py重新计算路径，输出到output.yaml中。
  - unity端从控制台接收到路径计算结束的信息，从output.yaml读取智能体路径信息，然后运行3D动画。

### 3D场景建模（智能体运行动画）

#### 货运小车移动动画

控制移动的脚本，PalletrobotMove.cs：控制单个货运小车按照给定路径数组movepoints运动，在移动方向改变时实现转向动画

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NzZiZjQyZjlkNTUxMDhmZGY2NmE4OTY1OTRkODllMjBfa0VkcG15R2p1eHc3OU9pbU44ZVRtaTVGTHBtd2s5eWNfVG9rZW46SWZaSmJiNUprbzhoQ054SXZScGN2TTBrbjdnXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

将上面编写好的脚本挂载到货运小车对象上即可实现小车移动。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NTc4NDFhYjczZGU2MGVjYzk2MDkzYzQxYTcxYmQ1ZjVfdFdKQXpwU1k0MkhrdGQyZmlxbmtLeER4cjB6eVQ2MG5fVG9rZW46VHdVaWJIN3BvbzR3V1R4QU9xY2NKM0FpbnFoXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

#### 装货、卸货动画实现

智能体到达当前路径数组终点（即到达取货点/卸货点）时，通过碰撞体检测探查货物，并将货物绑定或解绑，脚本实现如下：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=ODdjNmI3MzI0YWNkMzBjNjlkYzg2NzAwMGU1MTdhOWJfOGlTQ1JRSUh5Wkt3NTlSMXJSN0pIRHJNV0V4d2xIeEVfVG9rZW46VUV4ZWJFN1llb3FhRnd4TWFDMmMzeWp1bk1oXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

#### 运输暂停、更新路径计算

为实现此功能在PalletrobotMove.cs和Pyrun.cs里做了一些更改：

1. PalletrobotMove.cs里的更改：

加了一个全局变量：

```Java
// 全局暂停标志
public static bool isPaused = false;
```

在Update（）中增加语句：

```Java
// 如果暂停标志为true，则不进行任何操作，Line16
if (isPaused)
{
    return;
}

// 检查是否为最后一个目标点，Line37
if (currentIndex == movePoints.Length)
{
    // 暂停所有智能体
    PyRun.Instance.PauseAllAgents();
}
```

1. Pyrun.cs里的更改：

增加两个函数：

```Java
//这个函数在PalletrobotMove.cs中当智能体走到终点时，被调用
//会将PalletrobotMove中的isPaused设为真从而让所有智能体暂停移动
public void PauseAllAgents()
//这个函数会读取input中的内容，获取当前各智能体的位置，更新到input中
public void SaveAgentPositions()
```

同时在input.yaml中做出修改：

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NzI2MTYzMzI2ZDA5M2M3MzQyYTJjNmNlYmYwMDEwZDlfZTQ4Q2NoQXZmUEJldmp3N1RxSG9uRzB3d0lyanFDWGJfVG9rZW46WGs1cmI4RXZHb3NsTFV4bTNmU2NJeXBOblNjXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

每次地图更新后，start是智能体当前位置。对于到达目标的agentArrived号智能体，将next的值赋值给goal，即此时goal变成该智能体卸货地点。

#### 智能体路径分配

有一个PyRun.cs脚本负责收集地图信息传给Python计算路径，并将路径分配给各小车即PalletrobotMove对象，在PyRun.cs中，我们设计一个智能体移动类型的数组，数组中每个元素与提取到的路径信息一一对应，并对应绑定智能小车元素。然后每个智能体按序执行读取到的路径点，即可实现规划路径移动。

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=MTg0ODU5N2E5ZGY0YTljY2Y4NTk3ZjFlOTMyYWFjZmNfZGhhTDQwTFFwM0lCc3ppbkYzazV0ZFBQdVRjZ0ZrVzBfVG9rZW46WDhHaGJqQXVHb1lvRnh4a1pPaGNrRDBhbmVkXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

![img](https://vvi5f2s0tcx.feishu.cn/space/api/box/stream/download/asynccode/?code=NDM3ZWE2ZWE5N2M2ZDI2M2U4MjkyNmJhOTU0YTI0MjRfanA5UEpjV3l1aUFtV3RxWjN5bWdpUTd2M0xBVElSNXNfVG9rZW46QzZzWmI5Tzl0b2g4YW14aXhDM2NMZUxlbmNSXzE3MTg2MTU0OTY6MTcxODYxOTA5Nl9WNA)

## 五、项目优点与缺点

### 项目优点

- **可实践的UI控制：**
  - **丰富项目应用场景：**项目提供了自定义的UI设置，使用户可以在不同场景中选择不同的路径规划过程，便于进行多智能体路径规划的实验和测试，提升了项目的实用性和多样性。
  - **扩大用户可操作空间：**用户界面设计直观友好，用户可以通过界面进行各种操作和配置，增强了用户的操作体验和灵活性。
- **多算法选择：**
  - **实现了两种算法（cbs与PRIMAL）且可选择：**项目实现了CBS（冲突基于搜索）和PRIMAL（优先级继承多智能体路径规划）两种算法，用户可以根据需求选择合适的算法进行路径规划，从而提高了项目的适用性和灵活性。
  - **unity与python分离，使项目可扩展性变强：**项目采用Unity进行3D建模和界面控制，而路径规划算法部分则使用Python实现。这样的架构设计使得项目具有良好的扩展性和可维护性，便于未来功能的增加和算法的更新。
- **3D场景建模：**
  - **真实仓库建模实现：**项目中使用了真实仓库的3D建模，细节逼真，使得模拟结果更加贴近实际应用场景，增加了项目的现实意义和实用价值。
  - **动画细节完整：在**路径规划执行过程中，动画细节处理得当，表现出色，增强了用户的视觉体验和理解效果。比如智能车搬运货物到具体货架位置的动画、智能车与货物移动与转向动画等等。
  - **不同视角选择：**用户可以在不同的视角下观察多智能体的路径规划和执行过程，包括俯视图、侧视图和第一人称视角等，从而提供了全方位的观察和分析功能，进一步提升了用户体验。

### 项目缺点

- **算法计算速度不够快：无论是CBS还是PRIMAL都需要等待几秒钟后才能给出路径规划结果，但此时的地图大小只有40\*40，智能体数量也只有8个；当投入真实场景应用后，地图大小和智能体数量都会呈倍数级往上增长期，此时系统的可使用性会受到严重影响。
- **Python与Unity通信存在延迟：**在进入Unity动画后，智能体小车每移动几步就会出现卡顿，这是因为通信无法即时快速反馈，这对于用户观看体验感会造成影响。
- **货物层数选择无限制：**目前对于货物层数的选择即为货架层数，这就意味着这列的货架上必须为全空，才能进行货物层数选择，否则可能会选到存在货物的货架。
- **初始化文件需自行编写：**初始化的input.yaml文件中关于地图、智能体、障碍物的相关信息都是人为通过计算Unity3D项目中的格子坐标，从而得到的结果，非常耗费人力和时间；如果要对地图进行修改，重新在input.yaml文件中找到对应物体的位置都十分困难。
- **项目无法打包**：由于Unity项目主要用c#语言开发，故而算法部分使用C++,才能进行项目完整打包输出可执行文件；另外，PRIMAL算法是基于conda环境下，对演示电脑也存在一定的要求。
