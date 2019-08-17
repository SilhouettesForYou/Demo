using UnityEngine;

namespace Demo
{
    public class GunnerProjectile :MonoBehaviour
    {
        public Vector2 initialForce;
        public float timer = 1;
        public float fuse = 0.01f;
        public GameObject explosion;
        public float explosionTimer = 3;
        private Rigidbody2D body;

        protected GameObject hitEffect;

        void OnDestroy()
        {
            if (transform != null)
            {
                hitEffect.transform.position = transform.position;
                hitEffect.SetActive(true);
                GameObject.Destroy(hitEffect, explosionTimer);
                Destroy(gameObject);
            }
            
        }

        void Start()
        {
            Destroy(gameObject, timer);

            hitEffect = Instantiate(explosion);
            hitEffect.SetActive(false);
        }


    }
}
