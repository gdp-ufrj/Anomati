using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    [SerializeField] private GameObject cinemachineCamera; //Referência à câmera cinemachine
    [SerializeField] private Transform player; //Referência ao jogador

    public static CameraController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetNewBounds(Collider2D newBounds)   //Define os novos limites da câmera
    {
        cinemachineCamera.GetComponent<CinemachineCamera>().ForceCameraPosition(player.position, Quaternion.identity); //Força a posição da câmera para a posição do jogador
        cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = newBounds; //Define os novos limites
        cinemachineCamera.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache(); //Invalida o cache dos limites
    }
    
    public string GetCurrentBoundsName()   //Retorna o nome do gameobject que está definindo os limites atuais da câmera
    {
        return cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D.gameObject.name;
    }
}
