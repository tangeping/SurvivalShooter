using UnityEngine;
using System.Collections;
using KBEngine;
using System;

public class ShowFPS : MonoBehaviour {

    public float f_UpdateInterval = 0.5F;

    private float f_LastInterval;

    private int i_Frames = 0;

    private float f_Fps;

    private int i_TimeDelay = 0;

    private float f_RTT;

    public System.DateTime startTime;

    void Start()
    {
		//Application.targetFrameRate=60;

        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;

        KBEngine.Event.registerOut("onNetworkDelay", this, "onNetworkDelay");

        StartCoroutine(onSendTime());
    }



    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 40, 1, 200, 200), f_Fps.ToString("f2"));
        GUI.Label(new Rect(Screen.width - 100, 1, 200, 200), f_RTT.ToString("f2"));
    }

    public void onNetworkDelay(KBEngine.Entity entity,int arg)
    {
        if(arg == i_TimeDelay)
        {
            f_RTT = (System.DateTime.Now - startTime).Milliseconds / 1.0f;
        }
    }

    public IEnumerator onSendTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);

            ++i_TimeDelay;
            startTime = System.DateTime.Now;
            ((KBEngine.Avatar)KBEngineApp.getSingleton().player()).reqNetworkDelay(i_TimeDelay);

        }
    }


    void Update()
    {
        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }
    }
}