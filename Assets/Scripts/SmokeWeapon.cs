using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeWeapon : WeaponBase
{
    [Header("Incense components")]
    [SerializeField]
    private float slownessEffect;
    [SerializeField]
    private float incenseChargeMax;
    public GameObject leftHand;

    private float currentIncenseCharge;
    bool hasSetChargeValue;

    private void Start()
    {
        if(!hasSetChargeValue)
        {
            hasSetChargeValue = true;
            currentIncenseCharge = incenseChargeMax;
        }
    }

    private void Update()
    {
        currentIncenseCharge -= Time.deltaTime;
        if(currentIncenseCharge <= 0 || Input.GetMouseButtonDown(1))
        {
            currentIncenseCharge = Mathf.Max(currentIncenseCharge, 0);
            DisableWeapon();
        }
    }

    private void DisableWeapon()
    {
        gameObject.SetActive(false);
        CallWeaponUsedEvent();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ReceiveSmoke(damage, slownessEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().RestoreSpeed();
        }
    }
}
