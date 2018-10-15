using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Examples.ScriptableObjects
{
    public class Enemy : MonoBehaviour
    {

        public int damage = 10;
        public int health = 100;
        public float attackRate = 1f;

        public Transform target;
        public NavMeshAgent agent;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(AttackDelay());
        }

        // Update is called once per frame
        void Update()
        {
            // seek to target
            agent.SetDestination(target.position);
        }

        private IEnumerator AttackDelay()
        {
            Attack();
            yield return new WaitForSeconds(attackRate);
            StartCoroutine(AttackDelay());
        }

        // polymorphism - virtual / override
        public virtual void Attack()
        {

        }
    }
}