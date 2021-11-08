using System;
using UnityEngine;


namespace Demo
{
    public class Resolution : MonoBehaviour
    {
        public Camera mainCamera;
        void Start()
        {
            //Screen.SetResolution(1280, 800, true, 60);
            mainCamera = Camera.main;
            mainCamera.aspect = 1.78f;
        }
    }
}
