using UnityEngine;

[RequireComponent(typeof(Collider))]   // voor OnMouseDown -> je tower moet een collider hebben
public class TowerRangeIndicator : MonoBehaviour
{
    [Header("Koppeling (optioneel)")]
    public TowerSimple tower;          // laat leeg = geen koppeling
    public float radius = 6f;          // gebruikt als er geen TowerSimple is

    [Header("Weergave")]
    public int segments = 64;
    public float lineWidth = 0.05f;
    public Color color = new Color(1f, 1f, 0f, 0.6f); // geel met beetje alpha
    public float yOffset = 0.02f;      // net boven de grond
    public bool startHidden = true;    // begin onzichtbaar

    LineRenderer lr;
    float lastRadius = -1f;

    void Awake()
    {
        // Koppel TowerSimple automatisch als het op hetzelfde object zit
        if (!tower) tower = GetComponent<TowerSimple>();

        // LineRenderer aanmaken
        lr = gameObject.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.loop = true;
        lr.positionCount = segments;
        lr.startWidth = lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default")); // werkt in URP/Built-in
        lr.startColor = lr.endColor = color;
        lr.enabled = !startHidden;

        Rebuild();
    }

    void Update()
    {
        float r = tower ? tower.range : radius;
        if (!Mathf.Approximately(r, lastRadius))
            Rebuild();
    }

    void Rebuild()
    {
        float r = tower ? tower.range : radius;
        lastRadius = r;

        // posities voor perfecte cirkel
        for (int i = 0; i < segments; i++)
        {
            float t = (i / (float)segments) * Mathf.PI * 2f;
            float x = Mathf.Cos(t) * r;
            float z = Mathf.Sin(t) * r;
            lr.SetPosition(i, new Vector3(x, yOffset, z));
        }
    }

    // Klik om zichtbaar/onzichtbaar te togglen
    void OnMouseDown()
    {
        lr.enabled = !lr.enabled;
    }

    // Handige functie als je ‘m elders wilt aan/uit zetten
    public void SetVisible(bool v) { if (lr) lr.enabled = v; }
}
