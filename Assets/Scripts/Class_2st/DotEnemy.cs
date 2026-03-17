using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class DotEnemy : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;
    public float currentViewAngle;

    public float viewDistance = 5f;
    public float currentViewDistance;

    public Material[] enemyColor;

    public Vector3 currentScale;


    void Start()
    {
        this.GetComponent<MeshRenderer>().material = enemyColor[0];

        currentScale = transform.localScale;

        float currentViewDistance = viewDistance;
        float currentViewAngle = viewAngle;
    }

    void Update()
    {

        Vector3 A = player.transform.position;
        Vector3 B = transform.position;

        Vector3 fowardVector = new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);

        float sqrMagnitude = fowardVector.x * fowardVector.x + fowardVector.y * fowardVector.y + fowardVector.z * fowardVector.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);


        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        //float dot = Vector3.Dot(forward, toPlayer);
        float dot = forward.x * toPlayer.x + forward.y * toPlayer.y + forward.z * toPlayer.z;
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;


        if(angle < currentViewAngle / 2 && magnitude <= currentViewDistance)
        {
            Debug.Log("플레이어가 시야 안에 있음");
            this.GetComponent<MeshRenderer>().material = enemyColor[1];
            transform.localScale = currentScale * 2;
            currentViewDistance = viewDistance * 3;
            currentViewAngle = viewAngle * 2;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = enemyColor[0];
            transform.localScale = currentScale;
            currentViewDistance = viewDistance;
            currentViewAngle = viewAngle;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * currentViewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -currentViewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, currentViewAngle / 2, 0) * forward;

        Gizmos.DrawRay (transform.position, leftBoundary);
        Gizmos.DrawRay (transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);

    }
}
