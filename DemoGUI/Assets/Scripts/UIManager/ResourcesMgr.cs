﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo

{
    public class ResourcesMgr : MonoBehaviour
    {
        private static ResourcesMgr m_Instance;              //本脚本私有单例实例
        private Hashtable ht = null;                        //容器键值对集合




        /// <summary>
        /// 得到实例(单例)
        /// </summary>
        /// <returns></returns>
        public static ResourcesMgr GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject("_ResourceMgr").AddComponent<ResourcesMgr>();
            }
            return m_Instance;
        }

        void Awake()
        {
            ht = new Hashtable();
        }

        /// <summary>
        /// 调用资源（带对象缓冲技术）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="isCatch"></param>
        /// <returns></returns>
        public T LoadResource<T>(string path, bool isCatch) where T : UnityEngine.Object
        {
            if (ht.Contains(path))
            {
                return ht[path] as T;
            }

            T TResource = Resources.Load<T>(path);
            if (TResource == null)
            {
                Debug.LogError(GetType() + "/GetInstance()/TResource 提取的资源找不到，请检查。 path=" + path);
            }
            else if (isCatch)
            {
                ht.Add(path, TResource);
            }

            return TResource;
        }

        /// <summary>
        /// 调用资源（带对象缓冲技术）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isCatch"></param>
        /// <returns></returns>
        public GameObject LoadAsset(string path, bool isCatch)
        {
            GameObject goObj = LoadResource<GameObject>(path, isCatch);
            GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj);
            if (goObjClone == null)
            {
                Debug.LogError(GetType() + "/LoadAsset()/克隆资源不成功，请检查。 path=" + path);
            }

            return goObjClone;
        }
    }
}
