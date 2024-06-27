using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPunchCollider : MonoBehaviour
{
    internal int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().ReceiveDamage(damage);
        }
    }
}
