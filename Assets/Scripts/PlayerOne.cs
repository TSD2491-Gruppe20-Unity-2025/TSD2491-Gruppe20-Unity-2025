using UnityEngine;

public class PlayerOne : PlayerController
{
    protected override string ActionMapName => "Player1";

    protected override void PerformAction2()
    {
        base.PerformAction2();
        Debug.Log("PlayerOne specific Action2 logic here.");
    }
}
