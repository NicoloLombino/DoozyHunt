using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    internal EventsManager eventsManager;
    [Header("Components")]
    public Sprite sprite;
    [SerializeField]
    protected float damage;
    [SerializeField]
    protected GameObject particles;

    private void Awake()
    {
        eventsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventsManager>();
    }
    public void CallWeaponUsedEvent()
    {
        eventsManager.weaponUsedEvent.Invoke();
    }
}