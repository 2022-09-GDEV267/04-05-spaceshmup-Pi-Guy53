using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static Main S;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = .5f;
    public float enemyDefaultPadding = 1.5f;

    public WeaponDefinition[] weaponDefinitions;

    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield };

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    private BoundsCheck bndCheck;

    private int ndx;
    private float currentPadding;

    private void Awake()
    {
        S = this;
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    private void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void ShipDestroyed(Enemy e)
    {
        if(Random.value <= e.powerUpDropChance)
        {
            WeaponType puType = powerUpFrequency[Random.Range(0, powerUpFrequency.Length)];

            GameObject go = Instantiate(prefabPowerUp);
            PowerUp pu = go.GetComponent<PowerUp>();

            pu.SetType(puType);

            pu.transform.position = e.transform.position;
        }
    }

    public void SpawnEnemy()
    {
        ndx = Random.Range(0, prefabEnemies.Length);

        GameObject go = Instantiate(prefabEnemies[ndx]);

        currentPadding = enemyDefaultPadding;

        if (go.GetComponent<BoundsCheck>())
        {
            currentPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        go.transform.position = new Vector3(Random.Range(-bndCheck.camWidth + currentPadding, bndCheck.camWidth - currentPadding), bndCheck.camHeight + currentPadding, 0);

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
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
