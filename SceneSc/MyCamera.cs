using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;

    public LayerMask cameraCollision;

    public Transform target; // Player

    [SerializeField]
    private float rotSensitive = 3f; // 카메라 회전 감도
    [SerializeField]
    private float dis = 2f; // 카메라와 플레이어 사이의 거리
    [SerializeField]
    private float height = 2f; // 카메라 높이
    [SerializeField]
    private float RotationMin = -10f; // 카메라 회전 각도 최소
    [SerializeField]
    private float RotationMax = 80f; // 카메라 회전 각도 최대
    private float smoothTime = 0.12f; // 카메라가 회전하는데 걸리는 시간
    private Vector3 targetRotation;
    private Vector3 currentVel;

    [SerializeField]
    private float maxDistance = 5f; // 카메라와 플레이어 사이의 최대 거리
    [SerializeField]
    private float maxHeight = 3f; // 카메라의 최대 높이
    [SerializeField]
    private float minHeight = 1f; // 카메라의 최소 높이

    private bool isCameraMovementEnabled = true; // 카메라 이동 제한 변수

    void LateUpdate() // Player가 움직이고 그 후 카메라가 따라가야 하므로 LateUpdate
    {
        if (!isCameraMovementEnabled)
        {
            return;
        }
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive; // 마우스 좌우 움직임을 입력받아서 카메라의 Y축을 회전시킨다
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * rotSensitive; // 마우스 상하 움직임을 입력받아서 카메라의 X축을 회전시킨다
        // Xaxis는 마우스를 아래로 했을 때(음수값이 입력받아질 때) 값이 더해져야 카메라가 아래로 회전한다 

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);
        // X축 회전이 한계치를 넘지 않게 제한해준다.

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation;
        // SmoothDamp를 통해 부드러운 카메라 회전

        Vector3 desiredPosition = (target.position - transform.forward * dis) + (Vector3.up * height);
        // 카메라의 위치는 플레이어보다 설정한 값만큼 떨어져 있게 계속 변경된다.

        Vector3 rayDir = desiredPosition - target.position;

        if (Physics.Raycast(target.position, rayDir, out RaycastHit hit, float.MaxValue, cameraCollision))
        {
            transform.position = hit.point - rayDir.normalized;
        }
        else
        {
            transform.position = desiredPosition;
        }

        // 카메라와 플레이어 사이의 거리가 maxDistance를 넘지 않도록 제한
        float currentDistance = Vector3.Distance(transform.position, target.position);
        if (currentDistance > maxDistance)
        {
            Vector3 direction = (transform.position - target.position).normalized;
            transform.position = target.position + direction * maxDistance;
        }

        // 카메라의 높이가 maxHeight를 넘지 않도록 제한
        if (Mathf.Abs(transform.position.y - target.position.y) > maxHeight)
        {
            float clampedHeight = Mathf.Clamp(transform.position.y, target.position.y, target.position.y + maxHeight);
            transform.position = new Vector3(transform.position.x, clampedHeight, transform.position.z);
        }

        // 카메라의 높이가 minHeight 이하로 내려가지 않도록 제한
        if (transform.position.y < target.position.y + minHeight)
        {
            float clampedMinHeight = Mathf.Clamp(transform.position.y, target.position.y + minHeight, Mathf.Infinity);
            transform.position = new Vector3(transform.position.x, clampedMinHeight, transform.position.z);
        }
    }
    public void SetCameraMovementEnabled(bool enabled)
    {
        isCameraMovementEnabled = enabled;
    }
}