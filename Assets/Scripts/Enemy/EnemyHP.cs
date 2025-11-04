using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 3f;
    public float Current => _hp;

    float _hp;

    void Awake() { _hp = maxHealth; }

    public void SetHealth(float v)
    {
        maxHealth = v;
        _hp = v;
    }

    public void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if (_hp <= 0f)
        {
            _hp = 0f;
            Die();
        }
    }

    void Die()
    {
        EnemyTracker.RegisterDeath();
        Destroy(gameObject);
    }
}