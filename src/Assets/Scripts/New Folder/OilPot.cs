using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPot : MonoBehaviour
{
    // Start is called before the first frame update
    bool falling = false;
    public float periodFall = 0.5f; 
    public GameObject oilSource; 
    public GameObject oilOnground;
    void Start()
    { 
        Demo.EventCenter.AddListener(Demo.EventType.AnDeath, reset);
        reset();
    }

    public void reset() 
    {
      
        if (falling) {
            falling = false;
            transform.Rotate(0,0,60);
        }
       
        oilOnground.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnTriggerEnter2D(Collider2D  other)
    {
       if (other.GetComponent<Demo.Pankapu>() != null &&  
            other.transform.position.x < transform.position.x && !falling
            && Demo.InputManager.InteractiveBtnDown) { //在左边
            transform.Rotate(0,0,-60);
            falling = true;
            if (oilSource != null)
            oilSource.SetActive(true);
            StartCoroutine(Timer());
       }
    }

    IEnumerator Timer() 
    {
        yield return new WaitForSeconds(periodFall);
        //transform.Rotate(0,0,60);
        if (oilSource != null)
        DestroyImmediate(oilSource);
        if (oilOnground != null) 
        oilOnground.SetActive(true);
        Debug.Log(string.Format("Timer2 is up !!! time=${0}", Time.time));
    }
}
