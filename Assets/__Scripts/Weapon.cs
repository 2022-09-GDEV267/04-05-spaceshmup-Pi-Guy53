using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,
    blaster,
    spread,
    phaser,
    missile,
    laser,
    shield,
    torpedo //possible addition sperate from missile
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;
    public Color color = Color.white;

    public GameObject projectilePref; //base system, use obj pooling

    public Color projectileColor = Color.white;
    public float damage = 0;
    public float DOTdamge = 0;
    public float shotDelay = 0;
    public float velocity;
}
public class Weapon : MonoBehaviour
{

}