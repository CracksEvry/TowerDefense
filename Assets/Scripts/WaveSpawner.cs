using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int enemyCount = 10;
    public float spawnInterval = 0.5f;
    public float enemySpeed = 2f;
    public float enemyHP = 10f;
}

public static class EnemyTracker
{
    public static int Alive { get; private set; }
    public static void Reset()         { Alive = 0; }
    public static void RegisterSpawn() { Alive++; }
    public static void RegisterDeath() { Alive = Mathf.Max(0, Alive - 1); }
}

public class WaveSpawner : MonoBehaviour
{
    [Header("Waves")]
    public List<WaveData> waves = new List<WaveData>();

    [Header("Scene Refs (net als SpaceSpawner)")]
    [SerializeField] private EnemyMover enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] turns;     // WP0, WP1, WP2... in JUISTE volgorde
    [SerializeField] private Basehealth baseHealth; // sleep je Base (met slider) hier in

    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start() { EnemyTracker.Reset(); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && !isSpawning && EnemyTracker.Alive == 0 && currentWave < waves.Count)
            StartWave(currentWave);
    }

    private void StartWave(int waveIndex)
    {
        EconomyManager.I?.OnWaveStarted(waveIndex + 1);
        StartCoroutine(SpawnWave(waves[waveIndex]));
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        if (!enemyPrefab || !spawnPoint || turns == null || turns.Length == 0 || baseHealth == null)
        { Debug.LogError("WaveSpawner: enemyPrefab/spawnPoint/turns/baseHealth niet gevuld."); yield break; }

        isSpawning = true;

        for (int i = 0; i < wave.enemyCount; i++)
        {
            var e = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            var mover = e.GetComponent<EnemyMover>();
            if (mover != null)
            {
                mover.speed = wave.enemySpeed;
                mover.Init(turns, spawnPoint.position, baseHealth); // path + base meegeven
            }

            var hp = e.GetComponent<EnemyHP>();
            if (hp != null) hp.SetHealth(wave.enemyHP);

            EnemyTracker.RegisterSpawn();

            yield return new WaitForSeconds(Mathf.Max(0f, wave.spawnInterval));
        }

        isSpawning = false;
        currentWave++;
    }
}
