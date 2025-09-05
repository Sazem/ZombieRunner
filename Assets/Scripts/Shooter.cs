using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class Shooter : MonoBehaviour
{
    [SerializeField] Weapon currentWeapon;
    private float currentCooldown = 0.0f;
    private bool isCoolingdown = false;
    private float coolingDownTimer = 0.0f;
    [SerializeField] private float rotationSpeed = 1.0f; // radians per seconds.
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float currentAngle;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // optimization todo:
        // Dont check enemies at range every frame, only when OnEnter or OnExit has been triggered. 
        if (IsEnemyAtRange()) // check if the enemy is at the weapon attack range. 
        {
            // find the closest enemy inside of the range
            currentTarget = FindClosestEnemy();
            // attack if we have weapon
            if (currentWeapon != null && currentTarget != null)
            {
                // rotate the shooter towards the currentTarget.
                // from Unity Docs: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.RotateTowards.html
                Vector3 current = transform.up; // y axis by default.
                Vector3 targetDirection = currentTarget.transform.position - transform.position;
                float angleToTarget = Vector3.Angle(transform.parent.up, targetDirection.normalized);
                currentAngle = angleToTarget;
                if (angleToTarget <= currentWeapon.WeaponAngleRange) // rotate weapon if the enemy is inside on the weapon angle range. 
                {
                    transform.up = Vector3.RotateTowards(current, targetDirection, rotationSpeed * Time.deltaTime, 0f);
                }
                float shooterAngleToTarget = Vector3.Angle(transform.up, targetDirection.normalized); // the "weapon" angle comparing to the target
                if (isCoolingdown == false && shooterAngleToTarget <= currentWeapon.MinAngleAttack) // weapon is not currently cooling and weapon is rotated inside of the threshold angle for attack: do attack and get the cooldown time.
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
            else
            {
                // smooth rotate the weapon towards forward direction
                Vector3 current = transform.up;
                Vector3 forward = transform.parent.up;
                transform.up = Vector3.RotateTowards(current, forward, rotationSpeed * Time.deltaTime, 0f);
            }
        }
    }

    bool IsEnemyAtRange() {
        if (enemiesInRange.Count == 0)
            return false;
        else {
            return true;
        }
    }
    GameObject FindClosestEnemy()
    {
        if (enemiesInRange.Count == 0)
        {
            return null;
        }
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            // check the closest target that is inside of the weapons angle.
            if (distance <= currentWeapon.MaxRange && distance < closestDistance && IsInsideWeaponAngle(enemy))
            {
                closestTarget = enemy;
                closestDistance = distance;
            }
        }
        return closestTarget;
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

    // calculate the angle between the enemy and the shooter using weapons max angle. If the enemy is inside, return true.
    private bool IsInsideWeaponAngle(GameObject enemy)
    {
        Vector3 targetDirection = enemy.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.parent.up, targetDirection.normalized);
        if (angleToTarget <= currentWeapon.WeaponAngleRange) {
            return true;
        }
        else {
            return false;
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
