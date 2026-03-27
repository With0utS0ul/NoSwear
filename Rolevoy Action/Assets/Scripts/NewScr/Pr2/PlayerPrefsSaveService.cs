using UnityEngine;

public class PlayerPrefsSaveService : ISaveService
{
    public void Save(GameData data)
    {
        PlayerPrefs.SetFloat("hp", data.PlayerHP);
        PlayerPrefs.SetString("pos", JsonUtility.ToJson(data.PlayerPosition));
    }

    public GameData Load()
    {
        var data = new GameData();

        data.PlayerHP = PlayerPrefs.GetFloat("hp", 100);

        var posJson = PlayerPrefs.GetString("pos", "");
        if (!string.IsNullOrEmpty(posJson))
            data.PlayerPosition = JsonUtility.FromJson<Vector3>(posJson);

        return data;
    }
}