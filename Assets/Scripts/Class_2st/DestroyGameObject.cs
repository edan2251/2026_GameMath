using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 5f);
    }
}
