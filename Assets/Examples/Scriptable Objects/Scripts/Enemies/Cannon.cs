using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.ScriptableObjects
{
    public class Cannon : Enemy // Inheritance - Enemy is the base class Cannon is derrived 
    {
        public float range = 10f;
        public Transform spawnPoint;
        public GameObject projectilePrefab;

        // Fires a projectile in the direction of the target
        public override void Attack()
        {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation); ;

        }
    }
}
