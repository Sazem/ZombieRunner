using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float pushAmount = 200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collision just hit us" + collision.gameObject.name);
            Vector3 dir = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
            Debug.DrawLine(transform.position, collision.transform.position, Color.red, 0.5f);
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeHit(damage, dir, pushAmount);
            }
        }
    }
}
