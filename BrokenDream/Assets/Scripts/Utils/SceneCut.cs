using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class SceneCut : MonoBehaviour
    {
        private Scene scene;
        private string lastSceneName = "Level-1-1";
        void Awake()
        {
            scene = SceneManager.GetActiveScene();
            string curSceneName = scene.name;
            if (curSceneName == "Level-1")
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAttack, true);
            }
            else
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAttack, false);
            }

            if (curSceneName != lastSceneName 
                || (lastSceneName == "Level-1-1" && curSceneName == "Level-1-1")
                || (lastSceneName == "Level-2-1" && curSceneName == "Level-2-1"))
            {
                InputManager.ClearInput();
                lastSceneName = curSceneName;
            }

        }
    }
}
