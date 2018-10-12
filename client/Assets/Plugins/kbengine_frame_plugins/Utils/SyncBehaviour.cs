using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{

    public class SyncBehaviour : MonoBehaviour, ISyncBehaviourGamePlay, ISyncBehaviourCallbacks
    {

        /**
         * @brief It is not called for instances of {@link TrueSyncBehaviour}.
         **/
        //public void SetGameInfo(TSPlayerInfo localOwner, int numberOfPlayers) { }

        public int numberOfPlayers;

        /**
         *  @brief Index of the owner at initial players list.
         */
        public int ownerIndex = -1;

        /**
         * @brief Called once when the object becomes active.
         **/
        public PlayerInfo owner;

        /**
         *  @brief Basic info about the local player.
         */
        public PlayerInfo localOwner;

        public virtual void OnSyncedStart() { }

        /**
         * @brief Called once on instances owned by the local player after the object becomes active.
         **/
        public virtual void OnSyncedStartLocalPlayer() { }

        /**
         * @brief Called when the game has paused.
         **/
        public virtual void OnGamePaused() { }

        /**
         * @brief Called when the game has unpaused.
         **/
        public virtual void OnGameUnPaused() { }

        /**
         * @brief Called when the game has ended.
         **/
        public virtual void OnGameEnded() { }

        /**
         *  @brief Called before {@link #OnSyncedUpdate}.
         *  
         *  Called once every lockstepped frame.
         */
        public virtual void OnPreSyncedUpdate() { }

        /**
         *  @brief Game updates goes here.
         *  
         *  Called once every lockstepped frame.
         */
        public virtual void OnSyncedUpdate() { }

        /**
         *  @brief Get local player data.
         *  
         *  Called once every lockstepped frame.
         */
        public virtual void OnSyncedInput() { }

        /**
         * @brief Callback called when a player get disconnected.
         **/
        public virtual void OnPlayerDisconnection(int playerId) { }
    }
}
