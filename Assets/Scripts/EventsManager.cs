using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<AttractBomb> pizzaEvent;
    public UnityEvent<int> killPrietsEvent;
    public UnityEvent weaponUsedEvent;
    public UnityEvent pizzaExplosionEvent;

    void Start()
    {
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    killPrietsEvent.Invoke();
        //}
    }
}
