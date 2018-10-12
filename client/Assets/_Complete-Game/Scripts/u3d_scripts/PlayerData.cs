using CBFrame.Utils;
using CompleteProject;
using KBEngine;
using SyncFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : CBSingleton<PlayerData>
{

    public PlayerHealth playerHealth = null;

    public Dictionary<UInt32, FRAME_DATA> frame_list = new Dictionary<UInt32, FRAME_DATA>();
}

public class GameData : CBSingleton<GameData>
{
    public Dictionary<int, GameObject> players = new Dictionary<int, GameObject>();

}