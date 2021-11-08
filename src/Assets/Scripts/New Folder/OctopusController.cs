using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 void OnTriggerEnter2D(Collider2D  other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pankapu")) {
            Debug.Log("attack");
            Demo.EventCenter.Braodcast(Demo.EventType.PankapuDeath);
           animator.SetBool("PlayerInside",true);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("AN"))
        {
            Debug.Log("attack");
            Demo.EventCenter.Braodcast(Demo.EventType.AnDeath);
            animator.SetBool("PlayerInside", true);
        }
        //other.isTrigger = true;
    }
    void OnTriggerExit2D(Collider2D  other)
    {
        //Debug.Log("enter");
         if (other.gameObject.layer == LayerMask.NameToLayer("Pankapu")) {
            animator.SetBool("PlayerInside",false);
         }
        if (other.gameObject.layer == LayerMask.NameToLayer("AN"))
        {
            animator.SetBool("PlayerInside", false);
        }
        //other.isTrigger = false;
    }

}
