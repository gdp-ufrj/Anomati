using UnityEngine;

public class HidePlayer : MonoBehaviour
{
    private void OnEnable()
    {
        GameControllerNicolas.GetInstance().HidePlayer();
        gameObject.SetActive(false);
    }
}
