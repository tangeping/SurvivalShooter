using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    public enum CMD
    {
        MOUSE = 1,
        KEYBOARD=2,
        USER = 3,
        MAX =4,
    }

    public abstract class FrameBase
    {
        public ENTITY_DATA e = new ENTITY_DATA();
        public MemoryStream s = MemoryStream.createObject();

        public abstract ENTITY_DATA Serialize();

        public abstract void PareseFrom(ENTITY_DATA e);
    }

    public class FrameMouse : FrameBase
    {
        public Vector3 point = Vector3.zero;
        public FrameMouse(CMD cmd, Vector3 p)
        {
            e.cmd_type = (Byte)cmd;
            point = p;
        }

        public FrameMouse()
        {
        }

        public override ENTITY_DATA Serialize()
        {
            s.writeVector3(point);
            e.datas = new byte[s.wpos];
            Array.Copy(s.data(),e.datas, s.wpos);
            return e;
        }

        public override void PareseFrom(ENTITY_DATA e)
        {
            this.e = e;
            s.setBuffer(e.datas);
            point = s.readVector3();
        }

    }
   
    public class FrameKeyboard : FrameBase
    {
        public List<KeyCode> keys = new List<KeyCode>();

        public FrameKeyboard(CMD cmd, List<KeyCode> keys )
        {
            e.cmd_type = (Byte)cmd;
            this.keys = keys;
        }

        public FrameKeyboard()
        {
        }

        public override ENTITY_DATA Serialize()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                s.writeUint16((UInt16)keys[i]);
            }
            e.datas = new byte[s.wpos];
            Array.Copy(s.data(), e.datas, s.wpos);
            return e;
        }

        public override void PareseFrom(ENTITY_DATA e)
        {
            this.e = e;
            s.setBuffer(e.datas);

            KeyCode k = KeyCode.None;

            while ((k = (KeyCode)s.readUint8()) != 0)
            {
                keys.Add(k);
            }
        }
    }

    public class FrameUser:FrameBase
    {
        public Vector3 movement = Vector3.zero;
        public double d_point = 0.0;

        public FrameUser(CMD cmd, Vector3 p,double d)
        {
            e.cmd_type = (Byte)cmd;
            movement = p;
            d_point = d;
        }

        public FrameUser()
        {
        }

        public override ENTITY_DATA Serialize()
        {
            s.writeVector3(movement);
            s.writeDouble(d_point);
            e.datas = new byte[s.wpos];
            Array.Copy(s.data(), e.datas, s.wpos);
            return e;
        }

        public override void PareseFrom(ENTITY_DATA e)
        {
            this.e = e;
            s.setBuffer(e.datas);
            movement = s.readVector3();
            d_point = s.readDouble();
        }
    }


    public class FrameProto
    {
        static public ENTITY_DATA encode(FrameBase sendMsg)
        {
            return sendMsg.Serialize();
        }
        
        static public FrameBase decode(ENTITY_DATA readMsg)
        {
            FrameBase f;

            switch ((CMD)readMsg.cmd_type)
            {
                case CMD.MOUSE:
                    {
                        f = new FrameMouse();
                    }
                    break;
                case CMD.KEYBOARD:
                    {
                        f = new FrameKeyboard();
                    }
                    break;
                case CMD.USER:
                    {
                        f = new FrameUser();
                    }
                    break;
                default:
                    {
                        f = null;
                    }
                    return f;
            }

            f.PareseFrom(readMsg);

            return f;
        }

    }
}

