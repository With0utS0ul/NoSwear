using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SettingsMenuEntryPoint : MonoBehaviour
{

    [SerializeField] private SettingsView view;
    

    private SettingsController controller;
    
    
    void Start()
    {
        IAudioService audioService = GameEntryPoint.Instance?.AudioService;
        ISaveService saveService = GameEntryPoint.Instance?.SaveService;
        SettingsModel model = new SettingsModel(saveService);
        controller = new SettingsController(audioService, model, view);
    }

    private void OnDestroy()
    {
        controller?.Dispose();
    }


}
