using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.SceneManagement;

public class FollowingEnemy : MonoBehaviour
{
    public Transform player;
    public ParrySystem parrySystem;
    public float viewAngle = 60f;
    public float rotationAngle = 30f;

    public float viewDistance = 5f;
    public float parryAbleDistance = 5f;
    public float playerKillDistance = 3f;

    public Material[] enemyColor;

    public bool isPlayerInSight = false;
    public bool isAddedToSystem = false;

    public Transform enemy;



    void Start()
    {
        enemy = this.transform;
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
            //Debug.Log("플레이어가 시야 안에 있음");
            this.GetComponent<MeshRenderer>().material = enemyColor[1];
            isPlayerInSight = true;

            if (isAddedToSystem == false)
            {
                parrySystem.AddEnemy(this);
                isAddedToSystem = true;
            }

            if (playerKillDistance <= magnitude && magnitude <= parryAbleDistance)
            {
                parrySystem.canParry = true;
                //Debug.Log("패링 가능");
            }
            else if(magnitude <= playerKillDistance && magnitude <= parryAbleDistance)
            {
                //Debug.Log("플레이어 사망");
                parrySystem.canParry = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                //Debug.Log("패링 불가능");
                parrySystem.canParry = false;
            }    
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = enemyColor[0];
            isPlayerInSight = false;

            if (isAddedToSystem == true)
            {
                parrySystem.RemoveEnemy(this);
                isAddedToSystem = false;
            }
        }

        if(isPlayerInSight == true)
        {
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            
            transform.Translate(Vector3.forward * Time.deltaTime * 2f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward * viewDistance;

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay (transform.position, leftBoundary);
        Gizmos.DrawRay (transform.position, rightBoundary);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, parryAbleDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerKillDistance);

    }
}
