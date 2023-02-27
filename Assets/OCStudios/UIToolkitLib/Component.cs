using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Component
{
    public VisualElement visualElement;

    public List<EventCallback<ClickEvent>> clickEvents;
    public List<EventCallback<ChangeEvent<string>>> stringChangeEvents;
    public List<EventCallback<FocusInEvent>> focusInEvents;
    public List<EventCallback<FocusOutEvent>> focusOutEvents;
    public List<EventCallback<ChangeEvent<bool>>> boolChangeEvents;
    public List<EventCallback<ChangeEvent<float>>> floatChangeEvents;
    public List<EventCallback<ChangeEvent<Vector2>>> vector2ChangeEvents;
    public List<EventCallback<MouseEnterEvent>> mouseEnterEvents;
    public List<EventCallback<MouseLeaveEvent>> mouseLeaveEvents;

    public Component(VisualElement visualElement)
    {
        this.visualElement = visualElement;

        clickEvents = new List<EventCallback<ClickEvent>>();
        stringChangeEvents = new List<EventCallback<ChangeEvent<string>>>();
        focusInEvents = new List<EventCallback<FocusInEvent>>();
        focusOutEvents = new List<EventCallback<FocusOutEvent>>();
        boolChangeEvents = new List<EventCallback<ChangeEvent<bool>>>();
        floatChangeEvents = new List<EventCallback<ChangeEvent<float>>>();
        vector2ChangeEvents = new List<EventCallback<ChangeEvent<Vector2>>>();
        mouseEnterEvents = new List<EventCallback<MouseEnterEvent>>();
        mouseLeaveEvents = new List<EventCallback<MouseLeaveEvent>>();
    }

    public Component Init(Action<Component> onInit)
    {
        onInit(this);
        return this;
    }

    public Component AddClickEvent(EventCallback<ClickEvent> clickEvent)
    {
        clickEvents.Add(clickEvent);
        return this;
    }

    public Component AddStringChangeEvent(EventCallback<ChangeEvent<string>> stringChangeEvent)
    {
        stringChangeEvents.Add(stringChangeEvent);
        return this;
    }

    public Component AddFocusInEvent(EventCallback<FocusInEvent> focusInEvent)
    {
        focusInEvents.Add(focusInEvent);
        return this;
    }

    public Component AddFocusOutEvent(EventCallback<FocusOutEvent> focusOutEvent)
    {
        focusOutEvents.Add(focusOutEvent);
        return this;
    }

    public Component AddBoolChangeEvent(EventCallback<ChangeEvent<bool>> boolChangeEvent)
    {
        boolChangeEvents.Add(boolChangeEvent);
        return this;
    }

    public Component AddFloatChangeEvent(EventCallback<ChangeEvent<float>> floatChangeEvent)
    {
        floatChangeEvents.Add(floatChangeEvent);
        return this;
    }

    public Component AddVector2ChangeEvent(EventCallback<ChangeEvent<Vector2>> vector2ChangeEvent)
    {
        vector2ChangeEvents.Add(vector2ChangeEvent);
        return this;
    }

    internal Component AddMouseEnterEvent(EventCallback<MouseEnterEvent> mouseEnterEvent)
    {
        mouseEnterEvents.Add(mouseEnterEvent);
        return this;
    }

    internal Component AddMouseLeaveEvent(EventCallback<MouseLeaveEvent> mouseLeaveEvent)
    {
        mouseLeaveEvents.Add(mouseLeaveEvent);
        return this;
    }

    public void RegisterCallbacks()
    {
        foreach (EventCallback<ClickEvent> clickEvent in clickEvents)
            visualElement.RegisterCallback(clickEvent);

        foreach (EventCallback<ChangeEvent<string>> stringChangeEvent in stringChangeEvents)
            visualElement.RegisterCallback(stringChangeEvent);

        foreach (EventCallback<FocusInEvent> focusInEvent in focusInEvents)
            visualElement.RegisterCallback(focusInEvent);

        foreach (EventCallback<FocusOutEvent> focusOutEvent in focusOutEvents)
            visualElement.RegisterCallback(focusOutEvent);

        foreach (EventCallback<ChangeEvent<bool>> boolChangeEvent in boolChangeEvents)
            (visualElement as INotifyValueChanged<bool>).RegisterValueChangedCallback(boolChangeEvent);

        foreach (EventCallback<ChangeEvent<float>> floatChangeEvent in floatChangeEvents)
            (visualElement as INotifyValueChanged<float>).RegisterValueChangedCallback(floatChangeEvent);

        foreach (EventCallback<ChangeEvent<Vector2>> vector2ChangeEvent in vector2ChangeEvents)
            (visualElement as INotifyValueChanged<Vector2>).RegisterValueChangedCallback(vector2ChangeEvent);

        foreach (EventCallback<MouseEnterEvent> mouseEnterEvent in mouseEnterEvents)
            visualElement.RegisterCallback(mouseEnterEvent);

        foreach (EventCallback<MouseLeaveEvent> mouseLeaveEvent in mouseLeaveEvents)
            visualElement.RegisterCallback(mouseLeaveEvent);
    }

    public void UnregisterCallbacks()
    {
        foreach (EventCallback<ClickEvent> clickEvent in clickEvents)
            visualElement.UnregisterCallback(clickEvent);

        foreach (EventCallback<ChangeEvent<string>> stringChangeEvent in stringChangeEvents)
            visualElement.UnregisterCallback(stringChangeEvent);

        foreach (EventCallback<FocusInEvent> focusInEvent in focusInEvents)
            visualElement.UnregisterCallback(focusInEvent);

        foreach (EventCallback<FocusOutEvent> focusOutEvent in focusOutEvents)
            visualElement.UnregisterCallback(focusOutEvent);

        foreach (EventCallback<ChangeEvent<bool>> boolChangeEvent in boolChangeEvents)
            (visualElement as INotifyValueChanged<bool>).UnregisterValueChangedCallback(boolChangeEvent);

        foreach (EventCallback<ChangeEvent<float>> floatChangeEvent in floatChangeEvents)
            (visualElement as INotifyValueChanged<float>).UnregisterValueChangedCallback(floatChangeEvent);

        foreach (EventCallback<ChangeEvent<Vector2>> vector2ChangeEvent in vector2ChangeEvents)
            (visualElement as INotifyValueChanged<Vector2>).UnregisterValueChangedCallback(vector2ChangeEvent);

        foreach (EventCallback<MouseEnterEvent> mouseEnterEvent in mouseEnterEvents)
            visualElement.UnregisterCallback(mouseEnterEvent);

        foreach (EventCallback<MouseLeaveEvent> mouseLeaveEvent in mouseLeaveEvents)
            visualElement.UnregisterCallback(mouseLeaveEvent);
    }
}
