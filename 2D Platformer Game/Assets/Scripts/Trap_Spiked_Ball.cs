using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spiked_Ball : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 pushDirection;

    
    void Start()
    {
        rb.AddForce(pushDirection,ForceMode2D.Impulse);
    }

}
