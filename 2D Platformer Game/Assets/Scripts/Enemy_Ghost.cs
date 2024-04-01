using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Header("Ghost Specific")]
    [SerializeField] private float activeTime;
                     private float activeTimeCounter = 4;

    private Transform player;
    private SpriteRenderer sr;

    [SerializeField] private float[] xOffset;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        aggressive = true;
        invincible = true;

        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if(activeTimeCounter > 0)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if(activeTimeCounter < 0 && idleTimeCounter < 0 && aggressive)
        {
            anim.SetTrigger("disappear");
            aggressive = false;
            idleTimeCounter = idleTime;
        }

        if(activeTimeCounter < 0 && idleTimeCounter < 0 && !aggressive)
        {
            ChoosePosition();
            anim.SetTrigger("appear");
            aggressive = true;
            activeTimeCounter = activeTime;
        }

        if(facingDirection == -1 && transform.position.x < player.transform.position.x)
            Flip();
        else if(facingDirection == 1 && transform.position.x > player.transform.position.x)
            Flip();
    }

    private void ChoosePosition()
    {
        float _xOffset = xOffset[Random.Range(0, xOffset.Length)];
        float _yOffset = Random.Range(-10, 10);
        transform.position = new Vector2(player.transform.position.x + _xOffset, player.transform.position.y + _yOffset);
    }

    public void Disappear() => sr.enabled = false;
    public void Appear() => sr.enabled = true;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(aggressive)
            base.OnTriggerEnter2D(collision);
    }
}
