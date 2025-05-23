using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Animator anim;

    [SerializeField] Transform[] movePoint;
    [SerializeField] float speed;
    [SerializeField] float cooldown;

    float cooldownTimer;
    int movePointIndex;


    
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].position;

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
            cooldownTimer = cooldown;
            movePointIndex++;

            if(movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
            collision.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
            collision.transform.SetParent(null);
    }

}
