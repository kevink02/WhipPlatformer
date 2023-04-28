using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : MonoBehaviour
{
    public LevelPointType PointType;
    public enum LevelPointType : int
    {
        Entrance, Checkpoint, Exit, Patrol
    }
}
