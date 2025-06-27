using UnityEngine;
using UnityEngine.AI;

public class Pai : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Disable automatic rotation to control it manually
        agent.updateUpAxis = false; // Disable automatic up-axis updates
    }

    void Update()
    {
        agent.SetDestination(target.position);
    }
}
