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
        yield return new WaitForSeconds(1f);   //Esperando 1 segundo antes de iniciar a transição
        porta.SendMessage("interacao", new object[] { false, haveFade }, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
