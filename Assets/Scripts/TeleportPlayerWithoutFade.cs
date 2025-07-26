using UnityEngine;

public class TeleportPlayerWithoutFade : MonoBehaviour
{
    public int idleDirection;    //Vai controlar a direção do idle do jogador após o teleporte
    public Transform newPosition; //Nova posição para onde o jogador será teleportado

    void Start()
    {
        if (newPosition == null)
            GameControllerNicolas.GetInstance().MovePlayerToPosition(null, idleDirection);
        else
            GameControllerNicolas.GetInstance().MovePlayerToPosition(newPosition, idleDirection);
    }
}
