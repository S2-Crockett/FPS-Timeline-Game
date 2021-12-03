using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Respawn Transform")] 
    public string respawnName;
    public Transform spawnPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RespawnManager.Instance.AddRespawn(spawnPosition);
            UIManager.Instance.SetNewCheckpoint(respawnName);
            gameObject.SetActive(false);
            
            //add pick up and enemy information here as well (position & actual object / so we can respawn them correctly)
            //update 
        }
    }
    
    
}
