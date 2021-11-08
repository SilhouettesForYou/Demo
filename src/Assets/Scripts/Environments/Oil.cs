using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LayerMask speedUp; 
    void OnTriggerEnter2D(Collider2D  other) 
    {
        DispearAtReset dispearAtReset;
        if ( (dispearAtReset =  other.GetComponent<DispearAtReset>()) != null) {
           dispearAtReset.dispear();
        }
         //Debug.Log("enter oil");
        SpeedUpOnOil speedUpObj = other.gameObject.GetComponent<SpeedUpOnOil>();
        if (speedUpObj == null) return;
        if ( (speedUp.value & (1<<other.gameObject.layer)) != 0) {
            speedUpObj.enterOil();
        }
    }
     void OnTriggerExit2D(Collider2D  other) 
    {
        SpeedUpOnOil speedUpObj = other.gameObject.GetComponent<SpeedUpOnOil>();
        if (speedUpObj == null) return;
        if ((speedUp.value & (1<<other.gameObject.layer)) != 0) {
           speedUpObj.exitOil();
        }
    }
}
