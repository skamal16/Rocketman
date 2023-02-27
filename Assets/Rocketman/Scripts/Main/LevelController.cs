using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelAPI;

public class LevelController
{
    private IPlayer player;
    private List<IEnemy> enemyTypes;
    private Vector2 verticalBounds;

    private List<IEnemy> enemies;
    private float difficultyLevel;

    private Vector2 velocity;
    private bool toMove = false;

    private float score = 0;
    private Action<float> onUpdateScore = null;

    private bool paused = false;
    private bool endGame = false;

    private List<IUpdateable> updateables;

    private IUIController uiController;

    private PowerupController powerupController;
    private List<GameObject> powerupTypes;

    private readonly float ultCooldown = 5f;
    private float ultTimer;

    internal void Init(IUIController uiController, LevelConfig levelConfig)
    {
        player = levelConfig.player;
        enemyTypes = levelConfig.GetEnemyTypes();
        verticalBounds = levelConfig.verticalBounds;

        enemies = new List<IEnemy>(); 
        difficultyLevel = 1;

        this.uiController = uiController;

        uiController.SetOnMoveListener(OnMove);
        uiController.SetOnIdleListener(OnIdle);
        uiController.SetOnPressUltimateListener(OnPressUltimate);
        uiController.SetOnPauseListener(onPause);
        uiController.SetOnResumeListener(onResume);
        uiController.SetOnRetryListener(onRetry);

        onUpdateScore = (score) => uiController.UpdateScore(score);

        player = player.Spawn();
        player.SetOnUpdateHealthListener((healthRatio) =>
        {
            uiController.UpdateHealth(healthRatio);
            if (healthRatio <= 0) endGame = true;
        });
        player.SetOnPickPowerupListener((powerupName) =>
        {
            powerupController.AddPowerUp(powerupName, player, uiController);
        });

        updateables = new List<IUpdateable>();

        powerupController = new PowerupController();
        powerupTypes = levelConfig.powerupTypes;
    }

    private void onRetry()
    {
        player.Reset();
        enemies.ForEach(enemy => enemy.Kill());
        enemies.Clear();
        updateables.ForEach(i => i.Destroy());
        updateables.Clear();

        difficultyLevel= 0;
        score= 0;
        paused = false;
    }
    private void onResume()
    {
        paused = false;
    }

    private void onPause()
    {
        paused = true;
    }

    internal void Start()
    {
        uiController.Camera.MoveTo(player.GetPosition(), verticalBounds);
    }

    internal void Update()
    {
        if (endGame)
        {
            paused = true;
            uiController.GameOver();
            endGame = false;
        }
        if (paused) return;

        difficultyLevel += Time.deltaTime;

        float currentDifficultyScore = 0;

        foreach(IEnemy enemy in enemies)
        {
            currentDifficultyScore += enemy.GetDifficultyScore();
        }

        int loopBreak = 0;

        while (currentDifficultyScore < Math.Pow(difficultyLevel, 1.2) && loopBreak <= 10)
        {
            loopBreak++;

            List<IEnemy> spawnableEnemyTypes = enemyTypes.Where(enemy => enemy.GetDifficultyScore() <= difficultyLevel - currentDifficultyScore).ToList();

            if (spawnableEnemyTypes.Count == 0) break;

            int index = UnityEngine.Random.Range(0, spawnableEnemyTypes.Count);

            IEnemy enemyToSpawn = enemyTypes[index].Spawn();

            if (enemyToSpawn != null)
            {
                Vector2 randLoc = UnityEngine.Random.insideUnitCircle.normalized * uiController.Camera.GetBounds();
                Vector2 position = player.GetPosition() + randLoc;

                enemyToSpawn.SetPosition(position);
                enemies.Add(enemyToSpawn);
                currentDifficultyScore += enemyToSpawn.GetDifficultyScore();
            }
        }

        updateables.RemoveAll(i => {
            if (i.ToRemove())
            {
                i.Destroy();
                return true;
            }
            return false;
        });
        updateables.ForEach(i => i.LevelUpdate());

        UpdateScore();
    }

    private void UpdateScore()
    {
        onUpdateScore.Invoke(score);
    }

    internal void FixedUpdate()
    {
        if (paused) return;

        if(toMove) player.Move(velocity, verticalBounds);
        uiController.Camera.MoveTo(player.GetPosition(), verticalBounds);

        enemies.RemoveAll(enemy => 
        {
            if (enemy.ToKill())
            {
                Vector3 pos = enemy.GetPosition();
                enemy.Kill();
                score += enemy.GetDifficultyScore();

                bool toSpawnPowerup = UnityEngine.Random.Range(0f, 1f) <= 0.25;

                if (toSpawnPowerup)
                {
                    int index = UnityEngine.Random.Range(0, powerupTypes.Count);

                    if (powerupTypes.Count > index)
                    {
                        var powerup = UnityEngine.Object.Instantiate(powerupTypes[index], pos, Quaternion.identity);
                        powerup.name = powerupTypes[index].name;
                    }
                }

                return true;
            }
            else return false;
        });

        updateables.AddRange(player.Attack(enemies));

        foreach (IEnemy enemy in enemies)
        {
            enemy.Attack(player);
        }

        updateables.ForEach(i => i.LevelFixedUpdate());

        powerupController.Update();

        if(ultTimer > 0)
        {
            ultTimer -= Time.deltaTime;
            if (ultTimer <= 0) uiController.SetUltimateAvailable();
        }
    }

    private void OnMove(Vector2 velocity)
    {
        //Debug.Log("Touch coord at: " + target.x+ ", " + target.y);
        this.velocity = velocity;
        toMove = true;
    }

    private void OnIdle()
    {
        toMove = false;
    }

    private void OnPressUltimate()
    {
        Debug.Log("Ultimate Pressed");
        ultTimer = ultCooldown;
    }
}
