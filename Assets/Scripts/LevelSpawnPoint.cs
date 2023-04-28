using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnPoint : MonoBehaviour
{
    public SpawnPointType PointType;
    public enum SpawnPointType : int
    {
        Entrance, Checkpoint, Exit
    }
}
