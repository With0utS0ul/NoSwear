using UnityEngine;

public class SettingsMenuEntryPoint : MonoBehaviour
{

    [SerializeField] private SettingsView view;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. Забираем готовые сервисы из точки входа
        IAudioService audioService = GameEntryPoint.Instance.AudioService;
        ISaveService saveService = GameEntryPoint.Instance.SaveService;

        // 2. Создаем модель, передавая ей сервис сохранения
        SettingsModel model = new SettingsModel(saveService);

        // 3. Создаем контроллер, который запускает всю логику
        SettingsController controller = new SettingsController(audioService, model, view);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
