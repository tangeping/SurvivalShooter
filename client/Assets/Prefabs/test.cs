using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    // Use this for initialization
    private CharacterController m_CharacterController;
    private Rigidbody m_rigid;
    public float Speed = 20.0f;


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

    
    void TransformMove(Vector3 movement)
    {
        transform.Translate(movement * Speed * Time.deltaTime);
    }

    void rigdbodyMove(Vector3 movement)
    {
        m_rigid.MovePosition(transform.position + movement * Speed * Time.deltaTime);
    }

    void rigdSpeedMove(Vector3 movement)
    {
        m_rigid.velocity = movement * Speed;
    }

    void towardsMove(Vector3 movement)
    {
        Vector3 dest = transform.position + movement * Speed* Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, dest, Speed * Time.deltaTime);
    }

    void lerpMove(Vector3 movement)
    {
        Vector3 dest = transform.position + movement * Speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, dest, Speed * Time.deltaTime);
    }

	void FixedUpdate() {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(h, 0.0f, v);

        rigdSpeedMove(movement.normalized);

    }
}
