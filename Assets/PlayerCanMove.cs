using UnityEngine;

public class PlayerCanMove : MonoBehaviour
{
    public PlayerController Player;
    public GameObject porta;

    void Start()
    {
        //Player.canMove = true;
        porta.SendMessage("interacao", false, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
