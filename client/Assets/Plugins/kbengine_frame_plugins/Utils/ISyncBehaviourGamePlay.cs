using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public interface ISyncBehaviourGamePlay : ISyncBehaviour
    {
        void OnPreSyncedUpdate();

        void OnSyncedInput();

        void OnSyncedUpdate();
    }
}
