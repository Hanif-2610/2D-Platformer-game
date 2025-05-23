using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Transform respPoint;

    private void Awake()
    {
        PlayerManager.instance.respwanPoint = respPoint;
        PlayerManager.instance.PlayerRespawn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            if(collision.transform.position.x > transform.position.x)
                GetComponent<Animator>().SetTrigger("touch");
        }
            
    }
}
