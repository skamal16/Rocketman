using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

public class RangedEnemyTest : Enemy
{
    public float maxHealth;
    public override void Attack(IPlayer player)
    {
        throw new System.NotImplementedException();
    }

    public override float GetMaxHealth()
    {
        return maxHealth;
    }

    public override IEnemy Spawn()
    {
        throw new System.NotImplementedException();
    }
}
