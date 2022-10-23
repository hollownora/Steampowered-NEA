using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    // Start is called before the first frame update
    protected int hitPoints;
    public int hitPointsCap;
    public int defense;
    public string enemyName;
    public int battleIndex;
    public int attack;
    // used to load the object from a path in the resources directory
    public string prefabAssetPath;
    // list of encounter captions for this enemy
    public string[] battlecaptions;

    void Start()
    {
        hitPointsCap = hitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damage)
    {
        hitPoints -= damage;
    }

    public int GetHitPoints()
    {
        return hitPoints;
    }
}
