using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float currentSpeed;
    public float sprintMultipleValue = 2f;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting = false;

    Vector3 normalizedVector;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }


    public void OnClick(InputValue value)
    {
        if(value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if ( hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;             //Plane 에 마우스 클릭한 곳 타겟
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break;
                }
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed; //LShift 누르고 있으면 true, 떼면 false
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 A = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
            Vector3 B = transform.position;

            Vector3 fowardVector = new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);

            float sqrMagnitude = fowardVector.x * fowardVector.x + fowardVector.y * fowardVector.y + fowardVector.z * fowardVector.z;
            float magnitude = Mathf.Sqrt(sqrMagnitude);

            if (isSprinting == true)
            {
                currentSpeed = moveSpeed * sprintMultipleValue;
            }
            if (isSprinting == false)
            {
                currentSpeed = moveSpeed;
            }

            if (magnitude > 0) normalizedVector = fowardVector / magnitude;
            else normalizedVector = Vector3.zero;

            transform.Translate(normalizedVector * currentSpeed * Time.deltaTime);

            if (magnitude <= 0.1)
            {
                isMoving = false;
            }



        }


        
    }
}
