using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class WallNext : MonoBehaviour
    {
        public int curScenceIndex;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.name == "An'")
            {
                if (curScenceIndex == 3)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene(curScenceIndex + 1);
            }
        }

    }
}
