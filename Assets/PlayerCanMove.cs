using UnityEngine;
using System.Collections;

public class PlayerCanMove : MonoBehaviour
{
    public PlayerController Player;
    public GameObject porta;
    public bool isTeleport = false;

    void Start()
    {
        if (isTeleport)
        {
            StartCoroutine(Teleport());
        }
        else
        {
            Player.canMove = true;
        }
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);   //Esperando 1 srgundo antes de iniciar a transição
        porta.SendMessage("interacao", false, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
