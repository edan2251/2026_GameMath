using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float rotation = moveInput.x * rotationSpeed * Time.deltaTime;   
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;
    }
}
