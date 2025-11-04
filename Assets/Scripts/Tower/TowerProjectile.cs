using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    Transform target;
    float dmg;
    float speed;

    public void Init(Transform t, float damage, float projectileSpeed)
    {
        target = t; dmg = damage; speed = projectileSpeed;
    }

    void Update()
    {
        if (!target) { Destroy(gameObject); return; }

        Vector3 to = target.position - transform.position;
        float step = speed * Time.deltaTime;

        if (to.magnitude <= step)
        {
            var hp = target.GetComponent<EnemyHP>();
            if (hp) hp.TakeDamage(dmg);
            Destroy(gameObject);
        }
        else
        {
            transform.position += to.normalized * step;
        }
    }
}
