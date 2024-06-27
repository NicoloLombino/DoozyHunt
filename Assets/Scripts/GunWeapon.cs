using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWeapon : WeaponBase
{
    [Header("Melee Components")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform shotTransform;

    // Pooling System
    [Header("Pooling System")]
    public List<GameObject> poolList;

    private void Start()
    {
        //PreparePooling();
    }
    public void StartAttack()
    {
        GameObject obj = Instantiate(bullet, shotTransform.position, shotTransform.rotation);
        //SpawnFromPool(shotTransform.position, shotTransform.rotation);
    }

    public void EndAttack()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("ATTACK");
            other.gameObject.GetComponent<Enemy>().ReceiveMeleeDamage(damage);
            GameObject effect = Instantiate(particles, other.transform.position + Vector3.up, other.transform.rotation, other.transform);
            Destroy(effect, 1);
        }
    }
    /*
    private void PreparePooling()
    {
        poolList = new List<GameObject>();

        foreach (GameObject pool in poolList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < 20; i++)
            {
                GameObject obj = Instantiate(pool);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolList.Add(pool.tag, objectPool);

        }
    }
    */
    /*
    public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    {
        poolList.RemoveAt(0);

        GameObject objectToSpawn = poolList.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    */
}
