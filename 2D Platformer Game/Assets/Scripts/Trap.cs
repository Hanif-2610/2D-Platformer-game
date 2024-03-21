using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();

            if(player.transform.position.x > transform.position.x)
            {
                player.Knockback(1);
            }
            else if(player.transform.position.x < transform.position.x)
                player.Knockback(-1);
            else
                player.Knockback(0);
        }
    }
}
