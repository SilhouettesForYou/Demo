using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckPoint : MonoBehaviour
    {
        private void Reset()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            An an = collision.GetComponent<An>();
            if (an != null)
            {
                an.SetCheckPoint(this);

                Pankapu.SetCheckPoint(this);
            }
        }
    }
}
