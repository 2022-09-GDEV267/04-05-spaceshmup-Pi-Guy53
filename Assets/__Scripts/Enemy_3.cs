using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    public float lifeTime = 5;

    private Vector3[] points;
    private float birthTime;

    private float u;

    private void Start()
    {
        points = new Vector3[3];

        points[0] = pos;

        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xmax = bndCheck.camWidth - bndCheck.radius;

        Vector3 v = Vector3.zero;
        v.x = Random.Range(xMin, xmax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xmax);
        points[2] = v;

        birthTime = Time.time;
    }

    public override void Move()
    {
        u = (Time.time - birthTime) / lifeTime;

        if(u > 1)
        {
            Destroy(gameObject);
            return;
        }


    }
}