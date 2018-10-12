using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public class SyncFrameManager : MonoBehaviour
    {

        private FP DurationTime = 0;

        public const float playTime = 1 / 30.0f; // 33 s
        /**
        * @brief Instance of the lockstep engine.
        **/
        public GameObject[] PlayerObjects;

        private AbstractLockstep lockstep;

        private FP lockedTimeStep = playTime;
        /**
         * @brief A dictionary holding a list of {@link SyncBehaviour} belonging to each player.
         **/
        private Dictionary<int, List<SyncManagedBehaviour>> behaviorsByPlayer;

        /**
        * @brief A list of {@link TrueSyncBehaviour} not linked to any player.
        **/
        private List<SyncManagedBehaviour> generalBehaviours = new List<SyncManagedBehaviour>();

        private Dictionary<ISyncBehaviour,SyncManagedBehaviour> mapManagedBehaviors = new Dictionary<ISyncBehaviour, SyncManagedBehaviour>();


        private static SyncFrameManager instance;

        private FP time = 0;

        /** 
        * @brief Returns the local player.
        **/
        public static PlayerInfo LocalPlayer
        {
            get
            {
                if (instance == null || instance.lockstep == null)
                {
                    return null;
                }

                return instance.lockstep.LocalPlayer.playerInfo;
            }
        }

        /** 
         * @brief Returns the active {@link TrueSyncConfig} used by the {@link TrueSyncManager}.
         **/
        public static TrueSyncConfig Config
        {
            get
            {
                if (instance == null)
                {
                    return null;
                }
                Debug.LogError("no ActiveConfig");
                return /*instance.ActiveConfig*/ new TrueSyncConfig();
            }
        }



        private SyncManagedBehaviour NewManagedBehaviour(ISyncBehaviour b)
        {
            SyncManagedBehaviour behaviour = new SyncManagedBehaviour(b);

            mapManagedBehaviors[b] = behaviour;

            return behaviour;
        }

        private void InitBehaviours(bool realOwnerId)
        {
            foreach( var player in lockstep.players.Values)
            {
                List< SyncManagedBehaviour> behaviorsInstatiated = new List<SyncManagedBehaviour>();

                //:ToDo 
                for (int i = 0; i < PlayerObjects.Length; i++)
                {
                    GameObject prefab = PlayerObjects[i];

                    GameObject prefabInst = GameObject.Instantiate(prefab);

                    InitializeGameObject(prefabInst, prefabInst.transform.position.ToTSVector(), prefabInst.transform.rotation.ToTSQuaternion());

                    SyncBehaviour[] behaviours = prefabInst.GetComponentsInChildren<SyncBehaviour>();

                    for (int j = 0; j < behaviours.Length; j++)
                    {
                        SyncBehaviour behaviour = behaviours[j];

                        behaviour.owner = player.playerInfo;
                        behaviour.localOwner = lockstep.LocalPlayer.playerInfo;
                        behaviour.numberOfPlayers = lockstep.players.Count;
                        if(realOwnerId)
                        {
                            behaviour.ownerIndex = behaviour.owner.Id;
                        }

                        SyncManagedBehaviour managedBehaviour = NewManagedBehaviour(behaviour);
                        managedBehaviour.owner = behaviour.owner;
                        managedBehaviour.localOwner = behaviour.localOwner;

                        behaviorsInstatiated.Add(managedBehaviour);
                    }
                }

                behaviorsByPlayer.Add(player.ID, behaviorsInstatiated);
            }
        }

        private void CheckQueuedBehaviours()
        {
//             if (queuedBehaviours.Count > 0)
//             {
//                 generalBehaviours.AddRange(queuedBehaviours);
//                 initGeneralBehaviors(queuedBehaviours, true);
// 
//                 for (int index = 0, length = queuedBehaviours.Count; index < length; index++)
//                 {
//                     SyncManagedBehaviour tsmb = queuedBehaviours[index];
// 
//                     tsmb.SetGameInfo(lockstep.LocalPlayer.playerInfo, lockstep.players.Count);
//                     tsmb.OnSyncedStart();
//                 }
// 
//                 queuedBehaviours.Clear();
//             }
        }

        void Start()
        {
            InitBehaviours(false);

            SyncBehaviour[] behaviours = FindObjectsOfType<SyncBehaviour>();
            for(int i = 0; i < behaviours.Length;i++)
            {
                if((behaviours[i] is SyncBehaviour) && behaviorsByPlayer.ContainsKey(  behaviours[i].ownerIndex ))
                {
                    continue;
                }
                generalBehaviours.Add(NewManagedBehaviour(behaviours[i]));
            }           
        }


        private void FixedUpdate()
        {
            if(lockstep != null)
            {
                DurationTime += UnityEngine.Time.deltaTime;

                if(DurationTime >= playTime)
                {
                    DurationTime = 0;

                    lockstep.Update();
                }
            }
        }

        void GetLocalData(InputDataBase playerInputData)
        {
            SyncInput.CurrentInputData = (InputData)playerInputData;

            if (behaviorsByPlayer.ContainsKey(playerInputData.ownerID))
            {
                List<SyncManagedBehaviour> managedBehavioursByPlayer = behaviorsByPlayer[playerInputData.ownerID];
                for (int i = 0; i < managedBehavioursByPlayer.Count; i++)
                {
                    SyncManagedBehaviour bh = managedBehavioursByPlayer[i];

                    if (bh != null && !bh.disabled)
                    {
                        bh.OnSyncedInput();
                    }
                }
            }

            SyncInput.CurrentInputData = null;
        }

        void OnStepUpdate(List<InputDataBase> allInputData)
        {
            time += lockedTimeStep;

//             if (ReplayRecord.replayMode != ReplayMode.LOAD_REPLAY)
//             {
//                 CheckGameObjectsSafeMap();
//             }

            SyncInput.SetAllInputs(null);

            for (int index = 0; index < generalBehaviours.Count;  index++)
            {
                SyncManagedBehaviour bh = generalBehaviours[index];

                if (bh != null && !bh.disabled)
                {
                    bh.OnPreSyncedUpdate();
                    //instance.scheduler.UpdateAllCoroutines();
                }
            }

            for (int index = 0; index < allInputData.Count;  index++)
            {
                InputDataBase playerInputData = allInputData[index];

                if (behaviorsByPlayer.ContainsKey(playerInputData.ownerID))
                {
                    List<SyncManagedBehaviour> managedBehavioursByPlayer = behaviorsByPlayer[playerInputData.ownerID];
                    for (int index2 = 0, length2 = managedBehavioursByPlayer.Count; index2 < length2; index2++)
                    {
                        SyncManagedBehaviour bh = managedBehavioursByPlayer[index2];

                        if (bh != null && !bh.disabled)
                        {
                            bh.OnPreSyncedUpdate();
                            //instance.scheduler.UpdateAllCoroutines();
                        }
                    }
                }
            }

            SyncInput.SetAllInputs(allInputData);

            SyncInput.CurrentSimulationData = null;
            for (int index = 0; index < generalBehaviours.Count; index++)
            {
                SyncManagedBehaviour bh = generalBehaviours[index];

                if (bh != null && !bh.disabled)
                {
                    bh.OnSyncedUpdate();
                    //instance.scheduler.UpdateAllCoroutines();
                }
            }

            for (int index = 0, length = allInputData.Count; index < length; index++)
            {
                InputDataBase playerInputData = allInputData[index];

                if (behaviorsByPlayer.ContainsKey(playerInputData.ownerID))
                {
                    SyncInput.CurrentSimulationData = (InputData)playerInputData;

                    List<SyncManagedBehaviour> managedBehavioursByPlayer = behaviorsByPlayer[playerInputData.ownerID];
                    for (int index2 = 0, length2 = managedBehavioursByPlayer.Count; index2 < length2; index2++)
                    {
                        SyncManagedBehaviour bh = managedBehavioursByPlayer[index2];

                        if (bh != null && !bh.disabled)
                        {
                            bh.OnSyncedUpdate();
                            //instance.scheduler.UpdateAllCoroutines();
                        }
                    }
                }

                SyncInput.CurrentSimulationData = null;
            }

            CheckQueuedBehaviours();
        }

        private static void InitializeGameObject(GameObject go, TSVector position, TSQuaternion rotation)
        {
            ICollider[] tsColliders = go.GetComponentsInChildren<ICollider>();
            if (tsColliders != null)
            {
                for (int index = 0, length = tsColliders.Length; index < length; index++)
                {
                    PhysicsManager.instance.AddBody(tsColliders[index]);
                }
            }

            TSTransform rootTSTransform = go.GetComponent<TSTransform>();
            if (rootTSTransform != null)
            {
                rootTSTransform.Initialize();

                rootTSTransform.position = position;
                rootTSTransform.rotation = rotation;
            }

            TSTransform[] tsTransforms = go.GetComponentsInChildren<TSTransform>();
            if (tsTransforms != null)
            {
                for (int index = 0, length = tsTransforms.Length; index < length; index++)
                {
                    TSTransform tsTransform = tsTransforms[index];

                    if (tsTransform != rootTSTransform)
                    {
                        tsTransform.Initialize();
                    }
                }
            }

            TSTransform2D rootTSTransform2D = go.GetComponent<TSTransform2D>();
            if (rootTSTransform2D != null)
            {
                rootTSTransform2D.Initialize();

                rootTSTransform2D.position = new TSVector2(position.x, position.y);
                rootTSTransform2D.rotation = rotation.ToQuaternion().eulerAngles.z;
            }

            TSTransform2D[] tsTransforms2D = go.GetComponentsInChildren<TSTransform2D>();
            if (tsTransforms2D != null)
            {
                for (int index = 0, length = tsTransforms2D.Length; index < length; index++)
                {
                    TSTransform2D tsTransform2D = tsTransforms2D[index];

                    if (tsTransform2D != rootTSTransform2D)
                    {
                        tsTransform2D.Initialize();
                    }
                }
            }
        }

        /**
         * @brief Run/Unpause the game simulation.
         **/
        public static void RunSimulation()
        {
            if (instance != null && instance.lockstep != null)
            {
                //instance.lockstep.RunSimulation(false);
            }
        }

        /**
         * @brief Pauses the game simulation.
         **/
        public static void PauseSimulation()
        {
            if (instance != null && instance.lockstep != null)
            {
                //instance.lockstep.PauseSimulation();
            }
        }

        /**
         * @brief End the game simulation.
         **/
        public static void EndSimulation()
        {
            if (instance != null && instance.lockstep != null)
            {
                //instance.lockstep.EndSimulation();
            }
        }

        /**
         * @brief Update all coroutines created.
         **/
        public static void UpdateCoroutines()
        {
            if (instance != null && instance.lockstep != null)
            {
               // instance.scheduler.UpdateAllCoroutines();
            }
        }

        /**
         * @brief Starts a new coroutine.
         * 
         * @param coroutine An IEnumerator that represents the coroutine.
         **/
        public static void SyncedStartCoroutine(IEnumerator coroutine)
        {
            if (instance != null && instance.lockstep != null)
            {
                //instance.scheduler.StartCoroutine(coroutine);
            }
        }

        /**
         * @brief Instantiate a new prefab in a deterministic way.
         * 
         * @param prefab GameObject's prefab to instantiate.
         **/

        /** 
         * @brief Clean up references to be collected by gc.
         **/
        public static void CleanUp()
        {
            ResourcePool.CleanUpAll();
            StateTracker.CleanUp();
            instance = null;
        }

        void OnPlayerDisconnection(byte playerId)
        {
            SyncManagedBehaviour.OnPlayerDisconnection(generalBehaviours, behaviorsByPlayer, playerId);
        }

        void OnGameStarted()
        {
            SyncManagedBehaviour.OnGameStarted(generalBehaviours, behaviorsByPlayer);
            //instance.scheduler.UpdateAllCoroutines();

            CheckQueuedBehaviours();
        }

        void OnGamePaused()
        {
            SyncManagedBehaviour.OnGamePaused(generalBehaviours, behaviorsByPlayer);
            //instance.scheduler.UpdateAllCoroutines();
        }

        void OnGameUnPaused()
        {
            SyncManagedBehaviour.OnGameUnPaused(generalBehaviours, behaviorsByPlayer);
            //instance.scheduler.UpdateAllCoroutines();
        }

        void OnGameEnded()
        {
            SyncManagedBehaviour.OnGameEnded(generalBehaviours, behaviorsByPlayer);
            //instance.scheduler.UpdateAllCoroutines();
        }

        void OnApplicationQuit()
        {
            EndSimulation();
        }
    }
}
