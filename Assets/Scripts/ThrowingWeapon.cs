using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowingWeapon : WeaponBase
{
    protected virtual void DoEffect()
    {
        GameObject explosion = Instantiate(particles, transform.position, transform.rotation);
        explosion.GetComponent<ThrowingWeaponExplosionTrigger>().damage = this.damage;
        Destroy(gameObject);
    }

    public void DoThrow(GameObject obj, Transform endPosition)
    {
        StartCoroutine(ObjectThrowing(obj, endPosition));
    }

    private IEnumerator ObjectThrowing(GameObject objectToThrow, Transform endPosition)
    {
        yield return new WaitForSecondsRealtime(1);


        objectToThrow.transform.parent = null;
        float throwTimer = 0;
        float throwPercent = 0;
        Vector3 startPosition = objectToThrow.transform.position;

        if (objectToThrow.TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = true;
        }

        while (throwPercent < 1)
        {
            throwTimer += Time.deltaTime;
            throwPercent = throwTimer / 0.7f;
            objectToThrow.transform.position = Vector3.Lerp(startPosition, endPosition.position, throwPercent)
                + Vector3.up * 5 * Mathf.Sin(throwPercent * Mathf.PI);
            yield return null;
        }

        transform.position -= new Vector3(0, transform.position.y, 0);
        transform.eulerAngles = Vector3.zero;
    }
}
