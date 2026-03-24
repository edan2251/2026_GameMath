using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class SimpleAngleLauncher : MonoBehaviour
{
    public MoveSystem moveSystem;
    public MouseWheelScroller mouseWheelScroller;

    public GameObject HeadGFX;

    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firePoint;
    public float force = 15f;

    public void OnAttack(InputValue value)
    {
        if(value.isPressed)
        {
            UpgradeLaunch();
        }
    }

    public void UpgradeLaunch()
    {
        if(moveSystem.isMoving == false)
        {
            Launch();

            //내친구 제미나이가 해준 애니메이션
            if (_boingCoroutine != null) StopCoroutine(_boingCoroutine);
            _boingCoroutine = StartCoroutine(PlayBoingEffect());
        }
    }

    public void Launch()
    {
        float angle = mouseWheelScroller.currentAngle;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 localDir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        Quaternion bodyRotation = moveSystem.transform.rotation;

        Vector3 finalDir = bodyRotation * localDir;

        GameObject sphere = Instantiate(spherePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = sphere.GetComponent<Rigidbody>();

        rb.AddForce((finalDir + Vector3.up * 0.3f).normalized * force, ForceMode.Impulse);

    }


    //내친구 제미나이가 해준 애니메이션
    [Header("Boing Effect Settings")]
    public float boingDuration = 0.2f;   // 효과 지속 시간
    public Vector3 boingScale = new Vector3(1.2f, 0.8f, 1.2f); // 발사 시 변할 스케일 (예: 옆으로 퍼지고 위로 눌림)

    private Coroutine _boingCoroutine;

    // [연출] 머리 오브젝트의 스케일을 조절하는 코루틴
    IEnumerator PlayBoingEffect()
    {
        Transform headTransform = moveSystem.tankHeadGFX.transform;
        Vector3 originalScale = Vector3.one; // 기본 스케일 (보통 1, 1, 1)
        float elapsed = 0f;

        while (elapsed < boingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / boingDuration;

            // [수학 포인트] 사인파(0 ~ 180도)를 활용한 뾰잉 연출
            // Mathf.Sin(t * Mathf.PI)는 0에서 시작해 1까지 갔다가 다시 0으로 돌아옵니다.
            float curve = Mathf.Sin(t * Mathf.PI);

            // 기본 스케일에서 boingScale 방향으로 curve만큼 보간(Lerp)합니다.
            headTransform.localScale = Vector3.Lerp(originalScale, boingScale, curve);

            yield return null;
        }

        // 마지막에 정확히 원래 크기로 복구
        headTransform.localScale = originalScale;
    }
}
