using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class EmulatedAxisInputProvider : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineOrbitalTransposer orbitalTransposer;

    [SerializeField] float maxFov;
    [SerializeField] float minFov;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmulateInput(float delta)
    {
        orbitalTransposer.m_XAxis.m_InputAxisValue = delta;
    }
    public void ZoomCamera(float delta)
    {
        virtualCamera.m_Lens.FieldOfView += delta;
        virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView, minFov, maxFov);
    }

    
}
