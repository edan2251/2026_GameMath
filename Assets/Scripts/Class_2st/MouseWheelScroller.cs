using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWheelScroller : MonoBehaviour
{
    public float currentAngle = 0f;
    public float sensitivity = 0.05f;

    public MoveSystem moveSystem;
    public Transform arrowTransform;
    
    void Update()
    {
        Scroller();
    }

    void Scroller()
    {
        Vector2 scroll = Mouse.current.scroll.ReadValue();

        if (scroll.y != 0)
        {
            currentAngle += scroll.y * sensitivity;

            currentAngle = Mathf.Repeat(currentAngle, 360f);

            arrowTransform.localRotation = Quaternion.Euler(90f, -currentAngle, 0f);
            moveSystem.tankHeadGFX.transform.rotation = Quaternion.Euler(0f, -currentAngle + 90, 0f);
        }
    }
}
