using UnityEngine;

public class ResetAtelieAto2 : MonoBehaviour
{
    public DialogoTrigger dialogoTriggerHugo;
    public GameObject triggerTimelineAtelie, triggerTimelineCentro;

    void Start()
    {
        dialogoTriggerHugo.enabled = false;
        triggerTimelineAtelie.SetActive(true);
        triggerTimelineCentro.SetActive(true);
        Destroy(gameObject);
    }
}
