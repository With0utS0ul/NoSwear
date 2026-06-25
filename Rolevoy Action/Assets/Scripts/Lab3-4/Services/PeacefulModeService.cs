using System.Collections.Generic;

public class PeacefulModeService : IPeacefulModeService
{
    private bool isPeaceful = false;
    private List<IEnemyPeacefulHandler> enemies = new List<IEnemyPeacefulHandler>();

    public bool IsPeaceful
    {
        get => isPeaceful;
        set
        {
            if (isPeaceful == value) return;
            isPeaceful = value;
            foreach (var enemy in enemies)
                enemy.OnPeacefulModeChanged(isPeaceful);
        }
    }

    public void RegisterEnemy(IEnemyPeacefulHandler enemy)
    {
        if (!enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void UnregisterEnemy(IEnemyPeacefulHandler enemy)
    {
        enemies.Remove(enemy);
    }
}