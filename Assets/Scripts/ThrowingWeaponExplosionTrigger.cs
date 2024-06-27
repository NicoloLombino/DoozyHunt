using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeaponExplosionTrigger : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;

    internal float damage;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
        }
    }
}
