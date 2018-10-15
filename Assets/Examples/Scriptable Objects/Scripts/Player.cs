using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.ScriptableObjects
{
    public class Player : MonoBehaviour
    {

        public float movementSpeed = 20f;
        public CharacterController controller;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float inputH = Input.GetAxis("Horizontal");
            float inputV = Input.GetAxis("Vertical");
            Vector3 force = new Vector3(inputH, 0, inputV);
            force *= movementSpeed * Time.deltaTime;
            controller.Move(force);
        }
    }
}