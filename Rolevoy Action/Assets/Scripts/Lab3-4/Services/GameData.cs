using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public float PlayerHP;
    public Vector3 PlayerPosition;
    public List<Vector3> EnemyPositions; // дополнительный балл
}