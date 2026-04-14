using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnBasedGame : MonoBehaviour
{
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float normalAttackResult;
    [SerializeField] float criticalAttackResult;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;
    [SerializeField] int enemyCount;
    [SerializeField] int cutEnemyCount = 0;

    [SerializeField] float maxDamage = 0f;
    [SerializeField] float minDamage = 0f;

    [SerializeField] int goldCount = 0;
    [SerializeField] int potionCount = 0;
    [SerializeField] int normalWeaponCount = 0;
    [SerializeField] int rareWeaponCount = 0;
    [SerializeField] int normalArmorCount = 0;
    [SerializeField] int rareArmorCount = 0;

    public float rareItemDropRate = 0.2f;

    public TextMeshProUGUI TurnTimes;
    public TextMeshProUGUI EnemyCount;
    public TextMeshProUGUI CutEnemyCount;
    public TextMeshProUGUI AttackResult;
    public TextMeshProUGUI CritResult;
    public TextMeshProUGUI MaxDMG;
    public TextMeshProUGUI MinDMG;
    public TextMeshProUGUI RareItemDropChance;
    public TextMeshProUGUI PosionCount;
    public TextMeshProUGUI GoldCount;
    public TextMeshProUGUI NormalWeaponCount;
    public TextMeshProUGUI RareWeaponCount;
    public TextMeshProUGUI NormalArmorCount;
    public TextMeshProUGUI RareArmorCount;
    public TextMeshProUGUI RareItemDropped;

    public Button button;

    int turn = 0;
    bool rareItemObtained = false;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    public void ResetData()
    {
        enemyCount = 0;
        normalAttackResult = 0f;
        criticalAttackResult = 0f;
        maxDamage = 0f;
        minDamage = 0f;
    }

    public void TextUpdate()
    {
        TurnTimes.text = $"총 진행 턴 수 : {turn}";
        EnemyCount.text = $"발생한 적 수 : {enemyCount}";
        CutEnemyCount.text = $"처치한 적 수 : {cutEnemyCount}";
        AttackResult.text = $"공격 명중 결과 : {normalAttackResult}%";
        CritResult.text = $"발생한 치명타율 결과 : {criticalAttackResult}%";
        MaxDMG.text = $"최대 데미지 : {maxDamage}";
        MinDMG.text = $"최소 데미지 : {minDamage}";
        RareItemDropChance.text = $"레어 아이템 드롭 확률 : {rareItemDropRate * 100}%";
        PosionCount.text = $"포션 : {potionCount}개";
        GoldCount.text = $"골드 : {goldCount}개";
        NormalWeaponCount.text = $"무기 - 일반 : {normalWeaponCount}개";
        RareWeaponCount.text = $"무기 - 레어 : {rareWeaponCount}개";
        NormalArmorCount.text = $"방어구 - 일반 : {normalArmorCount}개";
        RareArmorCount.text = $"방어구 - 레어: {rareArmorCount}개";
    }

    private void Start()
    {
        turn = 0;
        rareItemObtained = false;
    }

    public void StartSimulation()
    {
        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복하는 구조
        rareItemObtained = false;
        turn = 0;
        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;
            rareItemDropRate += 0.05f;
        }

        Debug.Log($"레어 아이템 {turn} 턴에 획득");
    }

    public void SimulateTurn()
    {
        if ( rareItemObtained )
        {
            RareItemDropped.text = $"레어 아이템 획득! (총 {turn} 턴 소요)";
            button.interactable = false;
            return;
        }
        else
        {
            turn++;
            Debug.Log($"--- Turn {turn} ---");
            ResetData();
            // 푸아송 샘플링: 적 등장 수
            enemyCount = SamplePoisson(poissonLambda);
            Debug.Log($"적 등장 : {enemyCount}");

            for (int i = 0; i < enemyCount; i++)
            {
                // 이항 샘플링: 명중 횟수
                int hits = SampleBinomial(maxHitsPerTurn, hitRate);
                float totalDamage = 0f;

                for (int j = 0; j < hits; j++)
                {
                    float damage = SampleNormal(meanDamage, stdDevDamage);
                    float chance = Random.value;
                    // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                    if (chance < critChance)
                    {
                        damage *= critDamageRate;
                        criticalAttackResult = chance * 100;
                        Debug.Log($" 크리티컬 hit! {damage:F1}");
                    }
                    else
                    {
                        normalAttackResult = chance * 100;
                        Debug.Log($" 일반 hit! {damage:F1}");
                    }

                    totalDamage += damage;
                    minDamage = damage;
                    maxDamage = Mathf.Max(maxDamage, damage);
                    minDamage = Mathf.Min(minDamage, damage);
                }

                if (totalDamage >= enemyHP)
                {
                    Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");
                    cutEnemyCount += 1;
                    // 균등 분포 샘플링: 보상 결정
                    string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                    Debug.Log($"보상: {reward}");

                    float dropChance = Random.value;

                    if (reward == "Gold")
                    {
                        goldCount += 1;
                    }
                    else if (reward == "Potion")
                    {
                        potionCount += 1;
                    }
                    else if (reward == "Weapon" && dropChance < rareItemDropRate)
                    {
                        rareItemObtained = true;
                        rareWeaponCount += 1;
                    }
                    else if (reward == "Armor" && dropChance < rareItemDropRate)
                    {
                        rareItemObtained = true;
                        rareArmorCount += 1;
                    }
                    else if (reward == "Weapon")
                    {
                        normalWeaponCount += 1;
                    }
                    else if (reward == "Armor")
                    {
                        normalArmorCount += 1;
                    }
                }
                
            }
            TextUpdate();
            rareItemDropRate += 0.05f;
        }

        // --- 분포 샘플 함수들 ---
        int SamplePoisson(float lambda)
        {
            int k = 0;
            float p = 1f;
            float L = Mathf.Exp(-lambda);
            while (p > L)
            {
                k++;
                p *= Random.value;
            }
            return k - 1;
        }

        int SampleBinomial(int n, float p)
        {
            int success = 0;
            for (int i = 0; i < n; i++)
                if (Random.value < p) success++;
            return success;
        }

        float SampleNormal(float mean, float stdDev)
        {
            float u1 = Random.value;
            float u2 = Random.value;
            float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
            return mean + stdDev * z;
        }
    }

}