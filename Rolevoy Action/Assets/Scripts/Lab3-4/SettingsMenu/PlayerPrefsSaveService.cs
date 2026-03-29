using UnityEngine;

public class PlayerPrefsSaveService : ISaveService
{
    public void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public GameData Load()
    {
        string json = PlayerPrefs.GetString("SaveData", "");
        if (!string.IsNullOrEmpty(json))
            return JsonUtility.FromJson<GameData>(json);
        return new GameData(); // или null
    }
}