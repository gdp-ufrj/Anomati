using UnityEngine;

public class ShowPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        GameControllerNicolas.GetInstance().ShowPlayer();
        gameObject.SetActive(false);
    }
}
