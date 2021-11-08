using System.Collections;
using UnityEngine;

namespace Demo
{
    public class BeforeLoadingUI : BaseUI
    {

        void Start()
        {
            UIManager.isStartUILoaded = true;
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            LoadAsyncScene.sceneName = "Level-1";
            yield return new WaitForSeconds(2.0f);
            OpenUI(UINameConst.LoadingUI);
            Close();
        }
    }


}
