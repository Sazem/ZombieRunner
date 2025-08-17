using UnityEngine;

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
        Debug.Log("Collision just hit us" + collision.gameObject.name);
    }
}
