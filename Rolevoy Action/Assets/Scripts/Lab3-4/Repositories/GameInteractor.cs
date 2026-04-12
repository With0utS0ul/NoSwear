using UnityEngine;

public class GameInteractor
{
    private readonly IPlayerRepository playerRepository;
    private readonly IEnemiesRepository enemiesRepository;
    private readonly ISaveService saveService;

    public GameInteractor(IPlayerRepository playerRepo, IEnemiesRepository enemiesRepo, ISaveService saveService)
    {
        this.playerRepository = playerRepo;
        this.enemiesRepository = enemiesRepo;
        this.saveService = saveService;
    }

    public void SaveGame(Player player, PlayerController controller)
    {
        GameData data = new GameData();
        playerRepository.Save(player, controller, data);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesRepository.Save(enemies, data);
        saveService.Save(data);
        Debug.Log("Game saved via Interactor");
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