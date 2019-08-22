using UnityEngine;
using UnityEngine.SceneManagement;

namespace Demo
{
    public class SceneCut : MonoBehaviour
    {
        private Scene scene;
        private string lastSceneName = "Level-1-1";
        void OnGUI()
        {
            scene = SceneManager.GetActiveScene();
            string curSceneName = scene.name;
            if (curSceneName == "Level-1-1" || curSceneName == "Level-1-2" || curSceneName == "Level-1-3")
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAttack, true);
            }
            else
            {
                EventCenter.Braodcast<bool>(EventType.FrezeeAttack, true);
            }

            if (curSceneName != lastSceneName)
            {
                InputManager.ClearInput();
                lastSceneName = curSceneName;
            }

        }
    }
}
