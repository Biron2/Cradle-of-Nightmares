using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // Поворот объекта вокруг оси Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}