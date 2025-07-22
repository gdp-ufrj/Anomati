using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pai : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Transform originalPosition;
    private Animator animator;

    [SerializeField] private List<GameObject> checkPoints;   //Lista de pontos no qual o pai pode parar durante a perseguição
    private int currentCheckPointIndex = 0;   //Índice do ponto atual na lista de checkPoints
    private bool checkpointSetted = false;  //Flag para verificar se o checkpoint foi definido
    [SerializeField] private float originalSpeed = 3f;  //Velocidade original do pai

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (agent.enabled)
        {
            Debug.Log("Remaining Distance: " + agent.remainingDistance);
            if (!IsDestinationReachable(target.position))    //Se não der para chegar no jogador
            {
                agent.speed = originalSpeed * 0.75f;  //Reduz a velocidade do pai
                if (!checkpointSetted)
                {
                    checkpointSetted = true;
                    SetCheckPoint();
                }
                //Debug.Log("Pai não consegue alcançar o jogador!");
            }
            else
            {
                checkpointSetted = false;
                agent.speed = originalSpeed;  //Reseta a velocidade do pai para a velocidade original
                agent.SetDestination(target.position); //Se der para chegar no jogador, define o destino como a posição do jogador
            }
        }
        UpdateWalkAnimation();  //Atualiza a animação de caminhada do pai
    }

    private void SetCheckPoint()
    {
        int indexCheckpointMenorDistancia = 0;
        float menorDistancia = float.MaxValue;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            GameObject checkPoint = checkPoints[i];

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(checkPoint.transform.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                float pathDistance = GetPathLength(path);
                if (pathDistance < menorDistancia)
                {
                    menorDistancia = pathDistance;
                    indexCheckpointMenorDistancia = i;
                }
            }
        }

        currentCheckPointIndex = indexCheckpointMenorDistancia;
        agent.SetDestination(checkPoints[currentCheckPointIndex].transform.position);
    }

    private float GetPathLength(NavMeshPath path)
    {
        float length = 0f;
        if (path.corners.Length < 2)
            return length;

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        return length;
    }

    private void UpdateWalkAnimation()
    {
        if (Globals.triggerDadRun)
        {
            Vector3 velocity = agent.desiredVelocity;   //Velocidade "intencional"
            Vector2 vel2D = new Vector2(velocity.x, velocity.y).normalized;

            if (agent.enabled)
            {
                if (vel2D.magnitude > 0.1f)
                {
                    animator.SetBool("IsWalking", true);
                    animator.SetFloat("InputX", vel2D.x);
                    animator.SetFloat("InputY", vel2D.y);
                }
                else
                    SetIdleDirection();
            }
            else
            {
                if (animator.GetBool("IsWalking"))
                {
                    animator.SetBool("IsWalking", false);
                    //Salva direção para idle
                    animator.SetFloat("LastInputX", animator.GetFloat("InputX"));
                    animator.SetFloat("LastInputY", animator.GetFloat("InputY"));
                }
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("LastInputX", 0f);
            animator.SetFloat("LastInputY", -1f);
        }
    }


    public void ResetPosition()
    {
        transform.position = originalPosition.position;  //Reseta a posição do pai para a posição original
        animator.SetBool("IsWalking", false);
        animator.SetFloat("LastInputX", 0f);
        animator.SetFloat("LastInputY", -1f);
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

    bool IsDestinationReachable(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool pathFound = agent.CalculatePath(destination, path);

        if (!pathFound || path.status != NavMeshPathStatus.PathComplete)
            return false;

        Vector3 finalPos = path.corners[path.corners.Length - 1];   //Ponto final do caminho real
        float distToTarget = Vector3.Distance(finalPos, destination);

        return distToTarget < 0.5f;   //Se a distância entre o final do caminho e o destino for grande, é porque não achou um caminho viável
    }

    public void SetIdleDirection()
    {
        animator.SetFloat("InputX", 0);
        animator.SetFloat("InputY", -1);

        animator.SetFloat("LastInputX", 0);
        animator.SetFloat("LastInputY", -1);

        animator.SetBool("IsWalking", false);
    }
}