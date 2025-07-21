using UnityEngine;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Config")]
    [SerializeField] private float maxStamina = 5f;   //5 segundos de corrida
    [SerializeField] private float staminaRecoveryRate = 1f, staminaDrainRate = 1f;    //Por segundo

    [Header("Status")]
    [SerializeField] private bool isExhausted = false;
    private float currentStamina;

    private PlayerController playerController;

    void Start()
    {
        currentStamina = maxStamina;
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        HandleStamina(playerController.isSprinting);
    }

    void HandleStamina(bool wantsToSprint)
    {
        if (wantsToSprint && !isExhausted)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isExhausted = true;
            }
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isExhausted = false;
            }
        }
    }

    public bool CanRun(bool wantsToSprint)
    {
        return wantsToSprint && !isExhausted && currentStamina > 0f;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
    }
}
