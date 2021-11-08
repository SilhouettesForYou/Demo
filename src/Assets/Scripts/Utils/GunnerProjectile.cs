using UnityEngine;

namespace Demo
{
    public class GunnerProjectile :MonoBehaviour
    {
        public Vector2 initialForce;
        public float timer = 0.5f;
        public float fuse = 0.01f;
        public float explosionTimer = 1;
        private Rigidbody2D body;


        void Start()
        {
            Destroy(gameObject, timer);
        }


    }
}
