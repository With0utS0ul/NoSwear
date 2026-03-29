using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private PlayerView playerView;
    [SerializeField] private Image readyIcon;
    [SerializeField] private Image cooldownIcon;
    [SerializeField] private Text cooldownText;

    private void Update()
    {
        float remaining = playerView.Player.GetMagicCooldownRemaining();

        if (remaining <= 0f)
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