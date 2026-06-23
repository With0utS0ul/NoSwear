using UnityEngine;

public class SettingsModel
{

    private readonly ISaveService _saveService;
    private const string VolumeKey = "Volume";

    public float Volume { get; set; }


    public SettingsModel(ISaveService saveService)
    {
        _saveService = saveService;
    }
    public void Load()
    {
        Volume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(VolumeKey, Volume);
        PlayerPrefs.Save();
    }
}