
using UnityEngine;

public static class PlayerData
{
    public static int Score { get; set; }
    public static float Timer { get; set; }
    public static int currentScene { get; set; }
    public static Direction_Hor HorizontalDir { get; set; }
    public static Direction_Vert VerticalDir { get; set; }
    public static PlayerState playerState { get; set; }
    public static InteractState interactState { get; set; }
    public static bool ePress { get; set; }
    public static int counterDaikon { get; set; }
}
