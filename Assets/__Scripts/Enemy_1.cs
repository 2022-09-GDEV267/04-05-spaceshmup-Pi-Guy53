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

    private Vector3 tPos, rot;
    float age, theta, sin;

    private void Start()
    {
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        tPos = pos;
        age = Time.time - birthTime;
        theta = Mathf.PI * 2 * age / waveFrequency;
        sin = Mathf.Sin(theta);

        tPos.x = x0 + waveWidth * sin;
        pos = tPos;

        rot = new Vector3(0, sin * waveRotY, 0);
        transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }
}