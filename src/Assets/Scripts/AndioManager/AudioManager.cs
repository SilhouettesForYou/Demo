using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Demo
{
    public class AudioManager : MonoBehaviour
    {
        public AudioClip[] AudioClipArray;                               //
        public static float AudioBackgroundVolumns = 1F;                 //
        public static float AudioEffectVolumns = 1F;                     //

        private static Dictionary<string, AudioClip> dicAudioClipLib;   //
        private static AudioSource[] audioSourceArray;                  //
        private static AudioSource audioSourceBackground;         //
        private static AudioSource audioSourceAudioEffect;            //

        /// <summary>
        /// 音效库资源加载
        /// </summary>
        void Awake()
        {
            //音频库加载
            dicAudioClipLib = new Dictionary<string, AudioClip>();
            foreach (AudioClip audioClip in AudioClipArray)
            {
                dicAudioClipLib.Add(audioClip.name, audioClip);
            }
            //处理音频源
            audioSourceArray = this.GetComponents<AudioSource>();
            audioSourceBackground = audioSourceArray[0];
            audioSourceAudioEffect = audioSourceArray[1];

            //从数据持久化中得到音量数值
            if (PlayerPrefs.GetFloat("Volumns") >= 0)
            {
                AudioBackgroundVolumns = PlayerPrefs.GetFloat("Volumns", 0.5f);
                audioSourceBackground.volume = AudioBackgroundVolumns;
                audioSourceAudioEffect.volume = AudioBackgroundVolumns * 0.5f;
            }
        }//Start_end

        /// <summary>
        /// Change music volume
        /// </summary>
        /// <param name="volume">

        public static void ChangeVolume(float volume)
        {
            audioSourceBackground.volume = volume;
            audioSourceAudioEffect.volume = volume * 0.5f;

        }

        public static void Save(string name, float volume)
        {
            PlayerPrefs.SetFloat(name, volume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="audioClip">
        private static void PlayBackground(AudioClip audioClip)
        {
            if (audioSourceBackground.clip == audioClip)
            {
                return;
            }
            //处理全局背景音乐音量
            audioSourceBackground.volume = AudioBackgroundVolumns;
            if (audioClip)
            {
                audioSourceBackground.clip = audioClip;
                audioSourceBackground.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] audioClip==null !");
            }
        }

        //播放背景音乐
        public static void PlayBackground(string strAudioName)
        {
            if (!string.IsNullOrEmpty(strAudioName))
            {
                PlayBackground(dicAudioClipLib[strAudioName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] strAudioName==null !");
            }
        }

        /// <summary>
        /// play effective music
        /// </summary>
        /// <param name="strAudioEffctName">
        public static void PlayEffect(string name, bool isLoop = false)
        {
            if (!string.IsNullOrEmpty(name))
            {
                AudioClip audioClip = dicAudioClipLib[name];
                if (audioClip)
                {
                    audioSourceAudioEffect.clip = audioClip;
                    audioSourceAudioEffect.Play();
                    audioSourceAudioEffect.loop = isLoop;
                }
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectB()] strAudioEffctName==null ! Please Check! ");
            }
        }

        public static void StopBackgroundMusic()
        {
            audioSourceBackground.Stop();
        }

        public static void StopEffect()
        {
            audioSourceAudioEffect.Stop();
        }

        /// <summary>
        /// change volume of background music 
        /// </summary>
        /// <param name="floAudioBGVolumns"></param>
        public static void SetAudioBackgroundVolumns(float floAudioBGVolumns)
        {
            audioSourceBackground.volume = floAudioBGVolumns;
            AudioBackgroundVolumns = floAudioBGVolumns;

            PlayerPrefs.SetFloat("Volumns", floAudioBGVolumns);
        }

        /// <summary>
        /// change voulme of effective music
        /// </summary>
        /// <param name="floAudioEffectVolumns"></param>
        public static void SetAudioEffectVolumns(float floAudioEffectVolumns)
        {
            AudioEffectVolumns = floAudioEffectVolumns * 0.5f;

            PlayerPrefs.SetFloat("Volumns", floAudioEffectVolumns);
        }
    }//Class_end
}

