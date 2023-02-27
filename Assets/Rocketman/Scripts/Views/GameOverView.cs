using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverView : View
{
    private GamePresenter gamePresenter;
    internal override void Init()
    {
        name = "gameOverView";
        gamePresenter = presenter as GamePresenter;
    }

    internal override void PreEnable()
    {
        AddComponents(new List<Component>()
        {
            root.Q<Button>("button-retry").ToComponent().AddClickEvent(new EventCallback<ClickEvent>(ev =>
            {
                gamePresenter.Retry();
            })),
            root.Q<Button>("button-exit").ToComponent().AddClickEvent(new EventCallback<ClickEvent>(ev =>
            {
                gamePresenter.Exit();
            }))
        });
    }
}
