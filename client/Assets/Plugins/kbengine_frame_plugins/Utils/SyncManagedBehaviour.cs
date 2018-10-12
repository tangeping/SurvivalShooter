using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public class SyncManagedBehaviour : ISyncBehaviourGamePlay, ISyncBehaviourCallbacks, ISyncBehaviour
    {
        public ISyncBehaviour SyncBehavior;

        [AddTracking]
        public bool disabled;

        public PlayerInfo localOwner;

        public PlayerInfo owner;

        public SyncManagedBehaviour(ISyncBehaviour s)
        {
            this.SyncBehavior = s;
        }

        #region ITrueSyncBehaviourGamePlay 接口方法
        public void OnPreSyncedUpdate()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourGamePlay;
            if (flag)
            {
                ((ISyncBehaviourGamePlay)this.SyncBehavior).OnPreSyncedUpdate();
            }
        }

        public void OnSyncedInput()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourGamePlay;
            if (flag)
            {
                ((ISyncBehaviourGamePlay)this.SyncBehavior).OnSyncedInput();
            }
        }

        public void OnSyncedUpdate()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourGamePlay;
            if (flag)
            {
                ((ISyncBehaviourGamePlay)this.SyncBehavior).OnSyncedUpdate();
            }
        }
        #endregion ITrueSyncBehaviourGamePlay 接口方法

        #region ITrueSyncBehaviour 接口方法
        //         public void SetGameInfo(TSPlayerInfo localOwner, int numberOfPlayers)
        //         {
        //             this.SyncBehavior.SetGameInfo(localOwner, numberOfPlayers);
        //         }
        #endregion ITrueSyncBehaviour 接口方法

        #region 生命周期方法
        // 开始同步
        public static void OnGameStarted(List<SyncManagedBehaviour> generalBehaviours, Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer)
        {
            int i = 0;
            int count = generalBehaviours.Count;
            while (i < count)
            {
                generalBehaviours[i].OnSyncedStart();
                i++;
            }
            Dictionary<int, List<SyncManagedBehaviour>>.Enumerator enumerator = behaviorsByPlayer.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, List<SyncManagedBehaviour>> current = enumerator.Current;
                List<SyncManagedBehaviour> value = current.Value;
                int j = 0;
                int count2 = value.Count;
                while (j < count2)
                {
                    value[j].OnSyncedStart();
                    j++;
                }
            }
        }

        // 游戏暂停
        public static void OnGamePaused(List<SyncManagedBehaviour> generalBehaviours, Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer)
        {
            int i = 0;
            int count = generalBehaviours.Count;
            while (i < count)
            {
                generalBehaviours[i].OnGamePaused();
                i++;
            }
            Dictionary<int, List<SyncManagedBehaviour>>.Enumerator enumerator = behaviorsByPlayer.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, List<SyncManagedBehaviour>> current = enumerator.Current;
                List<SyncManagedBehaviour> value = current.Value;
                int j = 0;
                int count2 = value.Count;
                while (j < count2)
                {
                    value[j].OnGamePaused();
                    j++;
                }
            }
        }

        // 取消暂停
        public static void OnGameUnPaused(List<SyncManagedBehaviour> generalBehaviours, Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer)
        {
            int i = 0;
            int count = generalBehaviours.Count;
            while (i < count)
            {
                generalBehaviours[i].OnGameUnPaused();
                i++;
            }
            Dictionary<int, List<SyncManagedBehaviour>>.Enumerator enumerator = behaviorsByPlayer.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, List<SyncManagedBehaviour>> current = enumerator.Current;
                List<SyncManagedBehaviour> value = current.Value;
                int j = 0;
                int count2 = value.Count;
                while (j < count2)
                {
                    value[j].OnGameUnPaused();
                    j++;
                }
            }
        }

        // 游戏结束
        public static void OnGameEnded(List<SyncManagedBehaviour> generalBehaviours, Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer)
        {
            int i = 0;
            int count = generalBehaviours.Count;
            while (i < count)
            {
                generalBehaviours[i].OnGameEnded();
                i++;
            }
            Dictionary<int, List<SyncManagedBehaviour>>.Enumerator enumerator = behaviorsByPlayer.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<int, List<SyncManagedBehaviour>> current = enumerator.Current;
                List<SyncManagedBehaviour> value = current.Value;
                int j = 0;
                int count2 = value.Count;
                while (j < count2)
                {
                    value[j].OnGameEnded();
                    j++;
                }
            }
        }

        // 玩家断开连接
        public static void OnPlayerDisconnection(List<SyncManagedBehaviour> generalBehaviours, Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer, byte playerId)
        {
//             int i = 0;
//             int count = generalBehaviours.Count;
//             while (i < count)
//             {
//                 generalBehaviours[i].OnPlayerDisconnection((int)playerId);
//                 i++;
//             }
//             Dictionary<int, List<SyncManagedBehaviour>>.Enumerator enumerator = behaviorsByPlayer.GetEnumerator();
//             while (enumerator.MoveNext())
//             {
//                 KeyValuePair<byte, List<SyncManagedBehaviour>> current = enumerator.Current;
//                 List<SyncManagedBehaviour> value = current.Value;
//                 int j = 0;
//                 int count2 = value.Count;
//                 while (j < count2)
//                 {
//                     value[j].OnPlayerDisconnection((int)playerId);
//                     j++;
//                 }
//             }
        }
        #endregion 生命周期方法

        #region ISyncBehaviourCallbacks 接口方法
        // 开始同步
        public void OnSyncedStart()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourCallbacks;
            if (flag)
            {
                ((ISyncBehaviourCallbacks)this.SyncBehavior).OnSyncedStart();
//                 bool flag2 = this.localOwner.Id == this.owner.Id;
//                 if (flag2) // 本地玩家
//                 {
//                     ((ISyncBehaviourCallbacks)this.SyncBehavior).OnSyncedStartLocalPlayer();
//                 }
            }
        }

        // 开始同步本地玩家
        public void OnSyncedStartLocalPlayer()
        {
            throw new NotImplementedException();
        }

        // 游戏暂停
        public void OnGamePaused()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourCallbacks;
            if (flag)
            {
                ((ISyncBehaviourCallbacks)this.SyncBehavior).OnGamePaused();
            }
        }

        // 取消暂停
        public void OnGameUnPaused()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourCallbacks;
            if (flag)
            {
                ((ISyncBehaviourCallbacks)this.SyncBehavior).OnGameUnPaused();
            }
        }

        // 游戏结束
        public void OnGameEnded()
        {
            bool flag = this.SyncBehavior is ISyncBehaviourCallbacks;
            if (flag)
            {
                ((ISyncBehaviourCallbacks)this.SyncBehavior).OnGameEnded();
            }
        }

        // 玩家断开连接
        public void OnPlayerDisconnection(int playerId)
        {
            bool flag = this.SyncBehavior is ISyncBehaviourCallbacks;
            if (flag)
            {
                ((ISyncBehaviourCallbacks)this.SyncBehavior).OnPlayerDisconnection(playerId);
            }
        }
        #endregion ISyncBehaviourCallbacks 接口方法
    }
}
