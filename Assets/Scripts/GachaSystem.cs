using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class GachaSystem : MonoBehaviour
{
    public TextMeshProUGUI gachaResultText;
    public int GachaTimes = 0;

    public void GachaGOGOGO()
    {
        GachaTimes++;
        if(GachaTimes <= 9)
        {
            SimulateGachaSingle();
        }
        else
        {
            SimulateGachaTenth();
            GachaTimes = 0;
        }
    }

    public void SimulateGachaSingle()
    {
        string FinalResult = $"{GachaTimes}¹ø¤Š °¡Ã­: " + UpgradeSimulate();
        gachaResultText.text = FinalResult;
    }

    public void SimulateGachaTenth()
    {
        float r2 = Random.value;
        string result2 = string.Empty;
        if (r2 < 2f / 3f) result2 = "A";
        else result2 = "S";

        string tenTimesResult = $"{GachaTimes}¹ø¤Š °¡Ã­: " + result2;
        gachaResultText.text = tenTimesResult;
    }

    public void SimulateGachaTenTime()
    {
        List<string> results = new List<string>();
        for (int i = 0; i < 9; i++)
        {
            results.Add(UpgradeSimulate());
        }

        float r2 = Random.value;
        string result2 = string.Empty;
        if (r2 < 2f / 3f) result2 = "A";
        else result2 = "S";
        results.Add(result2);

        string tenTimesResult = "Gacha 10 Times Result: " + string.Join(", ", results);
        gachaResultText.text = tenTimesResult;
    }

   string Simulate()
    {
        float r = Random.value;
        string result = string.Empty;

        if (r < 0.4f) result = "C";
        else if (r < 0.7f) result = "B";
        else if (r < 0.9f) result = "A";
        else result = "S";

        return result;
    }

    string UpgradeSimulate()
    {
        Dictionary<string, float> gradeTable = new()
            {
            {"C", 40f },
            {"B", 30f },
            {"A", 20f },
            {"S", 10f }
            };
        float totalWeight = gradeTable.Values.Sum();
        float roll = Random.Range(0f, totalWeight);
        float accumulator = 0f;
        string result = string.Empty;

        foreach (var pair in gradeTable)
        {
            accumulator += pair.Value;
            if (roll <= accumulator)
            {
                result = pair.Key;
                break;
            }
        }
        return result;
    }
}
