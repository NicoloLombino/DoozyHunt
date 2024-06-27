using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public int pointsForOpen;
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private GameObject[] enemySpawner;

    [SerializeField]
    private TextMeshProUGUI[] pointsText;
    void Start()
    {
        foreach (TextMeshProUGUI text in pointsText)
        {
            text.text = pointsForOpen.ToString();
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject spawner in enemySpawner)
        {
            if(!spawner.activeInHierarchy)
            {
                spawner.SetActive(true);
            }
        }

        GameObject effect = Instantiate(particles, 
            transform.position + Vector3.up * 0.5f, transform.rotation);
        Destroy(effect, 2);
    }
}
