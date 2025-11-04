using UnityEngine;

public class TopDownCameraMover : MonoBehaviour
{
    [Header("View")]
    [Tooltip("Tilt angle down from horizontal (e.g., 45 = looking down at 45°).")]
    [Range(0f, 89f)] public float tiltAngle = 45f;
    [Tooltip("Yaw (rotation around Y). 0 = looking along +Z. Adjust to face your platform.")]
    public float yawDegrees = 0f;
    [Tooltip("Fixed height above the platform (kept constant).")]
    public float height = 10f;

    [Header("Movement")]
    [Tooltip("Units per second.")]
    public float moveSpeed = 10f;
    [Tooltip("If true, WASD moves relative to the camera's facing. If false, A/D = X and W/S = Z.")]
    public bool cameraRelative = true;

    [Header("Bounds (optional)")]
    public bool clampToBounds = false;
    public Vector2 minXZ = new Vector2(-50f, -50f);
    public Vector2 maxXZ = new Vector2(50f, 50f);

    float _initialY;

    void Start()
    {
       
        Vector3 p = transform.position;
        _initialY = p.y = height;
        transform.position = p;

        SetRotation();
    }

    void Update()
    {
        
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical");   

        Vector3 move;

        if (cameraRelative)
        {
           
            Vector3 fwd = transform.forward; fwd.y = 0f; fwd.Normalize();
            Vector3 right = transform.right; right.y = 0f; right.Normalize();
            move = (fwd * v + right * h);
        }
        else
        {
            
            move = new Vector3(h, 0f, v);
        }

        if (move.sqrMagnitude > 0f)
        {
            transform.position += move.normalized * moveSpeed * Time.deltaTime;
        }

        
        Vector3 pos = transform.position;
        pos.y = _initialY = height;    
        transform.position = pos;

       
      
        if (clampToBounds)
        {
            pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minXZ.x, maxXZ.x);
            pos.z = Mathf.Clamp(pos.z, minXZ.y, maxXZ.y);
            transform.position = pos;
        }

    }

    void OnValidate()
    {
        
        SetRotation();
    }

    void SetRotation()
    {
        
        transform.rotation = Quaternion.Euler(tiltAngle, yawDegrees, 0f);
    }
}
