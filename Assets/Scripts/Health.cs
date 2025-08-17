using System;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int startingHealth = 5;
    private int currentHealth;

    public Action onDeath;
    public Action onHealthChanged;
    void Start()
    {
        startingHealth = currentHealth;
        GameManager.Instance.HelloWorld();
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        // Todo
        // -VFX
        // -add signal, for GameManager, hud etc.
        //      if enemy, add score
        //      if player, stop game loop.
    }

    // give damage to the entity with pushforce.
    public void TakeHit(int damage, Vector3 hitPoint, float hitForce)
    {
        TakeDamage(damage);
    }

    public void Die()
    {
        // Evoke signal for possible:
        //      GameManager
        //      Hud
        //      etc.
        onDeath?.Invoke(); // null condition operator, if there is this has method registered it will call them. If empty, it will not do the invoke. 
        // remove entitys
    }
    
}
