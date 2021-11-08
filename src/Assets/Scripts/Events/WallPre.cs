using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class WallPre : MonoBehaviour
    {
        public bool pre;
        public int curScenceIndex;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                if (pre)
                    SceneManager.LoadScene(curScenceIndex - 1);
                else
                    SceneManager.LoadScene(curScenceIndex);
            }
        }

    }
}
