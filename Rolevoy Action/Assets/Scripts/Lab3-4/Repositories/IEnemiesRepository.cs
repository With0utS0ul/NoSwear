using UnityEngine;

public interface IEnemiesRepository
{
    void Save(GameObject[] enemies, GameData data);
    void Load(GameData data, GameObject[] enemies);
}