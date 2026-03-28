using UnityEngine;
public interface ISaveService
{
    void Save(GameData data);
    GameData Load();
}