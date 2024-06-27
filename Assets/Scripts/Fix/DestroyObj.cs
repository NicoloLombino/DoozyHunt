using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objToDestroy;

    private void OnDestroy()
    {
        foreach(GameObject obj in objToDestroy)
        {
            Destroy(obj);
        }
    }
}
