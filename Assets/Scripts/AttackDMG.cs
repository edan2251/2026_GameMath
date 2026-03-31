using UnityEngine;

public class AttackDMG : MonoBehaviour
{
    public CriticalAttack critAtk;
    public Enemy enemy;

    public float baseDamage = 30f;
    public float criticalMultiplier = 2.0f;
    public float currentDamage;

    public void OnAttack()
    {
        if(critAtk.RollCritical())
        {
            currentDamage = baseDamage * criticalMultiplier;
            Debug.Log("Critical Hit! Damage: " + currentDamage);
        }
        else
        {
            currentDamage = baseDamage;
            Debug.Log("Normal Hit. Damage: " + currentDamage);
        }
        enemy.TakeDamage(currentDamage);

    }
}
