using TMPro;
using UnityEngine;
public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI itemPickUpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthText.text = "healtti on ny 5";
        itemPickUpText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealth(int newHealth)
    {
        healthText.text = "Health: " + newHealth;
    }

    public void ItemPickedUp(string itemName, float amount, float duration)
    {
        itemPickUpText.gameObject.SetActive(true);
        itemPickUpText.text = itemName + " " + amount.ToString() + " picked up";
        Invoke("DisableItemText", 3); // disable the text
    }

    private void DisableItemText()
    {
        itemPickUpText.gameObject.SetActive(false);
    }

}
