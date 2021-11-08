using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    // Start is called before the first frame update
    public static float spinSpeed = 0f;
    public bool clockwise = true;
    private Quaternion initRotation;
    public static bool isStopping = false;
    void Start()
    {
        Demo.EventCenter.AddListener(Demo.EventType.AnRespawn, Reset);
        initRotation = transform.localRotation;
        spinSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopping)
            gameObject.transform.Rotate(0,0,clockwise?-spinSpeed:spinSpeed);
        
    }
    void OnDestroy()
    {
        Demo.EventCenter.RemoveListener(Demo.EventType.AnRespawn, Reset);
    }

    private void Reset()
    {
        transform.localRotation = initRotation;
        spinSpeed = 0;
    }
}
