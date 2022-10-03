using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score;

    private BoundsCheck bndCheck;

    [Header("Set Dynamically: Enemy")]
    bool placeholder2; // here to keep VS from freaking out - DELETE IT

    private Vector3 tempPos;

    private void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();
        bndCheck.keepOnScreen = false;
    }

    // This is a property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("ProjectileHero"))
        {
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }
    }

    void ShowDamage()
    {

    }

    void UnShowDamage()
    {

    }
}
