using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CutRope : MonoBehaviour
    {
        private int sufferedCount = 0;
        // Start is called before the first frame update

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "ShockWave(Clone)")
            {
                sufferedCount++;
                Destroy(collider.gameObject);
                if (sufferedCount == 2)
                {
                    Debug.Log("Rope is broken.");
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}

