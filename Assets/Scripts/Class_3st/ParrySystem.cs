using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParrySystem : MonoBehaviour
{
    public LinkedList<FollowingEnemy> nearbyEnemies = new LinkedList<FollowingEnemy>();
    public GameObject leftArmy;
    public GameObject rightArmy;


    public bool isParryingLeft = false;
    public bool isParryingRight = false;

    public bool canParry = false;

    public void AddEnemy(FollowingEnemy enemy)
    {
        nearbyEnemies.AddLast(enemy);
        Debug.Log($"적 추가됨, 추가된 적 이름 : {enemy.name} / 현재 적 수: {nearbyEnemies.Count}");
    }
    public void RemoveEnemy(FollowingEnemy enemy)
    {
        nearbyEnemies.Remove(enemy);
        Debug.Log($"적 제거됨, 제거된 적 이름 : {enemy.name} / 현재 적 수: {nearbyEnemies.Count}");
    }

    public void OnParryLeft(InputValue value)
    {
        isParryingLeft = value.isPressed;
        //LeftArmy.SetActive(isParryingLeft);
        //내친구 제미나이가 해준 병사 올라오게 하는 코루틴
        if (_leftCoroutine != null) StopCoroutine(_leftCoroutine);
        if (isParryingLeft)
        {
            PrepareArmyPosition(leftArmy.transform);
            leftArmy.SetActive(true);
            _leftCoroutine = StartCoroutine(SummonArmy(leftArmy.transform));
        }
        else
        {
            leftArmy.SetActive(false);
        }
    }

    public void OnParryRight(InputValue value)
    {
        isParryingRight = value.isPressed;
        //RightArmy.SetActive(isParryingRight);
        //내친구 제미나이가 해준 병사 올라오게 하는 코루틴
        if (_rightCoroutine != null) StopCoroutine(_rightCoroutine);
        if (isParryingRight)
        {
            PrepareArmyPosition(rightArmy.transform);
            rightArmy.SetActive(true);
            _rightCoroutine = StartCoroutine(SummonArmy(rightArmy.transform));
        }
        else
        {
            rightArmy.SetActive(false);
        }
    }

    void Update()
    {
        EnemyCrossCheckAndKill();
    }

    void EnemyCrossCheckAndKill()
    {
        LinkedListNode<FollowingEnemy> currentEnemy = nearbyEnemies.First;

        if (currentEnemy != null)
        {
            Vector3 forward = transform.forward;
            Vector3 dirToTarget = (currentEnemy.Value.enemy.position - transform.position).normalized;

            Vector3 crossProduct = GetCrossProduct(forward, dirToTarget);

            if (crossProduct.y > 0.1f)
            {
                Debug.Log("적이 오른쪽에 있습니다.");
                if (canParry == true && isParryingRight && nearbyEnemies.Count > 0)
                {
                    KillEnemy();
                }
            }
            else if (crossProduct.y < -0.1f)
            {
                Debug.Log("적이 왼쪽에 있습니다.");
                if (canParry == true && isParryingLeft && nearbyEnemies.Count > 0)
                {
                    KillEnemy();
                }
            }
        }
    }

    private void KillEnemy()
    {
        LinkedListNode<FollowingEnemy> currentEnemy = nearbyEnemies.First;

        if ( currentEnemy != null)
        {
            currentEnemy.Value.DestroyEnemy();
            
            nearbyEnemies.RemoveFirst();
        }
    }

    Vector3 GetCrossProduct(Vector3 a, Vector3 b)
    {
        float crossX = a.y * b.z - a.z * b.y;
        float crossY = a.z * b.x - a.x * b.z;
        float crossZ = a.x * b.y - a.y * b.x;
        return new Vector3(crossX, crossY, crossZ);
    }




    //내친구 제미나이가 해준 병사 올라오게 하는 코루틴
    [Range(0.1f, 1f)] public float riseDuration = 0.4f; // 올라오는 시간
    [Range(0.01f, 0.1f)] public float staggerDelay = 0.03f; // 병사 간 간격
    public float startDepth = -1.5f; // 시작 깊이

    // 실행 중인 코루틴을 기억해두기 위한 변수
    private Coroutine _leftCoroutine;
    private Coroutine _rightCoroutine;

    // 이 함수는 단순히 병사들을 미리 지하에 배치만 합니다.
    private void PrepareArmyPosition(Transform armyParent)
    {
        for (int i = 0; i < armyParent.childCount; i++)
        {
            Transform soldier = armyParent.GetChild(i);
            // 미리 X, Z는 유지한 채 Y만 지하로 내려둡니다.
            Vector3 targetPos = new Vector3(soldier.localPosition.x, 0, soldier.localPosition.z);
            soldier.localPosition = targetPos + Vector3.up * startDepth;
        }
    }

    // SummonArmy에서는 이제 위치 초기화 코드를 빼고 연출만 담당하면 됩니다.
    IEnumerator SummonArmy(Transform armyParent)
    {
        // 이미 PrepareArmyPosition에서 위치를 잡았으니 바로 시작합니다.
        for (int i = 0; i < armyParent.childCount; i++)
        {
            Transform soldier = armyParent.GetChild(i);
            Vector3 targetPos = new Vector3(soldier.localPosition.x, 0, soldier.localPosition.z);

            StartCoroutine(RiseIndividual(soldier, targetPos));
            yield return new WaitForSeconds(staggerDelay);
        }
    }

    // [핵심] 병사 하나하나를 부드럽게 올리는 코루틴
    IEnumerator RiseIndividual(Transform soldier, Vector3 target)
    {
        float elapsed = 0f;
        Vector3 start = soldier.localPosition;

        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / riseDuration;

            // 유니티 내장 함수인 SmoothStep을 쓰면 부드럽게 감속하며 도착합니다.
            float curve = Mathf.SmoothStep(0, 1, t);
            soldier.localPosition = Vector3.Lerp(start, target, curve);

            yield return null; // 다음 프레임까지 대기
        }

        soldier.localPosition = target;
    }
}
