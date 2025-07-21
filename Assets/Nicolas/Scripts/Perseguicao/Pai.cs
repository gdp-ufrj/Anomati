using UnityEngine;
using UnityEngine.AI;

public class Pai : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    public Transform originalPosition;
    private Animator animator;

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
            agent.SetDestination(target.position);
        UpdateWalkAnimation();  //Atualiza a animação de caminhada do pai
    }

    private void UpdateWalkAnimation()
    {
        if (Globals.triggerDadRun)
        {
            Vector3 velocity = agent.desiredVelocity;   //Velocidade "intencional"
            Vector2 vel2D = new Vector2(velocity.x, velocity.y).normalized;
            //Debug.Log("Pai velocity x: " + velocity.x + ", y: " + velocity.y + ", z: " + velocity.z);

            if (agent.enabled)
            {
                animator.SetBool("IsWalking", true);
                animator.SetFloat("InputX", vel2D.x);
                animator.SetFloat("InputY", vel2D.y);
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
}
