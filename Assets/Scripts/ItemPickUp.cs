using UnityEngine;
using MoreMountains.Feedbacks;
public class ItemPickUp : MonoBehaviour
{
    public enum ItemType { Health, Speed }
    [SerializeField] private float amount; // amount health, speed etc to be added to the player.
    [SerializeField] private float duration; // how long the effect will last in seconsd, 0 = instant.  

    [SerializeField] private ItemType itemType;
    [SerializeField] private MMF_Player pickUpEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory inv = collision.GetComponent<Inventory>();
        if (inv != null)
        {
            inv.AddItem(itemType, amount, duration);
            if (pickUpEffect != null)
            {
                pickUpEffect.PlayFeedbacks();
                // remember to remove the effect at the MMFeedback
            }
            else
            {
                Destroy(gameObject); // remove this after effect added.
            }
        }
    }


}
