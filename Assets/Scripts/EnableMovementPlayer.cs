using UnityEngine;

public class EnableMovementPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        GameControllerNicolas.GetInstance().EnablePlayerMovement();
        gameObject.SetActive(false);   //Desativa este objeto após ativar o movimento do jogador
    }
}
