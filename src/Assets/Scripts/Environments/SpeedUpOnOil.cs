using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpOnOil : MonoBehaviour
{
     bool onOil;
     public float speedupRate = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool isOnOil()
    {
        return onOil;
    } 
    public void enterOil()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        onOil = true;
    }
    public void exitOil()
    {
    
        onOil = false;
    }

    void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    
}
