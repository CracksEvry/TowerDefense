using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        if (EconomyManager.I == null || moneyText == null) return;
        moneyText.text = $"$ {EconomyManager.I.PlayerMoney}";
    }
}