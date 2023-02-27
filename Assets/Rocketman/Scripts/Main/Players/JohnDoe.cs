using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelAPI;

public class JohnDoe : Player
{
    public float maxHealth;
    public Bullet bulletPrefab;
    public float bulletDamage;
    public float attackSpeed;
    public float moveSpeed;
    public float attackRange;

    private float attacksQueued = 0; 
    public override List<IUpdateable> Attack(List<IEnemy> enemies)
    {
        List<IUpdateable> bullets = new List<IUpdateable>();

        if(enemies.Count > 0)
        {
            enemies = enemies.OrderBy(enemy => (GetPosition() - enemy.GetPosition()).magnitude).ToList();
            Vector2 targetVector = enemies[0].GetPosition() - GetPosition();

            if (targetVector.magnitude < attackRange)
            {
                if (attacksQueued > 1)
                {
                    Bullet bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = GetPosition();
                    bullet.Shoot(targetVector.normalized * attackRange, bulletDamage, this);
                    bullets.Add(bullet);
                    attacksQueued = 0;
                }

                attacksQueued += attackSpeed * GetAttackSpeedMult() * Time.deltaTime;
            }
        }

        return bullets;
    }

    public override float GetMaxHealth()
    {
        return maxHealth;
    }
    public override float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public override IPlayer Spawn()
    {
        return Instantiate(this);
    }
}
