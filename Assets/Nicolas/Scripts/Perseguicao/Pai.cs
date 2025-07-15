using UnityEngine;
using UnityEngine.AI;

public class Pai : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Transform originalPosition;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }

    public void ResetPosition()
    {
        transform.position = originalPosition.position;  //Reseta a posição do pai para a posição original
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            if (Globals.triggerDadRun && !Globals.endDadRun && !DoorTransitionController.GetInstance().isTransitioning)
            {
                Debug.Log("Pai colidiu com o jogador durante a perseguição!");
                GameControllerNicolas.GetInstance().ResetDadRun();    //Reseta a perseguição com o pai
            }
        }
    }
}
