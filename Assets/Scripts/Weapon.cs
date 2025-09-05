using UnityEngine;
using MoreMountains.Feedbacks;
public class Weapon : MonoBehaviour
{
    [SerializeField] private float cooldown = 1f; // time between each attack.
    public float Cooldown => cooldown; // "Getter" -only value for cooldown.
    [SerializeField] protected string weaponName = ""; // name for hud
    [SerializeField] protected int usesBeforeExpire = 5; // weapon before breaking down or no more ammos in weapon. When this hits zero, weapon is removed from inventory.
    [SerializeField] protected Vector2Int minMaxDamage = new Vector2Int(1, 2); // random damage value, X = min damage, Y = max damage.
    [SerializeField] protected float pushForce = 12.0f;
    [SerializeField] protected float maxRange = 3.0f; // distance from player to target before attack
    public float MaxRange => maxRange;
    [SerializeField] protected float weaponAngleRange = 45.0f; // angle from forward direction to left & and right. +
    public float WeaponAngleRange => weaponAngleRange;
    [SerializeField] protected float minAngleAttack = 20.0f; // Weapon will attack, when the angle is this or less. (bigger angle, less accurary but player will shoot earlier)
    public float MinAngleAttack => minAngleAttack;
    [SerializeField] protected MMF_Player attackFeedback;
    [SerializeField] protected LayerMask hitMask; 
    public virtual void Attack()
    {
        attackFeedback?.PlayFeedbacks(); // play sound, particles, screenshakes etc
    }

}
