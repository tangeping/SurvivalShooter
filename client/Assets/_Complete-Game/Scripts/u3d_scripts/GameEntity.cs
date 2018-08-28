
using UnityEngine;
using KBEngine;
using System.Collections;
using System;
using System.Xml;
using System.Collections.Generic;
using CBFrame.Sys;
using CBFrame.Core;
using CompleteProject;

[RequireComponent(typeof(CharacterController))]
public class GameEntity : MonoBehaviour
{


    public bool isPlayer = false;
    public bool isAvatar = false;

    private Vector3 _position = Vector3.zero;
    private Vector3 _eulerAngles = Vector3.zero;
    private Vector3 _scale = Vector3.zero;

    private Vector3 _destPosition = Vector3.zero;
    private Vector3 _destDirection = Vector3.zero;
    private Vector3 _moveDirection = Vector3.zero;

    public float smoothing = 5f;        // The speed with which the camera will be following.

    private float _speed = 0f;

    public string entity_name = "";

    public bool isOnGround = true;

    public bool isControlled = false;

    public bool entityEnabled = true;

    public UInt64 frameId = 0;

    private static GameObject directionObj = null;

    private CharacterController m_CharacterController;

    public static float DeltaTimeSample = 0.02f;

    Animator anim;                      // Reference to the animator component.

    public Vector3 destPosition
    {
        get
        {
            return _destPosition;
        }

        set
        {
            //_destPosition += speed * value * Time.deltaTime;
            _destPosition = value;
        }
    }

    public Vector3 destDirection
    {
        get
        {
            return _destDirection;
        }

        set
        {
            _destDirection += /*speed **/ value;
        }
    }
    public Vector3 moveDirection
    {
        get
        {
            return _moveDirection;
        }

        set
        {
            _moveDirection = value;

 //           System.TimeSpan time = System.DateTime.Now - GetComponent<PlayerMovement>().startTime;
 //           Debug.Log("TTL time:" + time);
            //m_CharacterController.Move(speed * moveDirection * DeltaTimeSample);
            //transform.position += speed * moveDirection * DeltaTimeSample;


        }
    }

    public Vector3 position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;

            if (gameObject != null)
                gameObject.transform.position = _position;
        }
    }

    public Vector3 eulerAngles
    {
        get
        {
            return _eulerAngles;
        }

        set
        {
            _eulerAngles = value;

            if (directionObj != null)
            {
                directionObj.transform.eulerAngles = _eulerAngles;
            }
        }
    }

    public Quaternion rotation
    {
        get
        {
            return Quaternion.Euler(_eulerAngles);
        }

        set
        {
            eulerAngles = value.eulerAngles;
        }
    }

    public Vector3 scale
    {
        get
        {
            return _scale;
        }

        set
        {
            _scale = value;

            if (gameObject != null)
                gameObject.transform.localScale = _scale;
        }
    }

    public float speed
    {
        get
        {
            _speed = 6.0f;
            return _speed;
        }

        set
        {
            //_speed = value;
        }
    }

    void Awake()
    {
    }

    void Start()
    {
 //       CBGlobalEventDispatcher.Instance.AddEventListener<Transform>((int)EVENT_ID.EVENT_FRAME_UPDATE, AttachTarget);

        m_CharacterController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

 //       StartCoroutine(onFixedUpdate());
    }

//     IEnumerator onFixedUpdate()
//     {
//         yield return new WaitForSecondsRealtime(DeltaTimeSample);
//         if (!isAvatar)
//             yield return null;
// 
//         m_CharacterController.Move(speed * moveDirection * DeltaTimeSample);
//     }

    void OnDestroy()
    {
    }

    void OnGUI()
    {
//         if (Camera.main == null || entity_name == "")
//             return;
// 
//         //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标     
//         Vector2 uiposition = Camera.main.WorldToScreenPoint(transform.position);
// 
//         //得到真实NPC头顶的2D坐标
//         uiposition = new Vector2(uiposition.x, Screen.height - uiposition.y);
// 
//         //计算NPC名称的宽高
//         Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(entity_name));
// 
// 
//         GUIStyle fontStyle = new GUIStyle();
//         fontStyle.normal.background = null;             //设置背景填充  
//         fontStyle.normal.textColor = Color.yellow;      //设置字体颜色  
//         fontStyle.fontSize = (int)(15.0 * gameObject.transform.localScale.x);
//         fontStyle.alignment = TextAnchor.MiddleCenter;

        GUI.Label(new Rect(Screen.width - 200, 1, 200, 200), "rec:" + frameId.ToString());
        //绘制NPC名称
        //       GUI.Label(new Rect(uiposition.x - (nameSize.x / 2), uiposition.y - nameSize.y*4, nameSize.x, nameSize.y), entity_name, fontStyle);
    }



    public void set_modelID(int modelID)
    {
        Debug.Log("GameEntity::set_modelID:" + modelID);
    }

    public void set_modelScale(float scale)
    {
        Debug.Log("GameEntity::set_modelScale:" + scale);
        //gameObject.transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void entityEnable()
    {
        entityEnabled = true;
    }

    public void entityDisable()
    {
        entityEnabled = false;
    }

    public void set_state(sbyte v)
    {
    }


    public void onUpdate()
    {

    }
    private void FixedUpdate()
    {
        destPosition += speed * moveDirection * DeltaTimeSample;

        anim.SetBool("IsWalking", moveDirection != Vector3.zero);
    }
    private void Update()
    {
        if (!isAvatar)
            return;



        //         if (!entityEnabled)
        //         {
        //             position = destPosition;
        //             return;
        //         }
        // 
        //         if (moveDirection != Vector3.zero)
        //         {
        //             Debug.Log("moveDirection:" + moveDirection);
        //             //            moveDirection = transform.TransformDirection(moveDirection);
        //             moveDirection *= speed;
        //         }

        //       moveDirection *= speed;
        //transform.position += moveDirection;
        //         m_CharacterController.Move(speed * moveDirection * Time.deltaTime);
        // 
        //         moveDirection = Vector3.zero;


        //         if (Vector3.Distance(eulerAngles, destDirection) > 0.0004f)
        //         {
        //             rotation = Quaternion.Slerp(rotation, Quaternion.Euler(destDirection), 8f * Time.deltaTime);
        // 
        //         }

        //         float dist = Vector3.Distance(destPosition, position);
        // 
        //         if (dist > 0.1f)
        //         {
        //             Vector3 movement = destPosition - position;
        //             movement.y = 0;
        // 
        // 
        //             if (dist > speed * Time.deltaTime || movement.magnitude > speed * Time.deltaTime)
        //             {
        //                 position += movement * speed * Time.deltaTime;
        // 
        //             }
        //             else
        //             {
        //                 position = destPosition;
        //             }
        //         }
        //         else
        //         {
        //             position = destPosition;
        //         }

        position = Vector3.Lerp(position, destPosition, speed * 10f * Time.deltaTime);
    }
}

