using UnityEngine;

namespace Demo
{
    public class Enemy : MonoBehaviour
    {
        public float speed = 1;
        public float patrolRadius = 5;

        private Animator animator;

        private bool isFacingRight = false;
        private Vector2 startPoint;
        private EnemyCheckHurtAssitPlayer checkHurt;

        void Awake()
        {
            checkHurt = GetComponent<EnemyCheckHurtAssitPlayer>();
            animator = GetComponent<Animator>();

            startPoint = transform.position;
            patrolRadius += Random.Range(0, 2);
            float r = Random.Range(0, 2);
            if (r == 0)
            {
                Flip();
            }
        }

        void Update()
        {
            if (isFacingRight)
            {
                Vector2 vec = transform.position;
                vec.x += Time.deltaTime * speed;

                if (vec.x <+ startPoint.x + patrolRadius)
                    transform.position = vec;
                else
                    Flip();
            }
            else
            {
                Vector2 vec = transform.position;
                vec.x -= Time.deltaTime * speed;

                if (vec.x >= startPoint.x - patrolRadius)
                    transform.position = vec;
                else
                    Flip();
            }
        }

        private void Flip()
        {
            Vector3 scale = transform.localScale;
            scale.x *= 1;
            transform.localScale = scale;
            isFacingRight = !isFacingRight;
        }

        public bool CheckPlayerBeHurt(AssistPlayerControl player)
        {
            bool playerBeHurt = false;
            if (player.isPlayHurtAnim)
            {
                checkHurt.enemyHurtPlayer = false;
            }

            if (!player.isPlayHurtAnim && checkHurt.enemyHurtPlayer)
            {
                player.isHurt = true;
                checkHurt.enemyHurtPlayer = false;
                playerBeHurt = true;
            }
            return playerBeHurt;
        }
    }
}
