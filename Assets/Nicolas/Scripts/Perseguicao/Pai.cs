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
}
