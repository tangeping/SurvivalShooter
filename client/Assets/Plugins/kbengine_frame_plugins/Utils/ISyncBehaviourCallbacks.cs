using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public interface ISyncBehaviourCallbacks : ISyncBehaviour
    {
        void OnSyncedStart();

        void OnSyncedStartLocalPlayer();

        void OnGamePaused();

        void OnGameUnPaused();

        void OnGameEnded();

        void OnPlayerDisconnection(int playerId);
    }
}

