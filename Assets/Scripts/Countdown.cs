using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdown;
    private int countdownIndex;

    private async void Start()
    {
        countdownIndex = 3;
        UpdateUI();
        await Threading();
    }

    private async Task Threading()
    {
        await Task.Delay(1000);
        countdownIndex--;
        UpdateUI();
        if (countdownIndex > 0)
        {
            await Threading();
        }
        else
        {
            countdown.text = string.Empty;
        }
    }

    private void UpdateUI()
    {
        countdown.text = countdownIndex.ToString();
    }
}
