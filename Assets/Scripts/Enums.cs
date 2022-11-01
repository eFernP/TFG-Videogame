using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorPuzzleAction {
    Decrease, 
    Increase, 
    Invert
}

public enum RandomEdgeType {
    Empty, 
    Wall,
    Door
}

public enum BossStatus
{
    OnGuard,
    NotMoving,
    Preparing,
    Charging,
    Attacking
}

public enum Character
{
    Hero,
    Boss,
}

