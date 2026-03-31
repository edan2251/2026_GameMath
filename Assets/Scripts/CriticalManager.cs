using UnityEngine;

public class CriticalManager : MonoBehaviour
{
    public int totalHits = 0;
    public int criticalHits = 0;
    public float targetRate = 0.1f;
    
    public bool RollCritical()
    {
        totalHits++;
        float currentRate = 0f;
        if(criticalHits > 0)
        {
            currentRate = (float)criticalHits / totalHits;
        }

        if(currentRate < targetRate && (float)(criticalHits + 1) / totalHits <= targetRate)
        {
            Debug.Log("Critical Hit!, (Forced)");
            criticalHits++;
            return true;
        }

        if(currentRate > targetRate && (float)criticalHits / totalHits >= targetRate)
        {
            Debug.Log("Normal Hit, (Forced)");
            return false;
        }

        if(Random.value < targetRate)
        {
            Debug.Log("Critical Hit, Base");
            criticalHits++;
            return true;
        }

        Debug.Log("Normal Hit, Base");
        return false;
    }

    public void SimulateCritical()
    {
        RollCritical();
        Debug.Log("TotalHits: " + totalHits);
        Debug.Log("CriticalHits: " + criticalHits);
        Debug.Log("Current Critical Rate: " + (float)criticalHits / totalHits);
    }
}
