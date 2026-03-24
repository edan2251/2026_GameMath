using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParrySystem : MonoBehaviour
{
    public bool isParryingLeft = false;
    public bool isParryingRight = false;

    public void OnParryLeft(InputValue value)
    {
        isParryingLeft = value.isPressed;

    }

    public void OnParryRight(InputValue value)
    {
        isParryingRight = value.isPressed;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
