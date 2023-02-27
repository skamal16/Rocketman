using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePresenter : Presenter
{
    public UIController uiController;

    internal void Exit()
    {
        Application.Quit();
    }

    internal override void Init()
    {
        name = "gamePresenter";
    }

    internal void Pause()
    {
        uiController.Pause();
        OpenPopup("menuView");
    }

    internal override void PostInit()
    {
        GoToView("levelView");
        uiController.SetOnGameOverListener(() => OpenPopup("gameOverView"));
    }

    internal void Resume()
    {
        uiController.Resume();
        ClosePopup();
    }

    internal void Retry()
    {
        uiController.Retry();
        ClosePopup();
    }

    internal void SetOnUpdateHealthListener(Action<float> onUpdateHealth)
    {
        uiController.SetOnUpdateHealthListener(onUpdateHealth);
    }

    internal void SetOnUpdateScoreListener(Action<float> onUpdateScore)
    {
        uiController.SetOnUpdateScoreListener(onUpdateScore);
    }

    internal void SetOnAddPowerupListener(Action<string, Dictionary<string, Action<float>>> onAddPowerup)
    {
        uiController.SetOnAddPowerupListener(onAddPowerup);
    }

    internal void PressUltimate()
    {
        uiController.PressUltimate();
    }

    internal void SetOnUltimateAvailable(Action onUltimateAvailable)
    {
        uiController.SetOnUltimateAvailable(onUltimateAvailable);
    }
}
