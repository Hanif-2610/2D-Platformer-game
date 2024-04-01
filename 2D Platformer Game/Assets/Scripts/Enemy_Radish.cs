using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{
    private RaycastHit2D groundBelowDetected;
    private RaycastHit2D groundAboveDetected;

    [Header("Radish Specific")]
    [SerializeField] private float ceilingDistance;
    [SerializeField] private float groundDistance;

    [SerializeField] private float aggroTime;
                     private float aggroTimeCounter;

    [SerializeField] private float flyForce;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        aggroTimeCounter -= Time.deltaTime;

        if(aggroTimeCounter < 0 && !groundAboveDetected)
        {
            rb.gravityScale = 1;
            aggressive = false;
        }
        
        if(!aggressive)
        {
            if(groundBelowDetected && !groundAboveDetected)
            {
                rb.velocity = new Vector2(0, flyForce);
            }
        }
        else
        {
            if(groundBelowDetected.distance <= 1.25f)
                WalkAround();
        }
        
        CollisionChecks();

        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("aggresive", aggressive);
    }

    public override void Damage()
    {
        if(!aggressive)
        {
            aggroTimeCounter = aggroTime;
            rb.gravityScale = 12;
            aggressive = true;
        }
        else
            base.Damage();
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingDistance, whatIsGround);
        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceilingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }
}
