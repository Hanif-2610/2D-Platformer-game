using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat Specific")]
    [SerializeField] private Transform[] idlePoint;
    
    private Vector2 destination;
    private bool canBeAggressive = true;
    private bool playerDetected;
    private Transform player;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    float defaultSpeed;
    
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
    }


    void Update()
    {
        anim.SetBool("canBeAggressive", canBeAggressive);
        anim.SetFloat("speed", speed);

        idleTimeCounter -= Time.deltaTime;
        if(idleTimeCounter > 0)
            return;

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

        if(playerDetected && !aggressive && canBeAggressive)
        {
            aggressive = true;
            canBeAggressive = false;
            destination = player.transform.position;
        }

        if(aggressive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        
            if(Vector2.Distance(transform.position, destination) < .1f)
            {
                aggressive = false;

                int i = Random.Range(0, idlePoint.Length);

                destination = idlePoint[i].position;
                speed = speed * .5f;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, destination) < .1f)
            {
                if(!canBeAggressive)
                    idleTimeCounter = idleTime;
                
                canBeAggressive = true;
                speed = defaultSpeed;
            }
        }

        FlipController();
    }

    public override void Damage()
    {
        base.Damage();
        idleTimeCounter = 5;
    }

    private void FlipController()
    {
        if(facingDirection == -1 && transform.position.x < destination.x)
            Flip();
        else if(facingDirection == 1 && transform.position.x > destination.x)
            Flip();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
