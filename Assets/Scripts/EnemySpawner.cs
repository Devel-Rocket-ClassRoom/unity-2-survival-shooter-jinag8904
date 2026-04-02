using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<Enemy> enemyPrefabs;
    public List<EnemyData> enemyDatas;

    public float spawnInterval = 5f;
    public float lastSpawnTime;

    public GameObject player;
    public ParticleSystem hitEffect;
    public GameManager gameManager;

    void Update()
    {
        if (Time.time > lastSpawnTime + spawnInterval)
        {
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Count);

        var newEnemy = Instantiate(enemyPrefabs[index], spawnPoints[index].transform.position, spawnPoints[index].transform.rotation);
        newEnemy.Health = enemyDatas[index].health;
        newEnemy.AttackPower = enemyDatas[index].attackPower;
        newEnemy.target = player;
        newEnemy.agent.speed = enemyDatas[index].speed;
        newEnemy.HitEffect = hitEffect;
        newEnemy.addScore = enemyDatas[index].addScore;
        newEnemy.gameManager = gameManager;

        lastSpawnTime = Time.time;

        Debug.Log($"스폰: {index}번 몬스터");
    }
}