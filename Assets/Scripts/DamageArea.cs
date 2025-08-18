using System;
using UnityEditor.UI;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float pushAmount = 12;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float hitProbability = 0.6f;
    private float attackTimer = 0.0f;
    private bool attacked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer >= 0.0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            attackTimer = 0.0f;
        }            
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (attackTimer <= 0.0f) // dont attack, if cooldown is active
        {
            if (collision.CompareTag("Player"))
            {
                Vector3 dir = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
                // TODO
                // Make hit soundFX
                // play Hit Animation
                float randomProbability = UnityEngine.Random.Range(0f, 1f);
                if (randomProbability <= hitProbability)
                { // random hit.
                    Debug.DrawLine(transform.position, collision.transform.position, Color.red, 0.5f);
                    Health playerHealth = collision.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeHit(damage, dir, pushAmount);
                    }
                }
                attackTimer = attackCooldown;
            }
        }
    }
}
