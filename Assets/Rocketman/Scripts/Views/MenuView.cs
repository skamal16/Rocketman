using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuView : View
{
    private GamePresenter gamePresenter;
    internal override void Init()
    {
        name = "menuView";
        gamePresenter = presenter as GamePresenter;
    }

    internal override void PreEnable()
    {
        AddComponents(new List<Component>()
        {
            root.Q<Button>("button-resume").ToComponent().AddClickEvent(new EventCallback<ClickEvent>(ev =>
            {
                gamePresenter.Resume();
            })),
            root.Q<Button>("button-exit").ToComponent().AddClickEvent(new EventCallback<ClickEvent>(ev =>
            {
                gamePresenter.Exit();
            }))
        });
    }
}
