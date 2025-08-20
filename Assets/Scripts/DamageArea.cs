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
    [SerializeField]
    private TargetType targetFlags;

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
            if (IsTarget(collision.tag)) // check if damagearea is touching correct target -group.
            {
                Vector3 dir = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
                // TODO
                // Make hit soundFX
                // play Hit Animation
                float randomProbability = UnityEngine.Random.Range(0f, 1f);
                if (randomProbability <= hitProbability)
                { // random hit.
                    Debug.DrawLine(transform.position, collision.transform.position, Color.red, 0.5f);
                    Health health = collision.GetComponent<Health>();
                    if (health != null)
                    {
                        health.TakeHit(damage, dir, pushAmount);
                    }
                }
                attackTimer = attackCooldown;
            }
        }
    }

    private bool IsTarget(string tag)
    {
        if (tag == "Player" && targetFlags.HasFlag(TargetType.Player)) return true;
        if (tag == "Enemy"  && targetFlags.HasFlag(TargetType.Enemy))  return true;
        return false;
    }


    [Flags]
    public enum TargetType
    {
        None   = 0,
        Player = 1 << 0,
        Enemy  = 1 << 1,
    }

}
