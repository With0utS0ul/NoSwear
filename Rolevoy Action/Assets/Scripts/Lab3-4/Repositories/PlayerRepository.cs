using UnityEngine;

public class PlayerRepository : IPlayerRepository
{
    public void Save(Player player, PlayerController controller, GameData data)
    {
        data.PlayerHP = player.Health.Current;
        data.PlayerPosition = controller.transform.position;
    }

    public void Load(GameData data, Player player, PlayerController controller)
    {
        player.Health.Restore(data.PlayerHP);
        controller.Teleport(data.PlayerPosition);
    }
}