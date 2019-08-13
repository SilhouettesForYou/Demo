using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class IceBlock : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // Destroy(melt) the ice block in 2 seconds.    
            Destroy(gameObject, 10);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

