using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CutRope : MonoBehaviour
    {
        private bool isBroken = false;
        private int sufferedCount = 0;
        // Start is called before the first frame update
         

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(collider.transform.name);
            if (collider.transform.name == "ShockWave(Clone)")
            {
                Debug.Log("Rope is broken.");
                EventCenter.Braodcast(EventType.KnifeStop);
            }
        }
    }
}

