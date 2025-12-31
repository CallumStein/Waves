using UnityEngine;
using static GameManagerController;

[RequireComponent(typeof(Renderer))]
public class InteractableObject : MonoBehaviour, IInteractable, IHighlightable
{
    private Material mat;

    [Header("Item Settings")]
    [SerializeField] private CollectibleItem m_itemKey;
    [SerializeField] [TextArea(5, 5)] private string m_messageText;

    [Header("Outline Settings")]
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.05f;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;

        // Ensure the outline is initially off
        mat.SetFloat("_Highlight", 0f);
    }

    public void Interact()
    {
        Debug.Log($"{name} was interacted with!");
        GameManagerController.Instance.SetItemCollected(m_itemKey, m_messageText);
    }

    public void Highlight(bool state)
    {
        if (state)
        {
            // Enable the outline pass
            mat.SetFloat("_Highlight", 1f);
            mat.SetColor("_OutlineColor", outlineColor);
            mat.SetFloat("_OutlineWidth", outlineWidth);
        }
        else
        {
            // Disable the outline pass
            mat.SetFloat("_Highlight", 0f);
        }
    }
}