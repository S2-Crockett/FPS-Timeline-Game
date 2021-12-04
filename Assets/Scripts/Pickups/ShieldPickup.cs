using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : MonoBehaviour
{
    [Header("Shield")] 
    public int AddShieldAmount;
    public AudioClip pickupSound;

    [Header("Floating")] 
    public float rotationPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 posOffset;
    private Vector3 tempPos;

    private HealthComponent _healthComponent;
    // Start is called before the first frame update
    void Start()
    {
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotationPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _healthComponent = other.GetComponent<HealthComponent>();
            _healthComponent.AddShield(AddShieldAmount);
            SoundManager.Instance.PlayPickupSound(pickupSound);
            //play sound? 
            Destroy(gameObject);
        }
    }
}
