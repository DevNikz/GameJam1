using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartState {
    Yes,
    No
}

public enum GameState {
    Play,
    End,
}

public enum LevelState {
    Playable,
    Unplayable,
}

public enum TimerState{
    Playing,
    Paused,
}

public enum SFXState {
    Playing,
    Paused
}

public enum VFXState {
    Playing,
    Paused
}

public enum Direction_Hor {
    Left,
    Right
}

public enum Direction_Vert {
    Up,
    Down
}

public enum PlayerState {
    Moving,
    Idle
}

public enum InteractState {
    None,
    Interacted
}