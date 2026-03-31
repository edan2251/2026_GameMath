using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyDeathGacha deathGacha;
    public float maxHealth = 300f;
    public float currenthealth;
    public TextMeshProUGUI healthText;

    public Image enemyImage;

    public Sprite[] changeimages;

    private void Start()
    {
        currenthealth = maxHealth;
        healthText.text = $"체력 : {currenthealth}/{maxHealth}";
    }

    public void TakeDamage(float damage)
    {
        currenthealth -= damage;

        if(currenthealth <= 0)
        {
            Die();
        }

        healthText.text = $"체력 : {currenthealth}/{maxHealth}";
    }

    void Die()
    {
        deathGacha.SimulateGachaSingle();

        enemyImage.sprite = changeimages[Random.Range(0, changeimages.Length)];
        currenthealth = maxHealth;
        healthText.text = $"체력 : {currenthealth}/{maxHealth}";
    }
}
