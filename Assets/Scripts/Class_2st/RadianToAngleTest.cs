using UnityEngine;

public class RadianToAngleTest : MonoBehaviour
{
    void Start()
    {
        AngleToRadianTest();
    }


    void AngleToRadianTest()
    {
        float degrees = 45f;
        //float radians = degrees * Mathf.Deg2Rad;
        float radians = degrees * (Mathf.PI/180);
        Debug.Log("45도 -> 라디안 : " + radians);

        float radianValue = Mathf.PI / 3;
        //float degreeValue = radianValue * Mathf.Rad2Deg;
        float degreeValue = radianValue * (180/Mathf.PI);
        Debug.Log("파이/3 라디안 -> 도 변환 : " + degreeValue);

    }
}
