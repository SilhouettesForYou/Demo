using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class KnifeMovement : MonoBehaviour
    {
        private Vector3 fixedAncher;
        private Vector3 fixedPoint;
        private float width;
        private float height;
        private readonly float angle = 0.025f;
        private float time = 0;

        public bool isStopped = false;
        private Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            Demo.EventCenter.AddListener(Demo.EventType.KnifeStop,onRopeStop);
            Demo.EventCenter.AddListener(Demo.EventType.AnDeath,reset);
            pos = transform.position;
        }

        void OnDestroy()
        {
            Demo.EventCenter.RemoveListener(Demo.EventType.KnifeStop, onRopeStop);
            Demo.EventCenter.RemoveListener(Demo.EventType.AnDeath, reset);
        }


        void reset()
        {
            isStopped = false;
            Rigidbody2D rigidbody =  gameObject.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            rigidbody.velocity = new Vector2(0,0);
            pos.z = 0;
            transform.position = pos;
            
        }
        void onRopeStop()
        {
            isStopped = true;
            Vector3 pos = gameObject.transform.position;
            pos.z = 10;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isStopped)
            {
                time += Time.fixedDeltaTime;
                if (Mathf.Cos(time * 2.0f) > 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + angle);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - angle);
                }
            }
            
        }
        private Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            resultVec3.z = 0;
            return resultVec3;
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "ShockWave(Clone)")
            {
                Debug.Log("Rope is broken.");
                EventCenter.Braodcast(EventType.KnifeStop);
            }
        }
        public void RopeIsCut()
        {
            isStopped = true;
        }
    }
}

