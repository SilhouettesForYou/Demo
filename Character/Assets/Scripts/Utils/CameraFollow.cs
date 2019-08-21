using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class CameraFollow : MonoBehaviour
    {
        public float cameraSpeed = 5f;
        public float maxX = 32f;
        public float minX = 0f;

        private bool focusOnPlayer = true;

        private Transform player;
        private Transform pool;

        void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<AssistPlayerControl>().transform;
            Scene curScene = SceneManager.GetActiveScene();
            if (curScene.name == "Level-1-3")
                pool = FindObjectOfType<UnderWater>().transform;
            minX = player.transform.position.x;

            EventCenter.AddListener<bool>(EventType.FocusOn, ChangeFocusOn);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vec = transform.position;
            if (focusOnPlayer)
            {
                if (player.position.x <= maxX && player.position.x >= minX)
                {
                    vec.x = player.position.x;
                }
                vec.y = Mathf.Lerp(vec.y, player.position.y, Time.deltaTime * cameraSpeed);
                transform.position = vec;
            }
            else
            {
                vec.x = pool.position.x;
                vec.y = pool.position.y;
                transform.position = vec;
            }
        }

        private void ChangeFocusOn(bool flag)
        {
            focusOnPlayer = flag;
        }
    }
}

