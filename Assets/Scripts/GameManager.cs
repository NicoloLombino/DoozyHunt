using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EventsManager eventsManager;

    [Header("Score")]
    public int score;
    public int money;
    public int enemiesKilled;
    public int enemiesToKilForNextRound;
    public int totalEnemiesKilled;
    public int round;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI moneyText;
    public GameObject pausePanel;

    public Transform[] enemySpawner;
    public GameObject enemyPriestPrefab;

    [Header("Spawn Enemies")]
    public float enemyWaitTime;
    public float EnemyDecreaseWaitTime;
    public float EnemyMinWaitTime;

    [Header("Spawn Vodka")]
    public GameObject vodkaPrefab;
    public Transform[] vodkaSpawner;
    public float vodkaSpawnTime;
    public float vodkaTimer;

    [Header("Music")]
    [SerializeField]
    private AudioSource musicBackground;
    [SerializeField]
    private AudioSource musicEasterEgg;
    internal int easterEggMusicCounter;
    

    void Start()
    {
        eventsManager.killPrietsEvent.AddListener((moneyToGive) => HandleKillsPrietsEvent(moneyToGive));
        CheckNextRound();
    }

    // Update is called once per frame
    void Update()
    {
        VodkaSpawn();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // when the pause will be fixed delete exit and use pause
            //PauseSystem();
            ExitGame();
        }
    }

    private void HandleKillsPrietsEvent(int moneyToGive)
    {
        totalEnemiesKilled++;
        score += moneyToGive;
        money += moneyToGive;
        enemiesKilled++;
        UpdateMoneyOnUI();
        CheckNextRound();
    }

    public void UpdateMoneyOnUI()
    {
        moneyText.text = money.ToString();
        scoreText.text = score.ToString();
    }

    private void CheckNextRound()
    {
        if(enemiesKilled == enemiesToKilForNextRound)
        {
            round++;
            enemiesToKilForNextRound = 3 * round;
            enemiesKilled = 0;
            StartCoroutine(StartRoundAndSpawnEnemies());
        }
    }

    private IEnumerator StartRoundAndSpawnEnemies()
    {
        if(enemiesToKilForNextRound >= 50)
        {
            enemiesToKilForNextRound = 50;
        }

        // i want improve using pooling
        for(int i = 0; i < enemiesToKilForNextRound; i++)
        {
            bool canSpawn = false;
            while(!canSpawn)
            {
                int rndSpawnPoint = Random.Range(0, enemySpawner.Length);
                if(enemySpawner[rndSpawnPoint].gameObject.activeInHierarchy)
                {
                    GameObject enemy = Instantiate(enemyPriestPrefab,
                        enemySpawner[rndSpawnPoint].position,
                        enemySpawner[rndSpawnPoint].rotation);
                    canSpawn = true;
                    enemyWaitTime -= EnemyDecreaseWaitTime;
                    enemyWaitTime = Mathf.Max(1, enemyWaitTime);

                    if (enemiesToKilForNextRound >= 50)
                    {
                        enemy.GetComponent<Enemy>().maxHealth = 100 + round * 2;
                        enemy.GetComponent<Enemy>().currentHealth = 100 + round * 2;
                        enemy.GetComponent<Enemy>().damageIncrementByLevel += 2;
                        enemy.GetComponent<Enemy>().damageMelee += 2;
                    }

                    yield return new WaitForSecondsRealtime(enemyWaitTime);
                }
            }
        }
    }

    private void VodkaSpawn()
    {
        vodkaTimer += Time.deltaTime;
        if (vodkaTimer >= vodkaSpawnTime)
        {
            //for (int i = 0; i < enemiesToKilForNextRound; i++)
            //{

            //}

            bool canSpawn = false;
            while (!canSpawn)
            {
                int rndSpawnPoint = Random.Range(0, vodkaSpawner.Length);
                if (enemySpawner[rndSpawnPoint].gameObject.activeInHierarchy &&
                    vodkaSpawner[rndSpawnPoint].childCount == 0)
                {
                    GameObject enemy = Instantiate(vodkaPrefab,
                        vodkaSpawner[rndSpawnPoint].position,
                        vodkaSpawner[rndSpawnPoint].rotation,
                        vodkaSpawner[rndSpawnPoint]);
                    canSpawn = true;
                    vodkaTimer = 0;
                }
                else { return; }
            }
        }
    }

    public void StartMusicEasterEgg()
    {
        StartCoroutine(DoMusicEasterEgg());
    }

    private IEnumerator DoMusicEasterEgg()
    {
        musicBackground.gameObject.SetActive(false);
        musicEasterEgg.gameObject.SetActive(true);
        float musicTimer = musicEasterEgg.clip.length;
        yield return new WaitForSecondsRealtime(musicTimer);
        musicBackground.gameObject.SetActive(true);
        musicEasterEgg.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseSystem()
    {
        // need to fix something but i was busy
        //pausePanel.SetActive(!pausePanel.activeInHierarchy);
        //Time.timeScale = pausePanel.activeInHierarchy ? 0 : 1;
    }
}
