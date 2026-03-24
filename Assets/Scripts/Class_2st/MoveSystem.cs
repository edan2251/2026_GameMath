using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSystem : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public Vector2 moveInput;

    Vector3 normalizedVector;

    public bool isMoving = false;

    public GameObject tankGFX;
    public GameObject tankHeadGFX;

    public SpriteRenderer triangleSprite;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        isMoving = true;
    }

    void Start()
    {
        isMoving = false;
    }

    void Update()
    {
        LetsMove();
        LetsLook();
        TriangleColorChanger();
    }

    void LetsMove()
    {
        Vector3 direction = new Vector3(0, 0, moveInput.y);

        float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        if (magnitude > 0) normalizedVector = direction / magnitude;
        else normalizedVector = Vector3.zero;

        transform.Translate(normalizedVector * moveSpeed * Time.deltaTime);

        if (magnitude == 0) isMoving = false;
    }

    void LetsLook()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
            transform.localRotation = rotation * transform.localRotation;
        }
    }

    void TriangleColorChanger()
    {
        if(isMoving == true)
        {
            triangleSprite.color = Color.black;
        }
        else
        {
            triangleSprite.color = Color.white;
        }
    }
}
