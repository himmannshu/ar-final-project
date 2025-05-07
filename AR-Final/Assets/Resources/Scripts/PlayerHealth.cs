using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public AudioSource audioSource;
    public float MaxHealth = 100f; 
    public float CurrentHealth; 
    private Dictionary<string, float> damageSources = new Dictionary<string, float>();
    public UnityEvent<float, float> OnHealthChanged; 
    public UnityEvent OnPlayerDeath; 
    private Coroutine _soundCoroutine;
    void Start()
    {
        Debug.Log("Player Health Initialized: " + MaxHealth);

        damageSources.Add("Enemy", 1f);
        damageSources.Add("Enemy2", 0.75f);
        damageSources.Add("EnemySplitter", 0.5f);
        
        CurrentHealth = MaxHealth; 
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player collided with: " + other.tag);
        if (other.CompareTag("Enemy") || other.CompareTag("Enemy2") || other.CompareTag("EnemySplitter"))
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
			if (_soundCoroutine == null)
			{
				audioSource.enabled = true; 
				_soundCoroutine = StartCoroutine(SelfDestruct(1f));
			}
        }
        //audioSource.enabled = true;
    }

    IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        audioSource.enabled = false;
        _soundCoroutine = null;
    }
}
