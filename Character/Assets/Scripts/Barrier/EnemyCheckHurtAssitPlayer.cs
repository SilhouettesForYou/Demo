using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class EnemyCheckHurtAssitPlayer : MonoBehaviour
    {
        [HideInInspector]
        public bool enemyHurtPlayer = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
                enemyHurtPlayer = true;
        }
    }
}
