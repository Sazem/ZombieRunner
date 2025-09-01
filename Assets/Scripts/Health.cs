using System;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
public class Health : MonoBehaviour
{
    [SerializeField] int startingHealth = 5;
    [SerializeField] private int currentHealth;

    public Action onDeath;
    public event Action<int> OnHealthChanged;
    public IPushable pushable;
    [SerializeField] private GameObject deathPrefab;
    [SerializeField] private MMF_Player hurtFeedback;

    void Awake()
    {
        currentHealth = startingHealth;
        pushable = GetComponent<IPushable>();
        if (pushable != null)
        {
            print(pushable.ToString());
        }
        
    }
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hurtFeedback?.PlayFeedbacks(); // play hurt visuals, sfx etc.
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
        // Todo
        // -add signal, for GameManager, hud etc.
        //      if enemy, add score
        //      if player, stop game loop.
    }

    // give damage to the entity with pushforce.
    public void TakeHit(int damage, Vector3 hitDir, float hitForce)
    {
        pushable?.ReceivePush(hitDir, hitForce);
        TakeDamage(damage);
    }

    public void Die()
    {
        OnHealthChanged?.Invoke(0);
        // Evoke signal for possible:
        //      GameManager
        //      Hud
        //      etc.
        if (deathPrefab != null)
        {
            Instantiate(deathPrefab, this.transform.position, transform.rotation);
        }
        onDeath?.Invoke(); // null condition operator, if there is this has method registered it will call them. If empty, it will not do the invoke. 
        Destroy(gameObject);
    }
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
