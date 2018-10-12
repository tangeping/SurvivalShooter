using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SyncFrame
{
    public class SynPlayer
    {
        public PlayerInfo playerInfo; //

        public int ID
        {
            get
            {
                return this.playerInfo.Id;
            }
        }

        internal SynPlayer(int id, string name)
        {
            this.playerInfo = new PlayerInfo(id, name);

        }

    }
}
