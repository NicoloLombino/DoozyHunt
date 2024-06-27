using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractBomb : ThrowingWeapon
{
    [SerializeField]
    private float timeToExplode;

    private float timer;

    private AudioSource audioSource;


    private void Awake()
    {
        eventsManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventsManager>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        eventsManager.pizzaEvent.Invoke(this);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeToExplode)
        {
            DoEffect();
            audioSource.Stop();
        }
    }

    private void OnDisable()
    {
        eventsManager.pizzaExplosionEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            CallWeaponUsedEvent();
        }
    }
}
