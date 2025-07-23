using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    public bool isFlashback = true, isFlashForward = false;    //Variáveis para determinar se é um flashback ou um flashforward
    public GameObject porta;

    void Start()
    {
        StartCoroutine(Teleport());
    }

    private IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);   //Esperando 1 segundo antes de iniciar a transição
        if (isFlashback)
        {
            GameControllerNicolas.GetInstance().ChangePlayerSprite("Past");
        }
        else if (isFlashForward)
        {
            GameControllerNicolas.GetInstance().ChangePlayerSprite("Present");
        }
        bool haveFade = !isFlashback && !isFlashForward;
        porta.SendMessage("interacao", new object[] { false, haveFade }, SendMessageOptions.DontRequireReceiver);   //Inicia a transição para a casa do pai
    }
}
