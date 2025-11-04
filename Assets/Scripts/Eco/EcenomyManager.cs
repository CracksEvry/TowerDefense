using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager I { get; private set; }

    [SerializeField] private int startingMoney = 200;
    [SerializeField] private int moneyPerWave = 100;
    public int PlayerMoney { get; private set; }

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        PlayerMoney = startingMoney;
    }

    public bool CanAfford(int cost) => PlayerMoney >= cost;

    public bool Spend(int cost)
    {
        if (!CanAfford(cost)) return false;
        PlayerMoney -= cost;
        return true;
    }

    public void Add(int amount) { if (amount > 0) PlayerMoney += amount; }

    // call bij start van elke wave
    public void OnWaveStarted(int waveIndex) { Add(moneyPerWave); }
}