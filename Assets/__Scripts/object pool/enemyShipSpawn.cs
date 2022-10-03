using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShipSpawn : MonoBehaviour
{
    public GameObject[] ships;
    public int nextShipID;

    private void OnEnable()
    {
        for(int i = 0; i<ships.Length; i++)
        {
            ships[i].SetActive(false);
        }

        ships[nextShipID].SetActive(true);
    }
}