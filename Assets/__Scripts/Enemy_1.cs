using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    public float waveFrequency = 2;
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0;
    private float birthTime;

    private Vector3 tempPos, rot;
    float age, theta, sin;

    private void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        tempPos = pos;
        age = Time.time - birthTime;
        theta = Mathf.PI * 8 * age / waveFrequency;
        sin = Mathf.Sin(theta);

        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        rot = new Vector3(0, sin * waveRotY, 0);

        base.Move();
    }
}