using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public abstract class View : MonoBehaviour
{
    internal new string name;

    internal Presenter presenter;

    internal VisualElement root;

    private List<Component> visualComponents;

    bool init = false;

    internal void Initialize()
    {
        Init();
        visualComponents = new List<Component>();

        init = true;
    }

    internal abstract void Init();

    internal void SetName(string name)
    {
        this.name = name;
    }

    private void OnEnable()
    {
        if (!init) return;

        root = GetComponent<UIDocument>().rootVisualElement;
        StyleSheet styleSheet = Resources.Load<StyleSheet>("CustomCSS");
        if (styleSheet != null) root.parent.styleSheets.Add(styleSheet);
        else Debug.Log("CustomCSS not found");
        PreEnable();

        visualComponents.ForEach(component => component.RegisterCallbacks());
    }

    internal abstract void PreEnable();

    internal void AddComponent(Component component)
    {
        visualComponents.Add(component);
    }

    internal void RemoveComponent(Component component)
    {
        visualComponents.Remove(component);
        component.UnregisterCallbacks();
    }

    internal void AddComponents(List<Component> components)
    {
        visualComponents.AddRange(components);
    }

    internal void RemoveComponents(List<Component> components)
    {
        visualComponents.RemoveAll(component => components.Contains(component));
        components.ForEach(component => component.UnregisterCallbacks());
    }

    private void OnDisable()
    {
        visualComponents.ForEach(component => component.UnregisterCallbacks());
    }

    internal void SetActive(bool toActivate)
    {
        gameObject.SetActive(toActivate);
    }

    internal virtual void OnReEnable() { }
}
