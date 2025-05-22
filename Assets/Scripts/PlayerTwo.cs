using UnityEngine;

public class PlayerTwo : PlayerController
{
    protected override string ActionMapName => "Player2";

    protected override void PerformAction2()
    {
        base.PerformAction2();
        Debug.Log("PlayerOne specific Action2 logic here.");
    }
}
