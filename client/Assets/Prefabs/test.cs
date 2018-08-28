using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    // Use this for initialization
    private CharacterController m_CharacterController;
    private Rigidbody m_rigid;


    private void Awake()
    {
        Application.targetFrameRate = 120;
    }
    void Start ()
    {
        //       m_CharacterController = GetComponent<CharacterController>();
        m_rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame

    

	void FixedUpdate() {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(h, 0.0f, v);

        m_rigid.velocity += 4.0f * movement * Time.deltaTime;
 //        transform.position += 4.0f * movement *Time.deltaTime;


        //       transform.position += new Vector3(2f* h * Time.deltaTime, 0.0f, 2f * v * Time.deltaTime);

        //      Debug.Log("Time.deltaTime = " + Time.deltaTime.ToString("f4"));
        // m_CharacterController.Move( 4.0f * movement * Time.deltaTime);


    }
}
