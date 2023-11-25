using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public void SetCameraToTarget(Vector3 target, float YValue = 0)
    {
        if (target != null)
            transform.position = new Vector3(target.x, target.y + YValue, CameraManager.Instance.cameraDistance);
    }

    public void MoveCameraToTarget(Vector3 target, float speed = 0.1f, float distanceFrom = 2, bool instant = false)
    {
        if (target != null && CameraManager.Instance.cameraDistance != target.z - distanceFrom)
            CameraManager.Instance.cameraDistance = instant ? target.z - distanceFrom : Mathf.MoveTowards(CameraManager.Instance.cameraDistance, target.z - distanceFrom, speed);
    }

    public void MoveCameraToOriginal(float speed = 0.01f)
    {
        if (CameraManager.Instance.cameraDistance != CameraManager.Instance.originalCameraDistance)
            CameraManager.Instance.cameraDistance = Mathf.MoveTowards(CameraManager.Instance.cameraDistance, CameraManager.Instance.originalCameraDistance, speed);
    }
    public void RotateTowardsTarget(Vector3 target, float speed = 0.1f, bool instant = false)
    {
        if (target != null)
        {
            var direction = (target - transform.position).normalized;
            var rotation = Quaternion.LookRotation(direction);
            CameraManager.Instance.transform.rotation = instant ? rotation : Quaternion.RotateTowards(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
}