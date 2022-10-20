using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public static Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = .5f;
    public float enemyDefaultPadding = 1.5f;

    public GameObject bossEnemy;
    public int scoreToSpawnBoss;
    private bool bossDeployed;

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    private BoundsCheck bndCheck;

    private int ndx;
    private float currentPadding;

    static public int highScore = 2000;
    private static int averageScore = 0;
    private int score;
    public Text scoreTxt;
    public Text highScoreTxt;
    
    
    public float disAverage;//show the average score

    private void Update()
    {
        disAverage = PlayerPrefs.GetInt("AverageScore");
    }
    private void Awake()
    {
        S = this;
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }

        if (PlayerPrefs.HasKey("AverageScore"))
        {
            averageScore = PlayerPrefs.GetInt("AverageScore");
        }

        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("AverageScore", averageScore);

        score = 0;

        scoreTxt.text = "Score: " + score;
        highScoreTxt.text = "High Score: " + highScore;
    }

    private void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void ShipDestroyed(Enemy e)
    {
        if (Random.value <= e.powerUpDropChance)
        {
            WeaponType puType = powerUpFrequency[Random.Range(0, powerUpFrequency.Length)];

            GameObject go = Instantiate(prefabPowerUp);
            PowerUp pu = go.GetComponent<PowerUp>();

            pu.SetType(puType);

            pu.transform.position = e.transform.position;
        }

        score += e.score;

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        if (e.GetComponent<Enemy_5>())
        {
            bossDeployed = false;
            scoreToSpawnBoss = scoreToSpawnBoss + score;

            averageScore = (averageScore + score) / 2;
            PlayerPrefs.SetInt("AverageScore", averageScore);
        }

        if (averageScore >= scoreToSpawnBoss && score > scoreToSpawnBoss && !bossDeployed)
        {
            GameObject go = Instantiate(bossEnemy);

            currentPadding = enemyDefaultPadding;
            go.transform.position = new Vector3(Random.Range(-bndCheck.camWidth + currentPadding, bndCheck.camWidth - currentPadding), bndCheck.camHeight + currentPadding, 0);

            bossDeployed = true;
        }

        scoreTxt.text = "Score: " + score;
        highScoreTxt.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
    }

    public void SpawnEnemy()
    {
        if (!bossDeployed)
        {
            ndx = Random.Range(0, prefabEnemies.Length);
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        }
        else
        {
            ndx = 2; //send out weak, un armed enemies to give the player something to crash into
            Invoke("SpawnEnemy", 2f / enemySpawnPerSecond);
        }

            GameObject go = Instantiate(prefabEnemies[ndx]);

            currentPadding = enemyDefaultPadding;

            if (go.GetComponent<BoundsCheck>())
            {
                currentPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            go.transform.position = new Vector3(Random.Range(-bndCheck.camWidth + currentPadding, bndCheck.camWidth - currentPadding), bndCheck.camHeight + currentPadding, 0);
    }

    public void DelayedRestart(float delay)
    {
        averageScore = (averageScore + score) / 2;
        PlayerPrefs.SetInt("AverageScore", averageScore);

        Invoke("Restart", delay);
    }

    public void Restart()
    {
        SceneManager.LoadScene("_SpaceSHMUP-Plus");
    }
    
    public static WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if(WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }

        return new WeaponDefinition();
    }
}