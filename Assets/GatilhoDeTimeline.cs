using UnityEngine;

public class GatilhoDeTimeline : MonoBehaviour
{
    public GameObject TimelineObject;
    public bool BlockMovement = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("player"))
        {
            TimelineObject.SetActive(true);
            if (BlockMovement)
            {
                GameControllerNicolas.GetInstance().DisablePlayerMovement();
            }
        }
    }

}
