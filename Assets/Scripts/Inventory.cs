using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] PlayerMovement playerMovement;
    private float currentSpeedBuffDuration = 0.0f;
    public void AddItem(ItemPickUp.ItemType itemType, float amount, float duration)
    {
        switch (itemType)
        {
            case ItemPickUp.ItemType.Health:
                playerHealth?.AddHealth((int)amount);
                break;
            case ItemPickUp.ItemType.Speed:
                AddSpeed(amount, duration);
                break;
        }
    }

    public void AddSpeed(float amount, float duration)
    {
        // add speed to player x duration
        // reset speed after duration.
        // if speed is already buffed, add only duration
    }
}
