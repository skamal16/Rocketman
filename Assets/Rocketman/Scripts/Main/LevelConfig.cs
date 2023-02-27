using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelAPI;

[CreateAssetMenu(fileName = "Data", menuName = "LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public Player player;
    public List<Enemy> enemyTypes;
    public Vector2 verticalBounds;
    public List<GameObject> powerupTypes;

    public List<IEnemy> GetEnemyTypes()
    {
        return enemyTypes.ConvertAll(enemy => enemy as IEnemy).ToList();
    }
}