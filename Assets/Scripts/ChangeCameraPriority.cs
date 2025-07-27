using Unity.Cinemachine;
using UnityEngine;

public class ChangeCameraPriority : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    void Start()
    {
        cinemachineCamera.Priority = 10; // Define a prioridade da câmera para 10
    }
}
