public interface IPeacefulModeService
{
    bool IsPeaceful { get; set; }
    void RegisterEnemy(IEnemyPeacefulHandler enemy);
    void UnregisterEnemy(IEnemyPeacefulHandler enemy);
}