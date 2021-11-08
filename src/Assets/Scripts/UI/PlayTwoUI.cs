using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class PlayTwoUI : BaseUI
    {

        void Start()
        {
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(3.0f);
            OpenUI(UINameConst.SelectLevelUI);
            Close();
        }
    }

    
}
