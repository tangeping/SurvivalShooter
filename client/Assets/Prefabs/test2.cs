using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {

    // Use this for initialization
    float smoothing = 5.0f;
    public float Speed = 10.0f;
    public float Displacement = 0.0f;
    public float PlayTime = 1 / 30.0f;

    public float CurrTime = 0f;
    bool begine = false;

    Vector3 cameraOffset = Vector3.zero;
    Vector3 movement = Vector3.zero;
    Vector3 destment = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 LastOper = Vector3.zero;

    Queue<Vector3> FramePool = new Queue<Vector3>();
    Queue<Vector3> OperPool = new Queue<Vector3>();


    void Start () {

        cameraOffset = Camera.main.transform.position - transform.position;

        StartCoroutine(onTimer());
	}
	
    void follow()
    {
        Vector3 targetCamPos = transform.position + cameraOffset;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetCamPos, smoothing * Time.deltaTime);

        //Camera.main.transform.position = cameraOffset + transform.position;
    }

    void TransformMove(Vector3 movement)
    {
        transform.Translate(movement * Speed * Time.deltaTime);
    }

    void towardsMove(Vector3 movement)
    {
        Vector3 dest = transform.position + movement * Speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, dest, Speed * Time.deltaTime);
    }

    void lerpMove(Vector3 movement)
    {
        Vector3 dest = transform.position + movement * Speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, dest, Speed * Time.deltaTime);
    }

    public IEnumerator onTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(PlayTime);
            if (!begine)
            {
                yield return null;
            }

            if (FramePool.Count > 0)
            {
                LastOper = FramePool.Dequeue();
            }

            OperPool.Enqueue(LastOper);
        }
    }

    void Operation()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 cache = new Vector3(h, 0.0f, v);
        if(movement  != cache)
        {
            begine = true;
            movement = cache;
            FramePool.Enqueue(movement);
        }

        
    }

    void Handler()
    {

//         float cacheSpeed = Displacement / PlayTime;
// 
        transform.position = Vector3.Lerp(transform.position, destination, Speed * Time.deltaTime);

        float dis = Vector3.Distance(transform.position, destination);
        if (dis < 0.0001)
        {
            Debug.Log("diff time --------> : " + (PlayTime - CurrTime));

        }


        CurrTime += Time.deltaTime;

        if(CurrTime > PlayTime)
        {
            if(OperPool.Count >0)
            {
                destment = OperPool.Dequeue();
                destination += destment * Speed * PlayTime;

                CurrTime = 0f;
            }
        }

    }

    void SimpleHandler()
    {

        CurrTime += Time.deltaTime;

        if (CurrTime > PlayTime)
        {
            if (FramePool.Count > 0)
            {
                destment = FramePool.Dequeue();

                CurrTime = 0f;
            }
        }

        transform.position += destment * Speed * Time.deltaTime;

    }
    void NormalMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 cache = new Vector3(h, 0.0f, v);

        //        lerpMove(cache);
        transform.position += Speed * cache * Time.deltaTime;
        Debug.Log("NormalMove:cache = "+cache);
    }

    // Update is called once per frame
    void Update () {

        follow();

        NormalMove();


        Operation();


        //Handler();

        //SimpleHandler();
    }

    
}
