using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwitchController : MonoBehaviour
{
    // Start is called before the first frame update
    public float spinSpeed = 0.5f;
    private bool isReset = false;
    bool triggered = false;
    private bool isPushing = false;
    private Transform angel;
    private Transform devil;
    Animator animator;
    enum Status {
        LEFT,MID, RIGHT
    }
    Status s = Status.MID;
    void Awake()
    {
        animator = GetComponent<Animator>();
        angel = transform.Find("Angel");
        devil = transform.Find("Devil");

        isReset = true;
        animator.SetBool("chooseRight", false);
        animator.SetBool("chooseWrong", false);
        triggered = false;
        Spin.spinSpeed = 0;

        Demo.EventCenter.AddListener<bool>(Demo.EventType.Push, CheckPushing);
        Demo.EventCenter.AddListener(Demo.EventType.AnRespawn, Reset);
    }

    void Ondestroy()
    {
        Demo.EventCenter.RemoveListener<bool>(Demo.EventType.Push, CheckPushing);
        Demo.EventCenter.RemoveListener(Demo.EventType.AnRespawn, Reset);
    }

    void clockwise(bool bw)
    {
        int factor = 1;
        if (bw) {
            
            animator.SetBool("chooseWrong",true);
        }else {
            animator.SetBool("chooseRight",true);
            factor = -1;
        }
        Spin.spinSpeed = spinSpeed*factor;
        SpinWheel.spinSpeed = spinSpeed * factor;
    } 
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Reset", isReset);
        if (!triggered && !isPushing)
        {
            if (Demo.InputManager.InteractiveBtnDown && CheckTrigger(angel))
            {
                clockwise(false);
                triggered = true;
                isReset = false;
            }
            if (Demo.InputManager.InteractiveBtnDown && CheckTrigger(devil))
            {
                clockwise(true);
                triggered = true;
                isReset = false;
            }
        }
        
    }
    public LayerMask player;
    private void CheckPushing(bool flag)
    {
        isPushing = flag;
    }

    private bool CheckTrigger(Transform trigger)
    {
        Collider2D collider = Physics2D.OverlapCircle(trigger.position, 0.2f,
            1 << LayerMask.NameToLayer("Pankapu"));
        if (collider != null)
            return true;
        return false;
    }

    private void Reset()
    {
        if (animator != null)
        {
            isReset = true;
            animator.SetBool("chooseRight", false);
            animator.SetBool("chooseWrong", false);
            triggered = false;
            Spin.spinSpeed = 0;
        }
        
    }
}
