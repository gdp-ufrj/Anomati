using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;

public class PlayerCanMove : MonoBehaviour
{
    public PlayerController Player;
    public bool CanMove = false;
    public bool haveFade = true;
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
            if (CanMove)
            {
                Player.canMove = true;
            }
        }
    }

    private IEnumerator Teleport()
    {
        object[] Array = new object[2];
        Array[0] = false;
        Array[1] = haveFade;
        yield return new WaitForSeconds(1f);   //Esperando 1 srgundo antes de iniciar a transição
        porta.SendMessage("interacao", Array, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
