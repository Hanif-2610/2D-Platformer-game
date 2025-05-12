using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [SerializeField] private Transform respwanPoint;
    [SerializeField] private GameObject playerPrefab;
                     public GameObject currentPlayer;

    void Awake()
    {
        instance = this;
        PlayerRespawn(); 
    }

    private void PlayerRespawn()
    {
        if(currentPlayer == null) 
            currentPlayer = Instantiate(playerPrefab, respwanPoint.position, transform.rotation);
    }
}
