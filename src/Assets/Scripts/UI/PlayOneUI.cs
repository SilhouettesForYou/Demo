using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class PlayOneUI : BaseUI
    {

        void Start()
        {
            StartCoroutine(Delay());
        }
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(5.0f);
            OpenUI(UINameConst.SelectLevelUI);
            Close();
        }
    }

    
}
