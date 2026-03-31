using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerView playerView;
    [SerializeField] private Image readyIcon;
    [SerializeField] private Image cooldownIcon;
    [SerializeField] private Text cooldownText;

    private Player player;


    public void Init(Player player)
    {
        this.player = player;
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