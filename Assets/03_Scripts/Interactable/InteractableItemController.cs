using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class InteractableItemController : MonoBehaviour, IInteractable, IHighlightable
{
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Interact()
    {
        Debug.Log($"{name} was interacted with!");
    }

    public void Highlight(bool state)
    {
        Debug.Log("triggering Highlight");
        mat.SetFloat("_OutlineWidth", state ? 0.05f : 0f);
        mat.SetColor("_OutlineColor", state ? Color.yellow : Color.clear);
    }

}
