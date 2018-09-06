using CBFrame.Core;
using CBFrame.Sys;
using Frame;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour {

    public bool isPlayer = false;
    public bool isAvatar = false;
    public bool entityEnabled = true;
    public  KBEngine.Entity entity;

    //----------------------------
    public static float playTime = 1 / 30.0f; // 33 s
    private float FrameDuration = 0f;
    private Vector3 destPosition = Vector3.zero;
    private float destDuration = 0.0f;
    private int thresholdFrame = 4;


    private float Speed = 10.0f;
    //----------------------------
    public class FrameData
    {
        public float duration;
        public List<ENTITY_DATA> operation = new List<ENTITY_DATA>();
    }

    public Queue<KeyValuePair<UInt32, FrameData>> framePool = new Queue<KeyValuePair<UInt32, FrameData>>();

    public FrameData lastFrameDate = null;
    private UInt32 curreFrameId = 0;

    public float DestDuration
    {
        get
        {
            return destDuration;
        }

        set
        {
            destDuration = value;
        }
    }

    //----------------------------
    public void entityEnable()
    {
        entityEnabled = true;
    }

    public void entityDisable()
    {
        entityEnabled = false;
    }

    // Use this for initialization
    void Start ()
    {
        CBGlobalEventDispatcher.Instance.AddEventListener<FRAME_DATA>((int)EVENT_ID.EVENT_FRAME_TICK, onUpdateTick);
    }

    public void onUpdateTick(FRAME_DATA frameMsg)
    {
        curreFrameId = frameMsg.frameid;

        bool isEmptyFrame = true;

        for (int i = 0; i < frameMsg.operation.Count; i++)
        {
            var oper = frameMsg.operation[i];

            if (oper.entityid != entity.id)
            {
                continue;
            }

            isEmptyFrame = false;
            var data = new FrameData();
            data.duration = playTime;
            data.operation.Add(oper);

            framePool.Enqueue(new KeyValuePair<UInt32, FrameData>(curreFrameId, data));
            lastFrameDate = data;
        }

        if (isEmptyFrame && lastFrameDate != null)
        {
            framePool.Enqueue(new KeyValuePair<UInt32, FrameData>(curreFrameId, lastFrameDate));
        }

    }

    // Update is called once per frame
    void Update () {

        if (!isAvatar)
            return;

        float dis = Vector3.Distance(transform.position, destPosition);
        float currSpeed = DestDuration <=0 ? Speed : (Speed * playTime / DestDuration);

        Debug.Log("CurrSpeed:"+currSpeed + ",destDuration:"+ DestDuration);

        if(dis <= currSpeed * Time.deltaTime)
        {
            transform.position = destPosition;
//            Debug.LogError("----------diss time------------------:" + (playTime - FrameDuration));
        }
        else
        {
            Vector3 tempDirection = destPosition - transform.position;

            transform.position += tempDirection.normalized * currSpeed * Time.deltaTime;
        }
        


        FrameDuration += Time.deltaTime;

        if (FrameDuration >= DestDuration)
        {
            transform.position = destPosition;

//            float dis2 = Vector3.Distance(transform.position, destPosition);


 //            Debug.Log("diff distance---------->:" + dis2.ToString("f6"));

            if (framePool.Count > 0)
            {
                Vector3 movement = Vector3.zero;

                DestDuration = playTime / (  framePool.Count <= thresholdFrame ? 1: framePool.Count/ thresholdFrame);

                //DestDuration = playTime -  framePool.Count / thresholdFrame * 0.01f;//10ms快播

                var framedata = framePool.Dequeue();

                foreach (var item in framedata.Value.operation)
                {
                    if (item.cmd_type != (UInt32)CMD.USER)
                    {
                        continue;
                    }

                    FrameUser msg = FrameProto.decode(item) as FrameUser;
                    movement = msg.movement;
                }

                destPosition += Speed * movement * playTime;

                FrameDuration = 0.0f;
            }
        }
    }
}
