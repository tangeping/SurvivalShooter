using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public abstract class AbstractLockstep : CBSingleton<AbstractLockstep>
    {

        private enum SimulationState // 模拟状态
        {
            NOT_STARTED, // 未启动
            WAITING_PLAYERS, // 等待玩家
            RUNNING, // 运行中
            PAUSED, // 暂停
            ENDED // 结束
        }
        private AbstractLockstep.SimulationState simulationState;

        internal Dictionary<int, SynPlayer> players;

        private SynPlayer localPlayer;


        internal SynPlayer LocalPlayer
        {
            get
            {
                return localPlayer;
            }

            set
            {
                localPlayer = value;
            }
        }
        //------------ReplayMode--------
        private ReplayMode replayMode; // 重放模式

        //private ReplayRecord replayRecord; // 重放记录
        //-----------------

        //-----------call back----------------------
        protected SyncUpdateCallback StepUpdate;

        private SyncInputCallback GetLocalData; // 获取本地数据

        internal SyncInputDataProvider InputDataProvider;

        private SyncEventCallback OnGameStarted;

        private SyncEventCallback OnGamePaused;

        private SyncEventCallback OnGameUnPaused;

        private SyncEventCallback OnGameEnded;

        private SyncPlayerDisconnectionCallback OnPlayerDisconnection;

        public SyncIsReady GameIsReady;

        //----------end----------------------------

        //         public static AbstractLockstep NewInstance(
        //             IPhysicsManagerBase physicsManager, 
        //             SyncEventCallback OnGameStarted, 
        //             SyncEventCallback OnGamePaused,
        //             SyncEventCallback OnGameUnPaused,
        //             SyncEventCallback OnGameEnded, 
        //             SyncPlayerDisconnectionCallback OnPlayerDisconnection,
        //             SyncUpdateCallback OnStepUpdate,
        //             SyncInputCallback GetLocalData, 
        //             SyncInputDataProvider InputDataProvider)
        //         {
        //             bool flag = rollbackWindow <= 0 || communicator == null;
        //             AbstractLockstep result;
        //             if (flag)
        //             {
        //                 result = new DefaultLockstep(deltaTime, communicator, physicsManager, syncWindow, panicWindow, rollbackWindow, OnGameStarted, OnGamePaused, OnGameUnPaused, OnGameEnded, OnPlayerDisconnection, OnStepUpdate, GetLocalData, InputDataProvider);
        //             }
        //             else
        //             {
        //                 result = new RollbackLockstep(deltaTime, communicator, physicsManager, syncWindow, panicWindow, rollbackWindow, OnGameStarted, OnGamePaused, OnGameUnPaused, OnGameEnded, OnPlayerDisconnection, OnStepUpdate, GetLocalData, InputDataProvider);
        //             }
        //             return result;
        //         }

        // Update is called once per frame
        public void Update()
        {
            
            if (simulationState == AbstractLockstep.SimulationState.WAITING_PLAYERS) // 等待玩家
            {
                this.CheckGameStart();
            }
            else if(simulationState == AbstractLockstep.SimulationState.RUNNING)
            {

            }
        }

        // 检查游戏是否可以开始
        private void CheckGameStart()
        {
            bool flag = this.replayMode == ReplayMode.LOAD_REPLAY;
            if (flag) // 加载回放模式
            {
                this.RunSimulation(false);
            }
            else // 非加载回放模式
            {
//                 bool flag2 = true;
//                 int i = 0;
//                 int count = this.activePlayers.Count;
//                 while (i < count)
//                 {
//                     flag2 &= this.activePlayers[i].sentSyncedStart;
//                     i++;
//                 }
//                 bool flag3 = flag2;
//                 if (flag3)
//                 {
//                     this.RunSimulation(false);
//                     SyncedData.pool.FillStack(this.activePlayers.Count * (this.syncWindow + this.rollbackWindow));
//                 }
//                 else
//                 {
//                     this.RaiseEvent(196, SyncedInfo.Encode(new SyncedInfo
//                     {
//                         playerId = this.localPlayer.ID
//                     }));
//                 }
            }
        }


        private void Run()
        {
            bool flag = this.simulationState == AbstractLockstep.SimulationState.NOT_STARTED;
            if (flag) // 未开始
            {
                this.simulationState = AbstractLockstep.SimulationState.WAITING_PLAYERS;
            }
            else // 非NOT_STARTED状态
            {
                bool flag2 = this.simulationState == AbstractLockstep.SimulationState.WAITING_PLAYERS || this.simulationState == AbstractLockstep.SimulationState.PAUSED;
                if (flag2) // 正在等待玩家 或 暂停状态
                {
                    bool flag3 = this.simulationState == AbstractLockstep.SimulationState.WAITING_PLAYERS;
                    if (flag3) // 正在等待玩家
                    {
                        this.OnGameStarted();
                    }
                    else // 暂停状态
                    {
                        this.OnGameUnPaused();
                    }
                    this.simulationState = AbstractLockstep.SimulationState.RUNNING; // 改为运行状态
                }
            }
        }

        // 执行模拟
        public void RunSimulation(bool firstRun)
        {
            this.Run();
            bool flag = !firstRun;
            if (flag) // 不是第一次运行
            {
//                 this.RaiseEvent(197, new byte[]
//                 {
//                     1
//                 }, true, this.auxActivePlayersIds);
            }
        }
    }
}
