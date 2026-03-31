using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CriticalAttack : MonoBehaviour
{
    public int totalHits = 0;
    public int criticalHits = 0;
    public float targetRate = 0.3f;
    public float currentRate = 0f;

    public TextMeshProUGUI[] labels = new TextMeshProUGUI[4];

    public bool RollCritical()
    {
        totalHits++;
        if(criticalHits > 0)
        {
            currentRate = (float)criticalHits / totalHits;
            CriticalGUI();
        }

        if(currentRate < targetRate && (float)(criticalHits + 1) / totalHits <= targetRate)
        {
            criticalHits++;
            CriticalGUI();
            return true;
        }

        if(currentRate > targetRate && (float)criticalHits / totalHits >= targetRate)
        {
            CriticalGUI();
            return false;
        }

        if(Random.value < targetRate)
        {
            criticalHits++;
            CriticalGUI();
            return true;
        }

        CriticalGUI();
        return false;
    }

    public void CriticalGUI()
    {
        labels[0].text = "현재 공격 수 : " + totalHits;
        labels[1].text = "현재 크리티컬 수 : " + criticalHits;
        labels[2].text = "목표 크리티컬 확률 : " + (targetRate * 100f).ToString("F2") + "%";
        labels[3].text = "현재 크리티컬 확률 : " + (currentRate * 100f).ToString("F2") + "%";
    }
}
