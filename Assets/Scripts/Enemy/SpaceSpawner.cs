using System.Collections;
using UnityEngine;

public class SpaceSpawner : MonoBehaviour
{
    [Header("Assign once")]
    public EnemyMover enemyPrefab; // your enemy prefab with EnemyMoverSuperSimple
    public Transform[] turns;                 // drag your path turns here in order
    public Transform spawnPoint;              // empty placed where you want spawns

    [Header("Spawner Floats")]
    public float spawnPerPress = 1f;          // how many per SPACE (rounded)
    public float spawnGap = 0f;               // seconds between each spawn in the same press
    public float spawnY = 0.5f;               // forces enemy trackY (height)

    bool busy;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !busy)
            StartCoroutine(SpawnBurst());
    }

    IEnumerator SpawnBurst()
    {
        if (!enemyPrefab || spawnPoint == null || turns == null || turns.Length == 0)
        {
            Debug.LogWarning("Setup missing: enemyPrefab / spawnPoint / turns.");
            yield break;
        }

        busy = true;

        int count = Mathf.Max(1, Mathf.RoundToInt(spawnPerPress));
        for (int n = 0; n < count; n++)
        {
            var e = Instantiate(enemyPrefab);
            // push the float settings into the enemy so you only tweak floats
            e.speed = e.speed;              // keep prefab default unless you change on prefab
            e.turnSpeed = e.turnSpeed;      // same here (tweak on prefab if needed)
            e.trackY = spawnY;

            e.Init(turns, spawnPoint.position);

            if (spawnGap > 0f && n + 1 < count)
                yield return new WaitForSeconds(spawnGap);
        }

        busy = false;
    }

    void OnDrawGizmos()
    {
        if (!spawnPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawnPoint.position, 0.3f);
    }
}
