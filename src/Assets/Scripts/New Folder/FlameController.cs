using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    public static bool trigered = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled =false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigered ) {
            gameObject.GetComponent<Renderer>().enabled = true;
        }
    }



}
