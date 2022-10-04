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

    private BoundsCheck bndCheck;

    private int ndx;
    private float currentPadding;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();

        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    public void SpawnEnemy()
    {
        ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = objPool.S.getEnemyShip(ndx);

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
        SceneManager.LoadScene("_Scene_0");
    }
    
}
