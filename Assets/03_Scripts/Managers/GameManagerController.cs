using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; } // Defines Singleton controller

    public enum GameState { Menu, GameOver, Playing, CompleteEnding, PartialEnding }
    public GameState m_currentState { get; private set; }
    public enum CollectibleItem { Bear, Gameboy, Noose }

    [Header("Managers")]
    [SerializeField] private MusicManagerController m_musicManager;
    [SerializeField] private UiManagerController m_uiManager;

    [Header("Collectable Components")]
    [SerializeField] private bool m_AllItemsCollected = false;
    [SerializeField] private Dictionary<CollectibleItem, bool> m_itemsCollected = new Dictionary<CollectibleItem, bool>();

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_itemsCollected = new Dictionary<CollectibleItem, bool>();

        foreach (CollectibleItem item in System.Enum.GetValues(typeof(CollectibleItem)))
        {
            m_itemsCollected[item] = false;
        }
    }

    private void SetGameState(GameState state)
    {
        m_currentState = state;
    }

    public void SetItemCollected(CollectibleItem key, string message = "")
    {
        if (!m_itemsCollected.ContainsKey(key))
        {
            return;
        }

        Debug.Log($"Item key found: {key}");
        m_itemsCollected[key] = true;
        m_uiManager.ShowMessage(message);
        m_AllItemsCollected = AreAllItemsCollected();
    }

    public bool AreAllItemsCollected()
    {
        foreach (bool collected in m_itemsCollected.Values)
        {
            if (!collected)
            {
                return false;
            }
        }

        return true;
    }

    public void SetGameOver()
    {
        m_uiManager.SetStatic(true);
        Debug.Log("GAME OVER");
    }

    public void EndGame()
    {
        if (m_AllItemsCollected)
        {
            SetGameState(GameState.CompleteEnding);
        }
        else
        {
            SetGameState(GameState.PartialEnding);
        }

        Debug.Log(m_currentState);
    }
}
