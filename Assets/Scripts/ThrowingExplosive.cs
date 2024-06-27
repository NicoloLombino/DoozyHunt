using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingExplosive : ThrowingWeapon
{
    private void OnDestroy()
    {
        CallWeaponUsedEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Transform>() != null)
        {
            DoEffect();
        }
    }
}
