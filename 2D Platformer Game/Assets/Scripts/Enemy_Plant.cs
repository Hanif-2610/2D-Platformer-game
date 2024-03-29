using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    protected override void Start()
    {
        base.Start();
    }


    void Update()
    {
        CollisionChecks();
        idleTimeCounter -= Time.deltaTime;

        bool playerDetected = playerDetection.collider.GetComponent<Player>() != null;

        if(idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
        }
    }

    private void AttackEvent()
    {
        Debug.Log("Attack!" + playerDetection.collider.name);
    }
}
