using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // backbone of the data structure
    [SerializeField] protected List<string> enemies;

    public string GetRandomEnemy()
    {
        System.Random randGen = new System.Random();
        if (enemies.Count > 0)
        {
            return enemies[randGen.Next(0, enemies.Count - 1)];
        }
        else
        {
            return enemies[0];
        }
        
    }

    public string GetEnemy(int index)
    {
        return enemies[index];
    }

    public int GetPoolSize()
    {
        return enemies.Count;
    }
}
