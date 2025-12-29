using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public static GameManagerController Instance { get; private set; } // Defines Singleton controller

    public enum GameState { Menu, GameOver, Playing, GoalReached }
    public GameState currentState { get; private set; }
    public enum CollectibleItem { Bear, Gameboy }

    [Header("Managers")]
    [SerializeField] private MusicManagerController m_musicManager;

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

    public void SetItemCollected(CollectibleItem key)
    {
        if (!m_itemsCollected.ContainsKey(key))
        {
            return;
        }

        Debug.Log($"Item key found: {key}");
        m_itemsCollected[key] = true;

        m_AllItemsCollected = AreAllItemsCollected(); // Check if everything has been collected
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
}
