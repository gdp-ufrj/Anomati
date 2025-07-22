using UnityEngine;

public class PlayerCanMove : MonoBehaviour
{
    public PlayerController Player;
    void Start()
    {
        Player.canMove = true;
    }
}
