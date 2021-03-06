using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Cinemachine;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static RespawnManager Instance;
    
    [Header("Respawn Points")]
    public List<Transform> respawnPoints = new List<Transform>();
    public float timeToRespawn = 4f;

    [Header("Repsawn Object")] 
    public GameObject player;
    private GameObject _player;

    [Header("Cameras")] 
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera deathCam;
    public CinemachineVirtualCamera startCam;

    private bool isFirstLevelLoad = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!isFirstLevelLoad)
        {
            isFirstLevelLoad = true;
            
            _player = Instantiate(player,GetLastRepsawn() );
            _player.transform.SetParent(null);
            Camera UICam = GameObject.Find("UICamera").GetComponent<Camera>();
            UIManager.Instance.SetCameraHUD(UICam);
            
            //register both of our cameras with the register
            CameraManager.Register(deathCam);
            CameraManager.Register(playerCam);
            CameraManager.Register(startCam);
            
            CameraManager.SwitchCamera(startCam);
            StartCoroutine(DelayedCameraStart());
        }
    }

    private IEnumerator DelayedCameraStart()
    {
        yield return new WaitForSeconds(2.0f);
        CameraManager.SwitchCamera(playerCam);
        _player.GetComponent<PlayerController>().isDead = false;
        UIManager.Instance.FadeGameHUD(0,1, 3.5f);
    }

    public void AddRespawn(Transform position)
    {
        // need to get a snapshot of the current level, take in all enemies and pickups and add
        // them to our respawn data along with position and name
        Transform newTransform = position;
        respawnPoints.Add(newTransform);
    }
    
    private Transform GetLastRepsawn()
    {
        if (respawnPoints.Count > 0)
        {
            return respawnPoints[respawnPoints.Count - 1];
        }
        return null;
    }

    public void Respawn()
    {
        var newPlayer = Instantiate(player,GetLastRepsawn() );
        newPlayer.transform.SetParent(null);
        newPlayer.GetComponent<PlayerController>().isDead = false;
        Camera UICam = GameObject.Find("UICamera").GetComponent<Camera>();
        UIManager.Instance.SetCameraHUD(UICam);
        
        Destroy(_player);
        newPlayer.GetComponent<PlayerDeathController>().SetRespawning(true);
        _player = newPlayer;
        
    }

    public float GetTimeToSpawn()
    {
        return timeToRespawn;
    }

    public class RespawnData
    {
        public string RespawnName;
        public Transform RespawnTransform;
        public List<GameObject> Enemies;
        public List<Transform> EnemyTransform;
        public List<GameObject> Pickups;
        public List<Transform> PickupTransforms;

    }
}
