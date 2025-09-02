using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class Shooter : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;
    private float currentCooldown = 0.0f;
    private bool isCoolingdown = false;
    private float coolingDownTimer = 0.0f;

    private GameObject currentTarget;
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnemyAtRange()) // check if the enemy is at the weapon attack range. 
        {
            // find the closest enemy inside of the range
            
            // attack if we have weapon
            if (currentWeapon != null)
            {
                if (isCoolingdown == false) // weapon is not currently cooling, do attack and get the cooldown time.
                {
                    currentWeapon.Attack();
                    isCoolingdown = true;
                    coolingDownTimer = currentWeapon.Cooldown;
                }
                else if (coolingDownTimer >= 0.0f)
                {
                    coolingDownTimer -= Time.deltaTime;
                }
                if (coolingDownTimer <= 0.0f) // reset the timer
                {
                    isCoolingdown = false;
                }
            }
        }
    }

    bool IsEnemyAtRange() {
        if(enemiesInRange.Count == 0)
            return false;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance <= currentWeapon.MaxRange)
            {
                currentTarget = enemy;
                break; // Stop at the first valid target
            }
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!enemiesInRange.Contains(collision.gameObject))
            {
                enemiesInRange.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    if (collision.CompareTag("Enemy"))
        {
            if (enemiesInRange.Contains(collision.gameObject))
            {
                enemiesInRange.Remove(collision.gameObject);
            }
        }
    }

}
