using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Components")]
    [SerializeField]
    private Collider col;
    [SerializeField]
    private AudioSource audioSource;

    public void StartAttack()
    {
        col.enabled = true;
    }

    public void EndAttack()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            audioSource.Play();
            Debug.Log("ATTACK");
            other.gameObject.GetComponent<Enemy>().ReceiveMeleeDamage(damage);
            GameObject effect = Instantiate(particles, other.transform.position + Vector3.up, other.transform.rotation, other.transform);
            Destroy(effect, 1);
        }

        if(other.CompareTag("SecretWall"))
        {
            Destroy(other.gameObject);
        }
    }
}
