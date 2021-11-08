using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkController : MonoBehaviour
{
    public Transform upLimit,downLimit;
    private Vector3 upLimitPos,downLimitPos;
    public float speed = 0.3f;
    private Vector3 speedV3;
    bool needMove = true;
    bool headdingUp = true;
    Vector3 posDefault;
    // Start is called before the first frame update
    void Start()
    {

         Demo.EventCenter.AddListener(Demo.EventType.AnDeath, reset);
        posDefault = transform.position;
        reset();
        
    }

    void OnDestroy()
    {
        Demo.EventCenter.RemoveListener(Demo.EventType.AnDeath, reset);
    }
    void reset()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = posDefault;

        if (upLimit == null || downLimit == null) return;

        upLimitPos = upLimit.position;
        downLimitPos = downLimit.position;

        if ((upLimitPos - downLimitPos).magnitude < speed) {
            needMove = false;
            return;
        }

        speedV3 = (upLimitPos- downLimit.position).normalized*speed;
        Vector3 disToUp = upLimitPos - posDefault;
        Vector3 disToDown = downLimitPos - posDefault;
        
        if (disToDown.magnitude < disToUp.magnitude) {
            speedV3 *= -1f;
            headdingUp = false;
        }else {
            headdingUp = true;
        }
        //速度朝upLimit
    }


    bool reach(Vector3 pos) {
        float dX =  (gameObject.transform.position - pos).magnitude;
        return dX <= speed;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (upLimit == null || downLimit == null) return;
        if (!needMove) return;

        if ((headdingUp && reach(upLimitPos))  || (!headdingUp && reach(downLimitPos))) {
            speedV3 *= -1;
            headdingUp = !headdingUp;
        }else {
            transform.position += speedV3*Time.deltaTime;
        }

    }
}
