using UnityEngine;
using System.Collections;

public class DadRunEnable : MonoBehaviour
{
    void Start()
    {
        if (Globals.triggerDadRun && !Globals.endDadRun)
        {
            StartCoroutine(EnableDad());
        }
    }

    private IEnumerator EnableDad()
    {
        yield return new WaitForSeconds(1f);   //Esperando 1 segundo antes de iniciar a transição
        GameControllerNicolas.GetInstance().EnableSprintSystem();
        GameControllerNicolas.GetInstance().EnableDadRun();  //Ativa a perseguição com o pai
    }
}
