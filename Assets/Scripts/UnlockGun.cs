using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGun : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.eulerAngles += Vector3.up * 20 * Time.deltaTime;
    }
}
