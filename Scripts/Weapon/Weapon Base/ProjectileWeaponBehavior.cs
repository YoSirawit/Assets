using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using UnityEngine;

//Base Script of all projectile behaviors [To be placed on a prejab of a weapon that is a projectile]
public class ProjectileWeaponBehavior : MonoBehaviour
{

    public WeaponScriptableObject weaponData;
    protected Vector3 direction;
    public float destroyAferSeconds;

    //Current Stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponData.Damage;
        currentSpeed = weaponData.Speed;
        currentCooldownDuration = weaponData.CooldownDuration;
        currentPierce = weaponData.Pierce;
    }
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAferSeconds);
        
    }

    public void DirectionChecker(Vector3 dir)
    {
        direction = dir;
        Debug.Log(dir);

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if(dirx < 0 && diry == 0) // Left
        {
            scale.x = scale.x*-1;
            scale.y = scale.y*-1;
        }
        else if(dirx == 0 && diry < 0) //Down
        {
            scale.y = scale.y*-1;
        }
        else if(dirx == 0 && diry > 0) //Up
        {
            scale.x = scale.x*-1;
        }
        else if(dirx < 0 && diry > 0) //LeftUp
        {
            scale.x = scale.x*-1;
            scale.y = scale.y*-1;
            rotation.z = -90f;
        }
        else if(dirx > 0 && diry > 0) //RightUp
        {
            rotation.z = 0f;
        }
        else if(dirx < 0 && diry < 0) //LeftDown
        {
            scale.x = scale.x*-1;
            scale.y = scale.y*-1;
            rotation.z = 0f;
        }
        else if(dirx > 0 && diry < 0) //RightDown
        {
            rotation.z = -90;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);// can't simply set the vector because cannot convert
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //Reference the script from the collided collider and deal damage using Takedamage()
        if(col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage); //Use current damage
            ReducePierce();
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakable))
            {
                breakable.TakeDamage(currentDamage);
                ReducePierce();
            }
        }
    }

    void ReducePierce() //Destroy when the pierce number is 0
    {
        currentPierce--;
        if(currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}
