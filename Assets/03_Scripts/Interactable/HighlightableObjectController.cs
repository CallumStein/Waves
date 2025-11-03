using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HighlightableObjectController : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("Outline Settings")]
    public GameObject outlinePrefab;      // prefab with same mesh + unlit material
    public float outlineScale = 1.05f;    // slightly larger than original
    public LayerMask outlineLayer;        // set to Outline layer

    private GameObject outlineInstance;
    private Renderer originalRenderer;
    private int originalLayer;

    private void Awake()
    {
        originalRenderer = GetComponent<Renderer>();
        originalLayer = gameObject.layer;

        if (outlinePrefab != null)
        {
            // Instantiate the outline as a child
            outlineInstance = Instantiate(outlinePrefab, transform);
            outlineInstance.transform.localPosition = Vector3.zero;
            outlineInstance.transform.localRotation = Quaternion.identity;
            outlineInstance.transform.localScale = Vector3.one * outlineScale;

            // Initially hide it
            outlineInstance.SetActive(false);

            // Set to the Outline camera layer
            outlineInstance.layer = Mathf.RoundToInt(Mathf.Log(outlineLayer.value, 2));
        }
    }

    public void Interact()
    {
        Debug.Log($"{name} interacted with!");
    }

    public void Highlight(bool state)
    {
        if (outlineInstance != null)
        {
            outlineInstance.SetActive(state);
        }
    }
}