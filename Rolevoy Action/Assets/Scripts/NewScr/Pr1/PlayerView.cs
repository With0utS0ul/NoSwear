using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Player Player { get; private set; }

    [Header("Config")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private PlayerController controller;

    private void Awake()
    {
        IHealth health = new Health(maxHealth);
        IDamageService damageService = new DamageService();

        Player = new Player(health, damageService);

        controller.Init(Player);
    }
}