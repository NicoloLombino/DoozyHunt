using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private AudioSource sound;

    private float timer;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if(timer >= 1)
        {
            sound.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ReceiveDamage(damage);
            GameObject effect = Instantiate(particles, transform.position, transform.rotation, transform);
            Destroy(gameObject, 0.1f);
        }

        if (other.CompareTag("Wall"))
        {
            GameObject effect = Instantiate(particles, transform.position, transform.rotation, transform);
            Destroy(gameObject, 0.1f);
        }
    }
}
