using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objPool : MonoBehaviour
{
    public static objPool S;

    public GameObject playerShotPref;
    public int numOfPShots;
    private List<GameObject> playerShotList;

    public GameObject enemyShotPref;
    public int numOfEnemyShots;
    private List<GameObject> enemyShotList;

    public GameObject enemyShipPref;
    public int numOfEnemyShips;
    private List<GameObject> enemyShipList;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        playerShotList = new List<GameObject>();
        enemyShotList = new List<GameObject>();
        enemyShipList = new List<GameObject>();

        GameObject tempGo;

        for (int i = 0; i < numOfEnemyShips; i++)
        {
            tempGo = Instantiate(enemyShipPref);
            tempGo.GetComponent<enemyShipSpawn>().setUp();

            enemyShipList.Add(tempGo);
            tempGo.GetComponent<enemyShipSpawn>().shipOut = false;
        }
    }

    public GameObject getEnemyShip(int shipID)
    {
        for (int i = 0; i < enemyShipList.Count; i++)
        {
            if (!enemyShipList[i].GetComponent<enemyShipSpawn>().shipOut)
            {
                return enemyShipList[i].GetComponent<enemyShipSpawn>().getShip(shipID);
            }
        }

        return null;
    }
}