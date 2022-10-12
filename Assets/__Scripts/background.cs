using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    public Hero playerHero;
    public Vector2 scrollSpeed;

    private Material backgroundMat;
    private Vector2 playerPos;

    private void Start()
    {
        backgroundMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        playerPos = playerHero.get2DPos();
        //use player pos to control background scroll later;

        backgroundMat.mainTextureOffset += scrollSpeed * Time.deltaTime;
    }
}