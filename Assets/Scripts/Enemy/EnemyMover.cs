using System.Collections;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [Header("Path (runtime gezet)")]
    public Transform[] turns;

    [Header("Movement")]
    [Min(0f)] public float speed = 3.5f;
    [Min(0f)] public float turnSpeed = 720f;
    public float trackY = 0.5f;
    public float yawOffset = 0f;
    public float endDespawnDelay = 0f;

    [Header("Goal / Base")]
    [Min(0)] public int damageOnGoal = 2;
    [SerializeField] private Basehealth baseHealth;

    int _i;
    Rigidbody _rb;

    public void Init(Transform[] pathTurns, Vector3 spawnPos, Basehealth baseHp = null)
    {
        turns = pathTurns;
        if (baseHp != null) baseHealth = baseHp;
        transform.position = new Vector3(spawnPos.x, trackY, spawnPos.z);
        _i = 0; FaceNext();
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezePositionY |
                              RigidbodyConstraints.FreezeRotationX |
                              RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void Start()
    {
        var p = transform.position; p.y = trackY; transform.position = p;
        FaceNext();
    }

    void Update()
    {
        if (turns == null || turns.Length == 0 || _i >= turns.Length) return;

        Vector3 target = turns[_i].position; target.y = trackY;
        Vector3 to = target - transform.position; to.y = 0f;

        float step = speed * Time.deltaTime;

        if (to.magnitude <= step)
        {
            transform.position = target;
            _i++;
            if (_i >= turns.Length)
            {
                if (baseHealth != null) baseHealth.RemoveHealth(damageOnGoal);
                EnemyTracker.RegisterDeath();
                if (endDespawnDelay > 0f) StartCoroutine(DespawnAfter(endDespawnDelay));
                else Destroy(gameObject);
                return;
            }
            FaceNext();
        }
        else
        {
            transform.position += to.normalized * step;
            if (to.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(to) * Quaternion.Euler(0f, yawOffset, 0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, look, turnSpeed * Time.deltaTime);
            }
        }

        var p = transform.position; p.y = trackY; transform.position = p;
    }

    void FaceNext()
    {
        if (turns == null || _i >= turns.Length) return;
        Vector3 dir = turns[_i].position - transform.position; dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, yawOffset, 0f);
    }

    IEnumerator DespawnAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
