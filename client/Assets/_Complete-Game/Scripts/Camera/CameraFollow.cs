using UnityEngine;
using System.Collections;
using CBFrame.Core;
using CBFrame.Sys;

namespace CompleteProject
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target=null;            // The position that that camera will be following.
        public float smoothing = 15f;        // The speed with which the camera will be following.

        //offset:(1.0,15.0,-22.0)
        Vector3 offset = new Vector3(1.0f,15.0f,-22.0f);                     // The initial offset from the target.


        void Start ()
        {
            CBGlobalEventDispatcher.Instance.AddEventListener<Transform>((int)EVENT_ID.EVENT_CAMERA_FOLLOW, AttachTarget);


            // Calculate the initial offset.
            //offset = transform.position - target.position;
        }

        public void AttachTarget(Transform t)
        {
            target = t;
            transform.position = target.position + offset;
            //offset = transform.position - target.position;
        }

        void RemoveTarget()
        {
            target = null;
        }

        void FixedUpdate ()
        {
            if (target != null)
            { // Create a postion the camera is aiming for based on the offset from the target.
                Vector3 targetCamPos = target.position + offset;

                // Smoothly interpolate between the camera's current position and it's target position.
                //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
                transform.position = targetCamPos;
            }
        }
    }
}