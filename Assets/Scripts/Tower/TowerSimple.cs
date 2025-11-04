using UnityEngine;

public class TowerSimple : MonoBehaviour
{
    [Header("Floats")]
    public float range = 6f;           // shoot distance
    public float fireRate = 1.0f;      // shots per second
    public float damage = 20f;         // per hit
    public float projectileSpeed = 12f;

    float cooldown;

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown > 0f) return;

        Transform target = FindClosestEnemy();
        if (!target) return;

        cooldown = 1f / Mathf.Max(0.01f, fireRate);

        // spawn one “projectile” (can be invisible)
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = Vector3.one * 0.2f;
        Destroy(go.GetComponent<Collider>()); // no physics collisions
        var p = go.AddComponent<TowerProjectile>();
        p.Init(target, damage, projectileSpeed);
        go.transform.position = transform.position + Vector3.up * 0.5f;
    }

    Transform FindClosestEnemy()
    {
        // Very simple: search by tag. Tag your enemy prefab as "Enemy".
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform best = null;
        float bestSqr = range * range;

        foreach (var e in enemies)
        {
            if (!e) continue;
            float d2 = (e.transform.position - transform.position).sqrMagnitude;
            if (d2 <= bestSqr) { bestSqr = d2; best = e.transform; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
