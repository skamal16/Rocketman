using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IUpdateable
{
    private Vector2 dir;
    private bool toShoot = false;
    private IDamageable shooter;
    private float bulletDamage;

    public float bulletSpeed;

    private float despawnRange;
    private bool toRemove = false;

    public void Shoot(Vector2 targetVector, float bulletDamage, IDamageable shooter = null)
    {
        dir = targetVector.normalized;
        toShoot = true;
        this.bulletDamage = bulletDamage;
        this.shooter = shooter;
        despawnRange = targetVector.magnitude;
    }

    // Update is called once per frame
    public void LevelUpdate()
    {
        if(despawnRange <= 0)
        {
            toRemove = true;
        }
    }

    public void LevelFixedUpdate()
    {
        if (toShoot)
        {
            Vector3 toMove = bulletSpeed * Time.deltaTime * dir;
            transform.position = transform.position + toMove;

            despawnRange -= toMove.magnitude;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<Player>();
        if (damageable == null) damageable = collision.gameObject.GetComponent<Enemy>();

        if(damageable != null && damageable != shooter)
        {
            damageable.Hit(bulletDamage);
            toRemove = true;
        }
    }

    public bool ToRemove()
    {
        return toRemove;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
