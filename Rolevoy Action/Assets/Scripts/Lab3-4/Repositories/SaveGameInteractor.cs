using UnityEngine;

public class SaveGameInteractor 
{
    private readonly IPlayerRepository playerRepository;
    private readonly IEnemiesRepository enemiesRepository;
    private readonly ISaveService saveService;

    public SaveGameInteractor(IPlayerRepository playerRepo, IEnemiesRepository enemiesRepo, ISaveService saveService)
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
}
