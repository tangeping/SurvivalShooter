using KBEngine;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CompleteProject;
using CBFrame.Core;
using CBFrame.Sys;

public class World : MonoBehaviour
{
    private UnityEngine.GameObject terrain = null;
    public UnityEngine.GameObject terrainPerfab;


    public UnityEngine.GameObject foodsPerfab;
    public UnityEngine.GameObject smashsPerfab;
    private UnityEngine.GameObject player = null;
    public UnityEngine.GameObject avatarPerfab;


    public Sprite[] avatarSprites = new Sprite[2];
    public Sprite[] foodsSprites = new Sprite[3];
    public Sprite[] smashsSprites = new Sprite[1];

    public static float GAME_MAP_SIZE = 0.0f;
    public static int ROOM_MAX_PLAYER = 0;
    public static int GAME_ROUND_TIME = 0;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        installEvents();
    }

    void installEvents()
    {
        // in world
        KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
        //         KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        //         KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");

        KBEngine.Event.registerOut("onEnterRoom", this, "onEnterRoom");
        KBEngine.Event.registerOut("onEnterRoom", this, "onEnterRoom");
        KBEngine.Event.registerOut("onRecieveFrame", this, "onRecieveFrame");

        KBEngine.Event.registerOut("set_position", this, "set_position");
        KBEngine.Event.registerOut("set_direction", this, "set_direction");
//        KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
        KBEngine.Event.registerOut("onControlled", this, "onControlled");
        KBEngine.Event.registerOut("onSetSpaceData", this, "onSetSpaceData");
        KBEngine.Event.registerOut("onDelSpaceData", this, "onDelSpaceData");

        // in world(register by scripts)
//        KBEngine.Event.registerOut("onAvatarEnterWorld", this, "onAvatarEnterWorld");
        KBEngine.Event.registerOut("set_name", this, "set_entityName");
        KBEngine.Event.registerOut("set_state", this, "set_state");
        KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
        KBEngine.Event.registerOut("set_modelScale", this, "set_modelScale");
        KBEngine.Event.registerOut("set_modelID", this, "set_modelID");

//         KBEngine.Event.registerOut("updatePos", this, "updatePos");
//         KBEngine.Event.registerOut("updateDir", this, "updateDir");
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void addSpaceGeometryMapping(string respath)
    {
        Debug.Log("loading scene(" + respath + ")...");
        UI.inst.info("");

        if (terrain == null && terrainPerfab != null)
            terrain = Instantiate(terrainPerfab) as UnityEngine.GameObject;

//         if (player)
//             player.GetComponent<GameEntity>().entityEnable();

    }

    public void onAvatarEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Avatar avatar)
    {
        if (!avatar.isPlayer())
        {
            return;
        }

        UI.inst.info("loading scene...(加载场景中...)");
        Debug.Log("loading scene...");

 //       TriggerEvent((int)EVENT_ID.EVENT_CREAT_PLAYER);
    }

    public void createPlayer()
    {
        Debug.Log("Room::createPlayer");
        if (player != null)
        {
            player.GetComponent<GameEntity>().entityEnable();
            return;
        }

        if (KBEngineApp.app.entity_type != "Avatar")
        {
            return;
        }

        KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
        if (avatar == null)
        {
            Debug.Log("wait create(palyer)!");
            return;
        }

        player = Instantiate(avatarPerfab, avatar.position, Quaternion.Euler(avatar.direction)) as UnityEngine.GameObject;

        player.AddComponent<PlayerMovement>();

        avatar.renderObj = player;

        ((GameObject)avatar.renderObj).name = avatar.className + "_" + avatar.id;

        GameEntity entity = player.GetComponent<GameEntity>();
        entity.entityDisable();
        entity.isAvatar = true;
        entity.isPlayer = true;
        entity.entity = avatar;
 
        // 有必要设置一下，由于该接口由Update异步调用，有可能set_position等初始化信息已经先触发了
        // 那么如果不设置renderObj的位置和方向将为0

        set_position(avatar);
        set_direction(avatar);

        //        Camera.main.GetComponent<CameraFollow>().AttachTarget(player.transform);

        CBGlobalEventDispatcher.Instance.TriggerEvent((int)EVENT_ID.EVENT_CAMERA_FOLLOW, player.transform);
        CBGlobalEventDispatcher.Instance.TriggerEvent((int)EVENT_ID.EVENT_PLAYER_HEALTH, player.GetComponent<PlayerHealth>());


    }

