using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerView playerView;
    [SerializeField] private Image readyIcon;
    [SerializeField] private Image cooldownIcon;
    [SerializeField] private Text cooldownText;

    private Player player;

    private void Start()
    {
        if (playerView != null)
        {
            if (playerView.Player != null)
                OnPlayerReady(playerView.Player);
            else
                playerView.OnPlayerReady += OnPlayerReady;
        }
    }

    private void OnPlayerReady(Player player)
    {
        this.player = player;
        if (playerView != null)
            playerView.OnPlayerReady -= OnPlayerReady; // отписка
    }

    private void Update()
    {
        if (player == null) return;

        float remaining = player.GetMagicCooldownRemaining();
        bool isOnCooldown = remaining > 0.01f;

        if (!isOnCooldown)
        {
            readyIcon.enabled = true;
            cooldownIcon.enabled = false;
            cooldownText.text = "";
        }
        else
        {
            readyIcon.enabled = false;
            cooldownIcon.enabled = true;
            cooldownText.text = remaining.ToString("F1");
        }
    }
}