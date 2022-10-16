using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float lerpSpeed;

    private GameObject hero;

    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
    }

    private void Update()
    {
        if (hero != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(hero.transform.position - transform.position), lerpSpeed);
        }
    }
}