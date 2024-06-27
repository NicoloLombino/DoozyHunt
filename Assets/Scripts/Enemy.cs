using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private Animator anim;
    public float distance;

    [Header("References")]
    public EventsManager eventManager;
    public Transform playerTransform;

    [Header("Stats")]
    public float maxHealth;
    [SerializeField]
    private float baseSpeed;
    [SerializeField]
    private float distanceToThrowAttack;
    [SerializeField]
    private float distanceToMeleeAttack;
    [SerializeField]
    private float healthToRunAwayPercentage;
    public int damageMelee;

    private float currentSpeed;
    public float currentHealth;
    public float runToPlayerMaxTime;

    [Header("Effects")]
    [SerializeField]
    private GameObject slowEffect;

    [Header("AI components")]
    public Transform target;
    [Range(0,9)]
    public float percentageToRunAwayFromFight;

    [Header("Others")]
    public int moneyToGive;
    [SerializeField]
    private Transform throwTransform;
    [SerializeField]
    private GameObject[] throwObject;
    [SerializeField]
    private Collider colPunchBase;
    [SerializeField]
    private Collider colPunchStrong;

    internal float healthToRunAway;
    internal bool isInThrowDistance;
    internal bool isInMeleeDistance;
    internal bool isFollowingPizza;

    public bool isRunningAway;
    public int damageIncrementByLevel;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        target = playerTransform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventsManager>();
        eventManager.pizzaEvent.AddListener((child) => HandlePizzaEvent(child));
        eventManager.pizzaExplosionEvent.AddListener(() => HandlePizzaExplosionEvent());
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthToRunAway = (healthToRunAwayPercentage / maxHealth) * 100;
        agent.SetDestination(target.position);
    }

    void Update()
    {
        if(!isRunningAway)
        {
            agent.SetDestination(target.position);
            CheckDistanceFromPlayer();
        }
    }

    private void OnDestroy()
    {
        eventManager.killPrietsEvent.Invoke(moneyToGive);
    }

    private void CheckDistanceFromPlayer()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance < distanceToThrowAttack)
        {
            isInThrowDistance = true;

            if(distance < distanceToMeleeAttack)
            {
                isInMeleeDistance = true;
            }
            else
            {
                isInMeleeDistance = false;
            }
        }
        else
        {
            isInMeleeDistance = false;
            isInThrowDistance = false;
        }
    }

    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // dead
            anim.SetTrigger("Death");
            gameObject.tag = "Untagged";
            agent.speed = 0;
        }
        else
        {
            // hurt
            anim.SetTrigger("ReceiveDamage");
            agent.speed = 0;
        }
    }

    public void ReceiveMeleeDamage(float damage)
    {
        ReceiveDamage(damage);

        // animations
    }

    public void ReceiveSmoke(float damage, float slowness)
    {
        if(!slowEffect.activeInHierarchy)
        {
            slowEffect.SetActive(true);
        }
        agent.speed -= slowness;
        agent.speed = Mathf.Max(agent.speed, 1);

        ReceiveDamage(damage);
    }

    private void HandlePizzaEvent(AttractBomb attractBomb)
    {
        target = attractBomb.transform;
        isFollowingPizza = true;
        isRunningAway = false;
        anim.SetBool("HasPizza", true);
    }

    private void HandlePizzaExplosionEvent()
    {
        target = playerTransform;
        isFollowingPizza = false;
        anim.SetBool("HasPizza", false);
    }

    public void RestoreSpeed()
    {
        agent.speed = baseSpeed;
    }

    public void BlockMovement()
    {
        agent.speed = 0;
    }

    public void RunAwayFromFight(Transform runAwayPosition)
    {
        target = runAwayPosition;
        agent.SetDestination(target.position);
        isRunningAway = true;
    }

    public void ThrowObject()
    {
        if (isFollowingPizza) return;

        int rnd = UnityEngine.Random.Range(0, throwObject.Length);
        GameObject obj = Instantiate(throwObject[rnd], throwTransform.position + Vector3.up, throwTransform.rotation);
        obj.GetComponent<ThrowingObject>().damage += damageIncrementByLevel;
    }

    public void StartPunchBase()
    {
        if (isFollowingPizza) return;

        colPunchBase.enabled = true;
        colPunchBase.gameObject.GetComponent<EnemyPunchCollider>().damage = damageMelee;
    }

    public void StartPunchStrong()
    {
        colPunchStrong.enabled = true;
        colPunchStrong.gameObject.GetComponent<EnemyPunchCollider>().damage = damageMelee + 3;
    }

    public void EndPunch()
    {
        colPunchBase.enabled = false;
        colPunchStrong.enabled = false;
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
    }
}
