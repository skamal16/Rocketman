using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelAPI
{
    public interface IJoystick
    {
        void End();
        Vector2 GetAxis(Vector3 mousePosition);
        void Begin(Vector2 position);
        void Init(ICamera cameraController);
        bool IsActive();
    }

    public interface ICamera
    {
        Vector2 GetBounds();
        Vector2 GetPosition();
        public void MoveTo(Vector2 position, Vector2 verticalBounds);
    }
    public interface IEnemy : IDamageable
    {
        public IEnemy Spawn();
        public void Attack(IPlayer player);
        public Vector2 GetPosition();
        public bool ToKill();
        public void SetPosition(Vector2 position);
        public void Kill();
        public float GetDifficultyScore();
    }
    public interface IUIController
    {
        ICamera Camera { get; }

        void SetOnMoveListener(Action<Vector2> onMove);
        void SetOnIdleListener(Action onIdle);
        void SetOnPressUltimateListener(Action onPressUltimate);
        void UpdateHealth(float healthRatio);
        void SetOnUpdateHealthListener(Action<float> onUpdateHealth);
        void UpdateScore(float score);
        void SetOnPauseListener(Action onPause);
        void SetOnResumeListener(Action onResume);
        void SetOnRetryListener(Action onRetry);
        void GameOver();
        void AddPowerup(string powerup, Dictionary<string, Action<float>> listeners);
        void SetUltimateAvailable();
    }
    public interface IPlayer : IDamageable
    {
        public IPlayer Spawn();
        public void Move(Vector2 velocity, Vector2 verticalBounds);
        public Vector2 GetPosition();
        public List<IUpdateable> Attack(List<IEnemy> enemies);
        void Reset();
        void SetOnPickPowerupListener(Action<string> onPickPowerup);
        void SetMoveSpeedMult(float mult);
        void SetAttackSpeedMult(float mult);
    }
    public interface IUpdateable
    {
        public void LevelUpdate();
        public void LevelFixedUpdate();
        bool ToRemove();
        void Destroy();
    }
    public interface IDamageable
    {
        public void Hit(float attack);
        public Collider2D GetCollider();
        public void SetOnUpdateHealthListener(Action<float> onUpdateHealth);
    }
}
