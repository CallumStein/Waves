using UnityEngine;

public class UiManagerController : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_gameOverOverlay;
    [SerializeField] private MessagePopupController m_messageOverlay;


    private void Update()
    {
        if (m_gameOverOverlay.alpha > 0)
        {
            m_gameOverOverlay.alpha = 0.85f + Mathf.PerlinNoise(Time.time * 25f, 0) * 0.15f;

        }
    }

    public void SetStatic(bool enabled)
    {
        m_gameOverOverlay.alpha = enabled ? 1f : 0f;
    }

    public void ShowMessage(string message)
    {
        if (m_messageOverlay != null) 
        { 
            m_messageOverlay.Show(message);
        }
    }

    public void ShowEndingMessage(string message)
    {
        if (m_messageOverlay != null)
        {
            m_messageOverlay.ShowEnding(message);
        }
    }
}
