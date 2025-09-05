using UnityEngine;
using UnityEngine.UIElements;

public class WeaponRanged : Weapon
{
    [SerializeField] private int projectilesPerShoot = 1; // pistol 1, shotgun example 6
    [SerializeField] private float spread = 0.0f; // spread, 0 => shoots directly into the dir.
    public GameObject hitEffect;
    public override void Attack()
    {
        base.Attack();
        Vector3 shootDir = transform.up;
        print("we are doing attack");
        if (spread > 0f)
        {
            shootDir = Quaternion.Euler(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0f) * shootDir;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDir, MaxRange, hitMask);

        if (hit.collider != null)
        {
            // hit.point
            print("we did hit" + hit.collider.gameObject.name);
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                Vector3 hitDir = shootDir.normalized;
                int nextDamage = Random.Range(minMaxDamage.x, minMaxDamage.y);
                print("next Damage was " + nextDamage);
                health.TakeHit(nextDamage, hitDir, pushForce);
            }
            Debug.DrawLine(transform.position, hit.point, Color.red, 0.5f);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.identity);
            }
        }
    }
}
