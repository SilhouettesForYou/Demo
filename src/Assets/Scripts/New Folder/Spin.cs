using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Start is called before the first frame update
    public static float spinSpeed = 0f;
    public bool clockwise = true;
    private Quaternion initRotation;
    private Transform detect;
    private bool isStopping = false;
    private bool isDetecting = true;
    private bool isFirstDetecting = false;
    private bool isLeaving = false;
    private float time = 0;
    private float angle = 0;
    void Awake()
    {
        Demo.EventCenter.AddListener(Demo.EventType.AnRespawn, Reset);
        initRotation = transform.localRotation;
        detect = transform.parent.parent.Find("Detect");
        spinSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopping)
        {
            gameObject.transform.Rotate(0, 0, clockwise ? -spinSpeed : spinSpeed);
            //angle += spinSpeed;
            //if (Mathf.Abs(angle) > 89f)
            //{
            //    isStopping = true;

            //    SpinWheel.isStopping = true;
            //}
        }

        if (isDetecting)
        {
            Debug.DrawLine(detect.position, detect.position + Vector3.left * 5.0f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(detect.position, Vector2.left, 5f,
            1 << LayerMask.NameToLayer("MovablePlatform"));
            if (hit.transform != null)
            {
                if (hit.transform.name == "Cabin-1")
                {
                    isFirstDetecting = true;
                }
                if (isFirstDetecting)
                {
                    if (hit.transform.GetComponent<Spin>() != null)
                    {
                        isStopping = true;
                        SpinWheel.isStopping = true;
                    }
                }
                
            }
        }
        if (isStopping)
        {
            time += Time.deltaTime;
            if (time > 3.0f)
            {
                isStopping = false;
                angle = 0;
                SpinWheel.isStopping = false;
                isDetecting = false;
                isLeaving = true;

            }
        }

        if (isLeaving)
        {
            time += Time.deltaTime;
            if (time > 4.0f)
            {
                isDetecting = true;
                time = 0;
                isLeaving = false;
            }
        }

    }

    void OnDestroy()
    {
        Demo.EventCenter.RemoveListener(Demo.EventType.AnRespawn, Reset);
    }

    private void Reset()
    {
        isLeaving = false;
        isFirstDetecting = false;
        isDetecting = true;
        transform.localRotation = initRotation;
        spinSpeed = 0;
    }
}
