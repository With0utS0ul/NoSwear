public interface IPlayerRepository
{
    void Save(Player player, PlayerController controller, GameData data);
    void Load(GameData data, Player player, PlayerController controller);
}