using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShipSpawn : MonoBehaviour
{
    public GameObject[] ships;
    public bool shipOut;

    public void setUp()
    {
        ships = new GameObject[Main.S.prefabEnemies.Length];

        for (int i = 0; i < ships.Length; i++)
        {
            ships[i] = Instantiate(Main.S.prefabEnemies[i]);
            ships[i].transform.parent = transform;

            ships[i].transform.position = transform.position;
            ships[i].SetActive(false);
        }
    }

    public GameObject getShip(int id)
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].transform.position = transform.position;
            ships[i].SetActive(false);
        }

        shipOut = true;

        ships[id].SetActive(true);

        return ships[id];
    }
}