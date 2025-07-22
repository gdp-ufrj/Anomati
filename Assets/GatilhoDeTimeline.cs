using UnityEngine;

public class GatilhoDeTimeline : MonoBehaviour
{
    public GameObject TimelineObject;
    public PlayerController Player;

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("player"))
            TimelineObject.SetActive(true);
            Player.canMove = false;
    }

}
