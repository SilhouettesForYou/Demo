using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UnderWater : MonoBehaviour
    {
        public delegate void ChangeStateToSwim();
        public static event ChangeStateToSwim changeStateToSwim;

        private AssistPlayerControl playerControl;
        private Rigidbody2D playerRigibody;
        private float mass = 40;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            playerRigibody = collider.gameObject.GetComponent<Rigidbody2D>();
            playerRigibody.mass = mass;
            changeStateToSwim();            
        }
    }
}

