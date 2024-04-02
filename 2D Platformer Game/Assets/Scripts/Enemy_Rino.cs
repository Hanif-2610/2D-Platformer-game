using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Rino specific info")]
    [SerializeField] float agroSpeed;
    [SerializeField] float shockTime;
                     float shockTimeCounter;
    

    protected override void Start()
    {
        base.Start();
        invincible = true;
    }

    void Update()
    {
        CollisionChecks();

        if (playerDetection.collider.GetComponent<Player>() != null)
            aggressive = true;

        if (!aggressive)
        {
            WalkAround();
        }
        else
        {
            if(!groundCheck)
            {
                aggressive = false;
                Flip();
            }

            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }

            if (shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                aggressive = false;
            }

            shockTimeCounter -= Time.deltaTime;
        }
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
