using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float speed = 5f;
    public Rigidbody rigid;





    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        rigid.AddForce(new Vector3(inputH, 0, inputV) * speed);


    }
}
