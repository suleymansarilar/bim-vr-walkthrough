using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

      
        Vector3 toCamera = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
    }
}