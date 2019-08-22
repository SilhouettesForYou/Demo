using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class WallNext : MonoBehaviour
    {
        public bool next;
        public int curScenceIndex;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                if (next)
                    SceneManager.LoadScene(curScenceIndex + 1);
                else
                    SceneManager.LoadScene(0);
            }
        }

    }
}
