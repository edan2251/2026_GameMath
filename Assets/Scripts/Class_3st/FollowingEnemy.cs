using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class FollowingEnemy : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;
    public float rotationAngle = 30f;

    public float viewDistance = 5f;

    public Material[] enemyColor;

    public bool isPlayerInSight = false;



    void Start()
    {
        this.GetComponent<MeshRenderer>().material = enemyColor[0];
    }

    void Update()
    {
        transform.Rotate(0, rotationAngle * Time.deltaTime, 0);

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


        if(angle < viewAngle / 2 && magnitude <= viewDistance)
        {
            Debug.Log("플레이어가 시야 안에 있음");
            this.GetComponent<MeshRenderer>().material = enemyColor[1];
            isPlayerInSight = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = enemyColor[0];
            isPlayerInSight = false;
        }

        if(isPlayerInSight == true)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay (transform.position, leftBoundary);
        Gizmos.DrawRay (transform.position, rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);

    }
}
