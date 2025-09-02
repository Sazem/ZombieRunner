using UnityEngine;
using MoreMountains.Feedbacks;
public class Weapon : MonoBehaviour
{
    [SerializeField] private float cooldown = 1f; // time between each attack.
    public float Cooldown => cooldown; // "Getter" -only value for cooldown.
    [SerializeField] private string weaponName = ""; // name for hud
    [SerializeField] private int usesBeforeExpire = 5; // weapon before breaking down or no more ammos in weapon. When this hits zero, weapon is removed from inventory.
    [SerializeField] private Vector2Int minMaxDamage = new Vector2Int(1, 2); // random damage value, X = min damage, Y = max damage.
    [SerializeField] private float maxRange = 3f; // distance from player to target before attack
    public float MaxRange => maxRange;
    [SerializeField] private float weaponAngleRange = 45f; // angle from forward direction to left & and right. +
    [SerializeField] private MMF_Player attackFeedback;

    public virtual void Attack()
    {
        attackFeedback?.PlayFeedbacks(); // play sound, particles, screenshakes etc
    }

}
