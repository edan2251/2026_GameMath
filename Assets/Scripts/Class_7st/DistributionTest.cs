using UnityEngine;

public class DistributionTest : MonoBehaviour
{
    public float testLamda = 3f;

    int PoissonDistribution(float lamda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lamda);        //- Áß¿ä
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }



    void Start()
    {
        for (int i = 0; i < 10; i ++)
        {
            int count = PoissonDistribution(testLamda);
            Debug.Log($"Minute {i + 1}: {count} events");
        }
    }

}
