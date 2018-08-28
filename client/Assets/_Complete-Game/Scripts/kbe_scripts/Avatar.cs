namespace KBEngine
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Avatar : AvatarBase
    {
        public Avatar()
        {
        }

        public override void __init__()
        {
            if (isPlayer())
            {
                Event.registerIn("relive", this, "relive");
                Event.registerIn("reqPlayerPos", this, "reqPlayerPos");
                Event.registerIn("reqPlayerDir", this, "reqPlayerDir");

                // 触发登陆成功事件
                Event.fireOut("onLoginSuccessfully", new object[] { KBEngineApp.app.entity_uuid, id, this });
            }
        }

        public override void onDestroy()
        {
            if (isPlayer())
            {
                KBEngine.Event.deregisterIn(this);
            }
        }

        public override void onEnterWorld()
        {
            base.onEnterWorld();

            if (isPlayer())
            {
                Event.fireOut("onAvatarEnterWorld", new object[] { KBEngineApp.app.entity_uuid, id, this });
            }
        }

        public void relive(Byte type)
        {
            cellCall("relive", type);
        }

        public virtual void reqPlayerPos(UInt64 frameid,Vector3 pos)
        {
            position = pos;

            cellEntityCall.reqPosition(frameid, position);
 //           Debug.Log("Avatar::reqPlayerPos:" + position);
        }

        public virtual void reqPlayerDir(UInt64 frameid, Vector3 dir)
        {
            direction = dir;

            cellEntityCall.reqDirection(frameid, dir);
 //           Debug.Log("Avatar::reqPlayerDir:" + direction);
        }

        public override void onModelIDChanged(Byte old)
        {
            // Dbg.DEBUG_MSG(className + "::set_modelID: " + old + " => " + v); 
            Event.fireOut("set_modelID", new object[] { this, this.modelID });
        }

        public override void onNameChanged(string old)
        {
            // Dbg.DEBUG_MSG(className + "::set_name: " + old + " => " + v); 
            Event.fireOut("set_name", new object[] { this, this.name });
        }

        public override void onLevelChanged(Byte old)
        {
            // Dbg.DEBUG_MSG(className + "::set_level: " + old + " => " + v); 
            Event.fireOut("set_level", new object[] { this, this.level });
        }

        public override void onMoveSpeedChanged(float old)
        {
            // Dbg.DEBUG_MSG(className + "::set_moveSpeed: " + old + " => " + v); 
            Event.fireOut("set_moveSpeed", new object[] { this, this.moveSpeed });
        }

        public override void onModelScaleChanged(float old)
        {
            // Dbg.DEBUG_MSG(className + "::set_modelScale: " + old + " => " + v); 
            Event.fireOut("set_modelScale", new object[] { this, this.modelScale });
        }

        public override void updatePosition(UInt64 frameid, Vector3 direction)
        {
            Event.fireOut("updatePos", new object[] { this, frameid, direction });
        }

        public override void updateDirection(UInt64 frameid, Vector3 position)
        {
            Event.fireOut("updateDir", new object[] { this,frameid,position  });
        }

    }
}
