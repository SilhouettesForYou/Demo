using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class UnderWater : MonoBehaviour
    {
        private Rigidbody2D pankapuRigibody;
        private Transform pankapu;
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D anBody;
        private float mass;
        private float alpha = 1f;
        private float delta = 20f;
        private bool isDiving = false;
        // Start is called before the first frame update
        void Start()
        {
            mass = 95;
            EventCenter.AddListener(EventType.AnRespawn, Reset);
        }

        // Update is called once per frame
        void Update()
        {
            if (isDiving)
            {
                if (alpha > 0.1f)
                {
                    alpha -= Time.deltaTime / delta;
                    spriteRenderer.color = new Color(255, 255, 255, alpha);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                pankapuRigibody = collider.gameObject.GetComponent<Rigidbody2D>();
                pankapuRigibody.mass = mass;
                pankapu = collider.transform;
                spriteRenderer = pankapu.GetComponent<SpriteRenderer>();
                isDiving = true;
                EventCenter.Braodcast(EventType.Dive);
                EventCenter.Braodcast(EventType.WaterLeak);
                EventCenter.Braodcast<Transform>(EventType.DivingInPool, transform);
                AudioManager.PlayEffect("MusicDropIntoWater");
            }

            if (collider.transform.GetComponent<An>() != null)
            {
                anBody = collider.gameObject.GetComponent<Rigidbody2D>();
                anBody.mass = mass;
            }
        }

        private void Reset()
        {
            if (isDiving)
            {
                isDiving = false;
                spriteRenderer.color = new Color(255, 255, 1);
            }
            
        }
    }
}

