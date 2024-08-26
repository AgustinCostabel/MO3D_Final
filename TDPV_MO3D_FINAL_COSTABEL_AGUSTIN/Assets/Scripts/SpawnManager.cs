using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private float spawnTimerMax;
    [SerializeField] private float spawnTimerBossMax;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject minotaur;
    [SerializeField] private GameObject crusader;
    [SerializeField] private GameObject bear;

    private float spawnTimer;
    private float spawnTimerBoss;
    private bool stopSpawn = false;
    private GameObject spawnMonster;

    void Start()
    {
        Player.Instance.OnDeathPlayer += Instance_OnDeathPlayer;
        GameManager.Instance.OnNoon += Instance_OnNoon;
        GameManager.Instance.OnNight += Instance_OnNight;
        GameManager.Instance.OnSunrise += Instance_OnSunrise;
        GameManager.Instance.OnDarkNight += Instance_OnDarkNight;

        spawnTimer = spawnTimerMax;
        spawnTimerBoss = spawnTimerBossMax;
        spawnMonster = skeleton;
    }

    private void Instance_OnSunrise(object sender, System.EventArgs e) {
        spawnMonster = skeleton;
    }

    private void Instance_OnNoon(object sender, System.EventArgs e) {
        spawnMonster = minotaur;
        spawnTimerMax = (float)2.5;
    }

    private void Instance_OnNight(object sender, System.EventArgs e) {
        spawnMonster = crusader;
        spawnTimerMax = (float)1.5;
    }

    private void Instance_OnDarkNight(object sender, System.EventArgs e) {
        spawnTimerMax = (float)1.0;
    }

    private void Instance_OnDeathPlayer(object sender, System.EventArgs e) {
        stopSpawn = true;
    }

    void Update() {
        stopSpawn = GameManager.Instance.GetTimer();

        if (!stopSpawn) {
            Spawn();
        }
    }

    public void Spawn() {
        spawnTimer -= Time.deltaTime;
        spawnTimerBoss -= Time.deltaTime;

        if (spawnTimer < 0) {
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, 0, randomDirection.y) * spawnRadius;
            Instantiate(spawnMonster, spawnPosition, Quaternion.identity);
            spawnTimer = spawnTimerMax;
        }

        if(spawnTimerBoss < 0) {
            spawnTimerBoss = spawnTimerBossMax;
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, 0, randomDirection.y) * spawnRadius;
            Instantiate(bear, spawnPosition, Quaternion.identity);
        }
    }
}
