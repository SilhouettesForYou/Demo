using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class IceBlock : MonoBehaviour
    {
        private bool isRespawn = false;
        // Start is called before the first frame update
        void Start()
        {
            // Destroy(melt) the ice block in 2 seconds.    
            EventCenter.AddListener(EventType.AnRespawn, Dispear);
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("LittlePuddle").GetComponent<EdgeCollider2D>(), 
                transform.GetComponent<CapsuleCollider2D>());
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("LittlePuddle").GetComponent<EdgeCollider2D>(),
                transform.GetComponent<BoxCollider2D>());
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDestroy()
        {

            EventCenter.RemoveListener(EventType.AnRespawn, Dispear);
        }

        private void Dispear()
        {
            if (!isRespawn)
            {
                Destroy(gameObject);
                isRespawn = true;
            }
                
        }
    }
}