//     public void onEnterWorld(KBEngine.Entity entity)
//     {
//         if (entity.isPlayer())
//         {
//             createPlayer();
//         }
//         else
//         {
//             UnityEngine.GameObject entityPerfab = null;
// 
//             entityPerfab = avatarPerfab;
// 
//             entity.renderObj = Instantiate(entityPerfab, entity.position, Quaternion.Euler(entity.direction))
//                 as UnityEngine.GameObject;
// 
//             ((UnityEngine.GameObject)entity.renderObj).name = entity.className + "_" + entity.id;
// 
//             if (entity.className == "Avatar")
//             {
//                 ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().isAvatar = true;
//                 set_position(entity);
//                 set_direction(entity);
//             }
//         }
//     }
// 
//     public void onLeaveWorld(KBEngine.Entity entity)
//     {
//         if (entity.renderObj == null)
//             return;
// 
//         UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
//         entity.renderObj = null;
//     }

    public void onEnterRoom(KBEngine.Entity me, KBEngine.Entity other)
    {
        if (other.isPlayer())
        {
            createPlayer();
            return;
        }

        UnityEngine.GameObject entityPerfab = avatarPerfab;

        other.renderObj = Instantiate(entityPerfab, other.position, Quaternion.Euler(other.direction))
            as UnityEngine.GameObject;

        ((UnityEngine.GameObject)other.renderObj).name = other.className + "_" + other.id;

        if (other.className == "Avatar")
        {
            ((UnityEngine.GameObject)other.renderObj).GetComponent<GameEntity>().isAvatar = true;
            ((UnityEngine.GameObject)other.renderObj).GetComponent<GameEntity>().entity = other;
            set_position(other);
            set_direction(other);
        }

    }
    public void onLeaveRoom(KBEngine.Entity me, KBEngine.Entity other)
    {
        if (other.renderObj == null)
            return;

        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)other.renderObj);
        other.renderObj = null;
    }

    public void onRecieveFrame(KBEngine.Entity entity,FRAME_DATA frameMsg)
    {
        if (entity.renderObj == null)
            return;

        //        Debug.Log("----------onRecieveFrame tick : " + DateTime.Now.TimeOfDay.ToString());

         CBGlobalEventDispatcher.Instance.TriggerEvent((int)EVENT_ID.EVENT_FRAME_TICK, frameMsg);

//         for (int i = 0; i < frameMsg.operation.Count; i++)
//         {
//             ENTITY_DATA entityData = frameMsg.operation[i];
// 
//             CBGlobalEventDispatcher.Instance.TriggerEvent((int)EVENT_ID.EVENT_FRAME_CMD,frameMsg.frameid, entityData);
//   
//         }
    }


    public void onSetSpaceData(UInt32 spaceID, string key, string value)
    {
        if ("GAME_MAP_SIZE" == key)
            GAME_MAP_SIZE = float.Parse(value);
        else if("ROOM_MAX_PLAYER" == key)
            ROOM_MAX_PLAYER = int.Parse(value);
        else if("GAME_MAP_SIZE" == key)
            GAME_ROUND_TIME = int.Parse(value);
    }

    public void onDelSpaceData(UInt32 spaceID, string key)
    {

    }

    public void set_position(KBEngine.Entity entity)
    {
        if (gameObject == null || entity.renderObj == null)
            return;
        Debug.Log("World::set_position." + entity.id + ",position:" + entity.position);

        GameObject go = ((UnityEngine.GameObject)entity.renderObj);
    }

    public void onControlled(KBEngine.Entity entity, bool isControlled)
    {
        if (entity.renderObj == null)
            return;
    }

    public void set_direction(KBEngine.Entity entity)
    {

    }

    public void set_entityName(KBEngine.Entity entity, object v)
    {

    }

    public void set_state(KBEngine.Entity entity, object v)
    {
    }

    public void set_moveSpeed(KBEngine.Entity entity, object v)
    {
        float fspeed = (float)v;

        Debug.Log("player->fspeed: " + v);

    }

    public void set_modelScale(KBEngine.Entity entity, object v)
    {
        float modelScale = ((float)v);
    }

    public void set_modelID(KBEngine.Entity entity, object v)
    {
        Byte modelID = ((Byte)v);

    }
}
