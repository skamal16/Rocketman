using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

[RequireComponent(typeof(Collider2D))]
public abstract class Player : MonoBehaviour, IPlayer
{
    private Collider2D col;
    private float health;

    private Action<float> onUpdateHealthListener;
    private Action<string> onPickPowerupListener;

    private float moveSpdMult = 1;
    private float attackSpdMult = 1;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        health = GetMaxHealth();
    }
    public abstract float GetMaxHealth();
    public abstract float GetMoveSpeed();
    public abstract List<IUpdateable> Attack(List<IEnemy> enemies);

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void Move(Vector2 velocity, Vector2 verticalBounds)
    {
        Debug.Log(velocity);
        Vector3 deltaPosition = GetMoveSpeed() * moveSpdMult * Time.deltaTime * velocity;
        Vector3 targetPos = transform.position + deltaPosition;
        targetPos.y = Mathf.Clamp(targetPos.y, verticalBounds.x, verticalBounds.y);

        transform.position = targetPos;
    }
    public void SetMoveSpeedMult(float mult)
    {
        moveSpdMult = mult;
    }
    public void SetAttackSpeedMult(float mult)
    {
        attackSpdMult = mult;
    }
    internal float GetAttackSpeedMult()
    {
        return attackSpdMult;
    }

    public abstract IPlayer Spawn();

    public void Hit(float attack)
    {
        health = Mathf.Max(0, health - attack);
        UpdateHealth();
    }
    private void UpdateHealth()
    {
        onUpdateHealthListener?.Invoke(health / GetMaxHealth());
    }

    public void SetOnUpdateHealthListener(Action<float> onUpdateHealth)
    {
        onUpdateHealthListener = onUpdateHealth;
    }

    public Collider2D GetCollider()
    {
        return col;
    }

    public void Reset()
    {
        health = GetMaxHealth();
        UpdateHealth();
        transform.position = Vector3.zero;
    }

    public void SetOnPickPowerupListener(Action<string> onPickPowerup)
    {
        onPickPowerupListener = onPickPowerup;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Powerup"))
        {
            onPickPowerupListener?.Invoke(collision.gameObject.name);
            Destroy(collision.gameObject);
        }
    }
}
