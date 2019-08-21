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
                if (sufferedCount == 1)
                {
                    Debug.Log("Rope is broken.");
                    EventCenter.Braodcast(EventType.KnifeStop);
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}

