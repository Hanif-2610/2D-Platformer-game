using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    public Transform respwanPoint;
    [SerializeField] private GameObject playerPrefab;
    public GameObject currentPlayer;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            PlayerRespawn();
    }

    public void PlayerRespawn()
    {
        if(currentPlayer == null) 
            currentPlayer = Instantiate(playerPrefab, respwanPoint.position, transform.rotation);
    }
}
