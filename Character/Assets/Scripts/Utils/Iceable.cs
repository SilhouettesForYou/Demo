using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Iceable : MonoBehaviour
    {
        public PlayerInfo playerInfo;
        private Transform leftTrigger;
        private Transform rightTrigger;
        private Transform boxGroundCheck;
        private Rigidbody2D body;

        private float playerRdius = 0.5f;

        private bool iceable = false;

        void Awake()
        {
            body = transform.GetComponent<Rigidbody2D>();
            leftTrigger = transform.Find("WaterSideLeft");
            rightTrigger = transform.Find("WaterSideRight");
        }

        void FixedUpdate()
        {
            
        }

        private void PerformSkill()
        {

        }
    }
}
