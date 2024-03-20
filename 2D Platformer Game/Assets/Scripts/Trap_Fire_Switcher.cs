using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire_Switcher : MonoBehaviour
{
    public Trap_Fire myTrap;
    private Animator anim;

    // private float countdown;
    [SerializeField] float timeNotActive = 2;

    private void Start() 
    {
        anim = GetComponent<Animator>();
    }

    private void Update() 
    {
        // if(countdown > 0)
        //     countdown -= Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // if(countdown > 0)
        //     return;

        if(collision.GetComponent<Player>() != null)
        {
            // countdown = timeNotActive;
            anim.SetTrigger("pressed");
            myTrap.FireSwitchAfter(timeNotActive);
        }
    }
}
