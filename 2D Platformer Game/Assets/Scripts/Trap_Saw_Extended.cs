using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw_Extended : Trap
{
    private Animator anim;

    [SerializeField] Transform[] movePoint;
    [SerializeField] float speed;

    int movePointIndex;
    bool goingForward = true;

    
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);
        transform.position = movePoint[0].position;

        Flip();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            if(movePointIndex == 0)
            {
                Flip();
                goingForward = true;
            }
            
            
            if(goingForward)
                movePointIndex++;
            else
                movePointIndex--;

            if(movePointIndex >= movePoint.Length)
            {
                movePointIndex = movePoint.Length -1;
                goingForward = false;
                Flip();
            }
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }
}
