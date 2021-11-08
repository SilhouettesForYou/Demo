using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGo : MonoBehaviour
{
    // Start is called before the first frame update
    public float swimmingSpeed = 0.5f;
    private Vector3 initPosition;
    private Quaternion initRotation;
    void Start()
    {
        initPosition = transform.position;
        initRotation = transform.localRotation;
        Demo.EventCenter.AddListener(Demo.EventType.AnRespawn, Reset);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0,swimmingSpeed,0)*Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Demo.Pankapu>() != null)
        {
            Demo.EventCenter.Braodcast(Demo.EventType.PankapuDeath);
        }
        if (collision.transform.GetComponent<Demo.An>() != null)
        {
            Debug.Log("An dead");
            Demo.EventCenter.Braodcast(Demo.EventType.AnDeath);
        }
    }


    void OnDestroy()
    {
        Demo.EventCenter.RemoveListener(Demo.EventType.AnRespawn, Reset);
    }

    private void Reset()
    {
        if (transform != null)
        {
            transform.position = initPosition;
            transform.localRotation = initRotation;
        }
        
    }
}
