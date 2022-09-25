using UnityEngine;
using UnityEngine.UIElements;

public abstract class Screen : MonoBehaviour
{
    protected VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        Setup();
        SetShown(false);
    }

    public void SetShown(bool shown) => root.visible = shown;

    protected abstract void Setup();
    
}