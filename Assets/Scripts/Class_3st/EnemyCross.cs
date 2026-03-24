using UnityEngine;

public class EnemyCross : MonoBehaviour
{
    public Transform target;

    public float rayLength = 2.0f;
    public Color gizmoColor = Color.blue;

    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        Vector3 crossProduct = GetCrossProduct(forward, dirToTarget);

        if (crossProduct.y > 0.1f)
        {
            Debug.Log("플레이어가 오른쪽에 있습니다.");
        }
        else if (crossProduct.y < -0.1f)
        {
            Debug.Log("플레이어가 왼쪽에 있습니다.");
        }
    }

    private void OnDrawGizmos()
    {
        DrawForwardRay();
    }
    private void DrawForwardRay()
    {
        Vector3 startPos = transform.position;
        Vector3 forwardDir = transform.forward * rayLength;
        Vector3 endPos = startPos + forwardDir;

        Gizmos.color = gizmoColor;
        Gizmos.DrawRay(startPos, forwardDir);
    }

    Vector3 GetCrossProduct(Vector3 a, Vector3 b)
    {
        float crossX = a.y * b.z - a.z * b.y;
        float crossY = a.z * b.x - a.x * b.z;
        float crossZ = a.x * b.y - a.y * b.x;
        return new Vector3(crossX, crossY, crossZ);
    }
}
