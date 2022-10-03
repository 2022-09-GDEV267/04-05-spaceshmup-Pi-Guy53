using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Set In Inspector")]
    private bool placeHolderDELETE;

    [Header("Set Dynamically")]
    private BoundsCheck bndCheck;

    private void Start()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
}