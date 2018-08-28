using CBFrame.Core;
using CompleteProject;
using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{


    bool Fire = true;

    // Use this for initialization
    void Start () {

        //offset:(1.0,15.0,-22.0)
        Vector3 offset = Camera.main.transform.position - transform.position;


        Debug.Log("offset : " + offset);

    }

    void Update()
    {

    }

}
