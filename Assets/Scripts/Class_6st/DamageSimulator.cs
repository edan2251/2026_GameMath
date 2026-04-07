using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI rangeDisplay;

    public TextMeshProUGUI missDisplay;
    public TextMeshProUGUI strongDisplay;
    public TextMeshProUGUI criticalDisplay;
    public TextMeshProUGUI highDamageDisplay;

    private int missCount = 0, strongCount = 0, critCount = 0;
    private float highDamage;
    private int level = 1;
    private float totalDamage = 0, baseDamage = 20f;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;

    private float randStdNormal;


    void Start()
    {
            
    }

    public void ThousandTimesAttack()
    {
        totalDamage = 0;
        attackCount = 0;

        missCount = 0;
        strongCount = 0;
        critCount = 0;
        highDamage = 0;

        for(int i = 0; i < 1000; i++)
        {
            OnAttack();
        }

        missDisplay.text = string.Format("빗나감: {0}회 ({1:F2}%)", missCount, (float)missCount / attackCount * 100);
        strongDisplay.text = string.Format("약점: {0}회 ({1:F2}%)", strongCount, (float)strongCount / attackCount * 100);
        criticalDisplay.text = string.Format("치명타: {0}회 ({1:F2}%)", critCount, (float)critCount / attackCount * 100);
        highDamageDisplay.text = string.Format("최고 데미지: {0:F1}", highDamage);
    }

    private void ResetData()
    {
        missCount = 0;
        strongCount = 0;
        critCount = 0;

        totalDamage = 0;
        attackCount = 0;
        level = 1;
        baseDamage = 20f;
    }

    public void SetWeapon(int id)
    {
        ResetData();
        if(id == 0)
        {
            SetStats("단검", 0.1f, 0.4f, 1.5f);
        }
        else if(id == 1)
        {
            SetStats("장검", 0.2f, 0.3f, 2.0f);
        }
        else
        {
            SetStats("도끼", 0.3f, 0.2f, 3.0f);
        }

        logDisplay.text = string.Format("{0} 장착!", weaponName);
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUP()
    {
        missCount = 0;
        strongCount = 0;
        critCount = 0;

        totalDamage = 0;
        attackCount = 0;
        level++;
        baseDamage = level * 20f;
        logDisplay.text = string.Format("레벨업! 현재 레벨: {0}", level);
        UpdateUI();
    }

    public void OnAttack()
    {
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDevDamage(baseDamage, sd);

        bool isCrit = Random.value < critRate;
        if(isCrit) critCount++;
        float finalDamage = isCrit ? normalDamage * critMult : normalDamage;

        if (normalDamage <= baseDamage + sd * -2)
        {
            finalDamage = 0;

            attackCount++;
            missCount++;
            totalDamage += finalDamage;

            string missMark = "<color=blue>[빗나감!]</color> ";
            logDisplay.text = string.Format("{0}데미지: {1:F1}", missMark, finalDamage);
        }
        else if (normalDamage >= baseDamage + sd * 2)
        {
            finalDamage *= 2;

            attackCount++;
            strongCount++;
            totalDamage += finalDamage;

            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            logDisplay.text = string.Format("{0}데미지: {1:F1}", critMark, finalDamage);
        }
        else
        {
            attackCount++;
            totalDamage += finalDamage;

            string critMark = isCrit ? "<color=red>[치명타!]</color> " : "";
            logDisplay.text = string.Format("{0}데미지: {1:F1}", critMark, finalDamage);
        }
        
        if(finalDamage > highDamage)
        {
            highDamage = finalDamage;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        statusDisplay.text = string.Format("Level: {0} / 무기: {1}\n기본 데미지: {2} / 치명타: {3}% (x{4})",
            level, weaponName, baseDamage, critRate * 100, critMult);

        rangeDisplay.text = string.Format("예상 일반 데미지 범위 : [{0:F1} ~ {1:F1}]",
            baseDamage - (3 * baseDamage * stdDevMult),
            baseDamage + (3 * baseDamage * stdDevMult));

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = string.Format("누적 데미지: {0:F1}\n공격 횟수: {1}\n평균 DPA: {2:F2}",
            totalDamage, attackCount, dpa);
    }

    private float GetNormalStdDevDamage(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
            Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }   
}
