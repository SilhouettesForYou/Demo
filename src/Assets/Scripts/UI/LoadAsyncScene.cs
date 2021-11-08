using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Demo
{
    public class LoadAsyncScene : BaseUI
    {
        private Text progress;
        private Image loadingFront;
        private float value;
        private AsyncOperation async = null;
        public static string sceneName;
        private float timer = 0;
        void Start()
        {
            progress = UnityHelper.Find(transform, "LoadingValue").GetComponent<Text>();
            loadingFront = UnityHelper.Find(transform, "ProgressFront").GetComponent<Image>();
            if (UIManager.isLogin)
            {
                OpenUI(UINameConst.SelectLevelUI);
                Close();
            }
            else
            {
                StartCoroutine(LoadScene());
            }
            
        }

        IEnumerator LoadScene()
        {
            async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;
            while (!async.isDone)
            {
                value = async.progress;
                loadingFront.fillAmount = value / 0.9f;
                progress.text = (int)(value * 100 / 0.9f) + " %";
                if (Mathf.Abs(async.progress - 0.9f) < 0.0001)
                {
                    async.allowSceneActivation = true;
                    Close();
                }
                yield return null;
            }
        }

    }
}
