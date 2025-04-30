using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100f; 
    public float CurrentHealth; 
    private Dictionary<string, float> damageSources = new Dictionary<string, float>();
    public UnityEvent<float, float> OnHealthChanged; 
    public UnityEvent OnPlayerDeath; 
    public GameObject bloodPrefab; 
    public OVRCameraRig cameraRig;
    void Start()
    {
        Debug.Log("Player Health Initialized: " + MaxHealth);

        damageSources.Add("Enemy", 1f);
        damageSources.Add("Enemy2", 2f);
        
        CurrentHealth = MaxHealth; 
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player collided with: " + other.tag);
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy2"))
        {
            if (damageSources.ContainsKey(other.tag))
            {
                float damage = damageSources[other.tag] * Time.deltaTime; 
                CurrentHealth -= damage;
                
                if (CurrentHealth <= 0)
                {
                    CurrentHealth = 0;
                    OnPlayerDeath?.Invoke();
                }                
                OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            }
        }
        var blood = Instantiate(bloodPrefab, cameraRig.centerEyeAnchor.position, Quaternion.identity);
    }
}
