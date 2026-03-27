using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EntityView playerView;

    private Player player;

    private void Awake()
    {
        var damageService = new DamageService();
        var saveService = GameEntryPoint.Instance.SaveService;

        player = new Player(new Health(100), damageService);

        playerView.Init(player);
        playerController.Init(player);
    }
}