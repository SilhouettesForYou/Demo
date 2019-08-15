using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class CameraFollow : MonoBehaviour
    {
        public float cameraSpeed = 5f;
        public float maxX = 32f;
        public float minX = 0f;

        private Transform player;

        void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<AssistPlayerControl>().transform;
            minX = player.transform.position.x;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vec = transform.position;
            if (player.position.x <= maxX && player.position.x >= minX)
            {
                vec.x = player.position.x;
            }
            vec.y = Mathf.Lerp(vec.y, player.position.y, Time.deltaTime * cameraSpeed);
            transform.position = vec;
        }
    }
}

