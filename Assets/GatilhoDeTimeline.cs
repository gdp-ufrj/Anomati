using UnityEngine;

public class GatilhoDeTimeline : MonoBehaviour
{
    public GameObject TimelineObject;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("player"))
        {
            TimelineObject.SetActive(true);
            GameControllerNicolas.GetInstance().DisablePlayerMovement();
        }
    }

}
