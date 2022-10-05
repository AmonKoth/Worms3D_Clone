using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera _virtualCamera;
    private IdleCameraLoc _idleCameraLocaiton;
    private void Start()
    {
        if (FindObjectOfType<CameraManager>() != this)
        {
            Destroy(gameObject);
        }
        _virtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        _idleCameraLocaiton = FindObjectOfType<IdleCameraLoc>();
        // if (_virtualCamera == null)
        // {
        //     Instantiate(new Cinemachine.CinemachineVirtualCamera(), this.transform.position, Quaternion.identity);
        // }
    }

    public void SetTarget(Transform target)
    {
        _virtualCamera.m_LookAt = target;
        _virtualCamera.m_Follow = target;
    }
    public void SetFolow(Transform target)
    {
        _virtualCamera.m_Follow = target;
    }
    public void SetCameraIdle()
    {
        _virtualCamera.m_Follow = _idleCameraLocaiton.transform;
    }

}
