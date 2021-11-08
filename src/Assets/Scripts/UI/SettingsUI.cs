using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class SettingsUI : BaseUI
    {

        private bool isMusicOn = true;
        private float volume;
        void Awake()
        {
            curUIType.type = UIFormType.Normal;
            curUIType.mode = UIFormShowMode.Normal;
            curUIType.lucencyType = UIFormLucencyType.Lucency;

            //注册按钮
            RegisterBtnOnClick("Cancel", Cancel);
            RegisterBtnOnClick("Confirm", Confirm);
            RegisterBtnOnClick("MusicOff", MusicTrigger);
        }

        private void Update()
        {
            volume = UnityHelper.Find(transform, "Slider").GetComponent<Slider>().value;
            AudioManager.ChangeVolume(volume);
        }

        public void Cancel(GameObject go)
        {
            Close();
        }

        public void Confirm(GameObject go)
        {
            AudioManager.Save("Volumns", volume);
            Close();
        }

        private void MusicTrigger(GameObject go)
        {
            isMusicOn = !isMusicOn;
            Transform musicOn = UnityHelper.Find(transform, "MusicOn");
            float value = !isMusicOn ? 0 : PlayerPrefs.GetFloat("Volumns", 0.5f);
            Debug.Log(value);
            AudioManager.ChangeVolume(value);
            musicOn.gameObject.SetActive(isMusicOn);
        }
    }
}
