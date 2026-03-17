using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

[RequireComponent(typeof(LineRenderer))]
public class SolaSystem : MonoBehaviour
{
    public GameObject centerObject;

    private Vector3 center;

    public float radius;
    public float speed = 2f;
    private float currentAngle;
    private float currentRadian;

    public int segments = 50;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GetRadius();
        DrawOrbit();
    }
    
    void Update()
    {
        NewPositioning();
        DrawOrbit();
    }

    void GetRadius()
    {
        //radius = Vector3.Distance(transform.position, center.position);
        center = centerObject.transform.position;

        Vector3 A = new Vector3(center.x, center.y, center.z);
        Vector3 B = transform.localPosition;

        Vector3 distanceVector = new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);

        float sqrMagnitude = distanceVector.x * distanceVector.x + distanceVector.y * distanceVector.y + distanceVector.z * distanceVector.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        radius = magnitude;
    }

    void NewPositioning()
    {
        if (center == null) return;

        center = centerObject.transform.position; //달 <<<< 이녀석 뭐임?

        currentAngle += speed * Time.deltaTime;
        currentRadian = currentAngle * Mathf.Deg2Rad;

        float x = center.x + radius * Mathf.Cos(currentRadian);
        float z = center.z + radius * Mathf.Sin(currentRadian);

        transform.localPosition = new Vector3(x, 0, z);
    }


    //제미나이가 LineRenderer로 궤도 그리는 코드 짜줌
    void DrawOrbit()
    {
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = true; // 월드 좌표 기준

        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = center.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = center.z + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, center.y, z));
            angle += (360f / segments);
        }
    }
}
