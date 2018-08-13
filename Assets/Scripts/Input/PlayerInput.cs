using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public PlayerController controller;

    



    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizonatl");
        float inputV = Input.GetAxis("Vertical");
        controller.Move(inputH, inputV);

    }
}
