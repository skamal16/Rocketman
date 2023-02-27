using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField]
    private float difficultyScore;

    private Collider2D col;
    private float health;
    private bool toKill = false;

    private Action<float> onUpdateHealthListener;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        health = GetMaxHealth();
    }
    public abstract float GetMaxHealth();
    public abstract void Attack(IPlayer player);

    public Collider2D GetCollider()
    {
        return col;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void Hit(float attack)
    {
        health = Mathf.Max(0, health - attack);
        UpdateHealth();
    }
    private void UpdateHealth()
    {
        onUpdateHealthListener?.Invoke(health / GetMaxHealth());

        if(health <= 0)
        {
            toKill = true;
        }
    }

    public void SetOnUpdateHealthListener(Action<float> onUpdateHealth)
    {
        onUpdateHealthListener = onUpdateHealth;
    }

    public abstract IEnemy Spawn();

    public bool ToKill()
    {
        return toKill;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public float GetDifficultyScore()
    {
        return difficultyScore;
    }
}
