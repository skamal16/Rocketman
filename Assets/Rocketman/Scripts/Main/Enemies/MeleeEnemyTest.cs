using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

[RequireComponent(typeof(Collider2D))]
public class MeleeEnemyTest : Enemy
{
    public float attack;
    public float moveSpeed;
    public float maxHealth;

    public override void Attack(IPlayer player)
    {
        if (player.GetCollider().IsTouching(GetCollider()))
        {
            player.Hit(attack * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.GetPosition(), moveSpeed * Time.deltaTime);
        }
    }

    public override float GetMaxHealth()
    {
        return maxHealth;
    }

    public override IEnemy Spawn()
    {
        return Instantiate(this);
    }
}
