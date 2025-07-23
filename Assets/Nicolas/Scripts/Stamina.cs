using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Stamina Config")]
    [SerializeField] private float maxStamina = 5f;   //5 segundos de corrida
    [SerializeField] private float staminaRecoveryRate = 1f, staminaDrainRate = 1f;    //Por segundo

    [Header("Status")]
    private bool isExhausted = false;
    private float currentStamina;
    private PlayerController playerController;
    public Image staminaFillImage;    //Referência para a UI de Stamina

    void Start()
    {
        currentStamina = maxStamina;
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.canRecoverStamina)
            return;
        HandleStamina(playerController.isSprinting);
    }

    void HandleStamina(bool isSprinting)
    {
        if (isSprinting && !isExhausted)
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

        if (staminaFillImage != null)
        {
            float percent = GetStaminaPercent();
            staminaFillImage.fillAmount = percent;    //Atualizando a barra de stamina

            Color corMin = Color.red;
            Color corMax = Color.yellow;
            staminaFillImage.color = Color.Lerp(corMin, corMax, percent);
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

    public void ResetStamina()
    {
        currentStamina = maxStamina;
        isExhausted = false;
        if (staminaFillImage != null)
        {
            staminaFillImage.fillAmount = 1f;    //Reseta a barra de stamina
            staminaFillImage.color = Color.yellow;   //Reseta a cor da barra de stamina
        }
    }
}
