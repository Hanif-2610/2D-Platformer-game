using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : Trap
{
    private Animator anim;
    bool isWorking;

    [SerializeField] Transform[] movePoint;
    [SerializeField] float speed;
    [SerializeField] float cooldown;

    float cooldownTimer;
    int movePointIndex;


    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        bool isWorking = cooldownTimer <0;
        anim.SetBool("isWorking", isWorking);

        if(isWorking)
            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            Flip();
            cooldownTimer = cooldown;
            movePointIndex++;

            if(movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }
}
