using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class EnemyDeathGacha : MonoBehaviour
{
    public TextMeshProUGUI[] gachaRate = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] itemText = new TextMeshProUGUI[4];
    public TextMeshProUGUI gachaResultText;

    public float NormalDropRate = 50f;
    public float RareDropRate = 30f;
    public float EpicDropRate = 15f;
    public float LegendaryDropRate = 5f;

    public int NormalItemCount = 0;
    public int RareItemCount = 0;
    public int EpicItemCount = 0;
    public int LegendaryItemCount = 0;

    public void Start()
    {
        ItemCountTextUpdate();
        GachaRateTextUpdate();
    }

    public void SimulateGachaSingle()   ////////////////////////////////////////
    {
        gachaResultText.text = GachaCaculator() + "획득!";
        GachaResult();
    }

    void GachaResult()
    {
        string result = GachaCaculator();
        if (result == "전설 아이템")
        {
            NormalDropRate = 50f;
            RareDropRate = 30f;
            EpicDropRate = 15f;
            LegendaryDropRate = 5f;

            LegendaryItemCount++;
            ItemCountTextUpdate();
        }
        else
        {
            LegendaryDropRate += 1.5f;
            NormalDropRate -= 0.5f;
            RareDropRate -= 0.5f;
            EpicDropRate -= 0.5f;

            if (result == "일반 아이템") { NormalItemCount++; ItemCountTextUpdate(); }
            else if (result == "고급 아이템") { RareItemCount++; ItemCountTextUpdate(); }
            else if (result == "희귀 아이템") { EpicItemCount++; ItemCountTextUpdate(); }
        }
        GachaRateTextUpdate();
    }

    void ItemCountTextUpdate()
    {
        itemText[0].text = $"일반 아이템: {NormalItemCount}개";
        itemText[1].text = $"고급 아이템: {RareItemCount}개";
        itemText[2].text = $"희귀 아이템: {EpicItemCount}개";
        itemText[3].text = $"전설 아이템: {LegendaryItemCount}개";
    }

    void GachaRateTextUpdate()
    {
        gachaRate[0].text = $"일반 아이템 드롭률: {NormalDropRate}%";
        gachaRate[1].text = $"고급 아이템 드롭률: {RareDropRate}%";
        gachaRate[2].text = $"희귀 아이템 드롭률: {EpicDropRate}%";
        gachaRate[3].text = $"전설 아이템 드롭률: {LegendaryDropRate}%";
    }

    string GachaCaculator()
    {
        Dictionary<string, float> gradeTable = new()
            {
            {"일반 아이템", NormalDropRate },
            {"고급 아이템", RareDropRate },
            {"희귀 아이템", EpicDropRate },
            {"전설 아이템", LegendaryDropRate }
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
