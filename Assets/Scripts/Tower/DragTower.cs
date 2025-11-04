using System.Collections.Generic;
using UnityEngine;

public class DragTower : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask groundMask;

    [Header("Towers")]
    [SerializeField] private List<GameObject> towerList = new List<GameObject>();

    private GameObject currentTower;
    private Collider[] cachedColliders;
    private int pendingCost;
    private bool hasValidPos;
    private Vector3 lastValidPos;

    private void Awake() { if (cam == null) cam = Camera.main; }

    private void Update()
    {
        if (currentTower == null) return;

        MoveTowerToMouse();

        if (Input.GetMouseButtonDown(0)) TryPlace();
        if (Input.GetMouseButtonDown(1)) CancelDrag();
    }

    public void StartDraggingTower(int selectedTower)
    {
        if (selectedTower < 0 || selectedTower >= towerList.Count) { Debug.LogError("Tower index out of range."); return; }
        var prefab = towerList[selectedTower];
        if (!prefab) { Debug.LogError("Tower prefab is null."); return; }

        var tc = prefab.GetComponent<TowerCost>();
        pendingCost = tc ? Mathf.Max(0, tc.cost) : 0;

        currentTower = Instantiate(prefab);
        cachedColliders = currentTower.GetComponentsInChildren<Collider>(true);
        ToggleColliders(false);
        hasValidPos = false;
        MoveTowerToMouse();
    }

    private void MoveTowerToMouse()
    {
        if (cam == null) cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 2000f, groundMask, QueryTriggerInteraction.Ignore))
        {
            currentTower.transform.position = hit.point;
            lastValidPos = hit.point;
            hasValidPos = true;
        }
        else hasValidPos = false;
    }

    private void TryPlace()
    {
        if (!hasValidPos) { Debug.Log("Geen geldige grondpositie."); return; }
        if (EconomyManager.I == null) { Debug.LogError("EconomyManager ontbreekt."); return; }

        if (!EconomyManager.I.Spend(pendingCost))
        {
            Debug.Log($"Onvoldoende geld. Nodig: {pendingCost}, Je hebt: {EconomyManager.I.PlayerMoney}");
            return;
        }

        ToggleColliders(true);
        currentTower.transform.position = lastValidPos;
        currentTower = null; cachedColliders = null; pendingCost = 0; hasValidPos = false;
    }

    private void CancelDrag()
    {
        if (currentTower) Destroy(currentTower);
        currentTower = null; cachedColliders = null; pendingCost = 0; hasValidPos = false;
    }

    private void ToggleColliders(bool enabled)
    {
        if (cachedColliders == null) return;
        foreach (var c in cachedColliders) if (c) c.enabled = enabled;
    }
}
