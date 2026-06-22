using UnityEngine;

public class LoadGameInteractor
{
    private readonly IPlayerRepository playerRepository;
    private readonly IEnemiesRepository enemiesRepository;
    private readonly ISaveService saveService;

    public LoadGameInteractor(IPlayerRepository playerRepo, IEnemiesRepository enemiesRepo, ISaveService saveService)
    {
        this.playerRepository = playerRepo;
        this.enemiesRepository = enemiesRepo;
        this.saveService = saveService;
    }


    public void LoadGame(Player player, PlayerController controller)
    {
        GameData data = saveService.Load();
        if (data == null) return;

        playerRepository.Load(data, player, controller);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesRepository.Load(data, enemies);
        Debug.Log("Game loaded via Interactor");
    }
}