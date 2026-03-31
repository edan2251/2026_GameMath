using UnityEngine;
using TMPro;

public class RandomTest : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UnityRandomTest();
        //SystemSeedRandomTest();
        //UnitySeedRandomTest();
        //RandomDiceTest();
    }
    
    private void UnityRandomTest()
    {
        float chance = Random.value;
        int dice = Random.Range(1, 7);

        System.Random sysRand = new System.Random();
        int number = sysRand.Next(1, 7);

        Debug.Log("Unity Random (Random.value) " + chance);
        Debug.Log("Unity Random (Random.Range) " + dice);

        Debug.Log("System Random (sysRand.Next) " + number);
    }

    private void SystemSeedRandomTest()
    {
        System.Random rnd = new System.Random(1234);
        for(int i = 0; i < 5; i++)
        {
            Debug.Log("System Seed Random " + rnd.Next(1, 7));
        }
    }

    private void UnitySeedRandomTest()
    {
        Random.InitState(1234);
        for(int i = 0; i < 5; i++)
        {
            Debug.Log("Unity Seed Random " + Random.Range(1, 7));
        }
    }

    int[] counts = new int[6];
    public int trials = 100;

    public TextMeshProUGUI[] labels = new TextMeshProUGUI[6];

    public void RandomDiceTest()
    {
        counts = new int[6];
        for (int i = 0; i < trials; i++)
        {
            int result = Random.Range(1, 7);
            counts[result -1]++;
        }

        for(int i = 0; i < counts.Length; i++)
        {
            float percentage = (float)counts[i] / trials * 100f;
            string results = $"{i + 1} : {counts[i]}count ({percentage:F2}%)";
            labels[i].text = results;
        }

    }

}
