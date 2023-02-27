using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class LevelView : View
{
    private GamePresenter gamePresenter;

    private ButtonThatCanBeDisabled pauseButton;
    private Dictionary<string, VisualElement> powerupItems;
    private VisualTreeAsset powerupItem;
    private ButtonThatCanBeDisabled ultimateButton;

    internal override void Init()
    {
        name = "levelView";
        gamePresenter = presenter as GamePresenter;
        powerupItems = new Dictionary<string, VisualElement>();
        powerupItem = Resources.Load<VisualTreeAsset>("UIItems/powerup-item");
    }

    internal override void PreEnable()
    {
        VisualElement healthBar = root.Q<VisualElement>("healthbar_bar");
        Label scoreLabel = root.Q<Label>("label-score");

        gamePresenter.SetOnUpdateHealthListener((float healthRatio) =>
        {
            healthBar.style.scale = new Scale(new Vector3(healthRatio, 1, 1));
        });

        gamePresenter.SetOnUpdateScoreListener((float score) =>
        {
            scoreLabel.text = "Score: " + Mathf.FloorToInt(score);
        });

        gamePresenter.SetOnAddPowerupListener(OnAddPowerup);

        AddComponents(new List<Component>
        {
            root.Q<ButtonThatCanBeDisabled>("button-pause").ToComponent().Init((comp) =>
            {
                pauseButton = comp.visualElement as ButtonThatCanBeDisabled;
                pauseButton.enabled = true;
            }).AddClickEvent(new EventCallback<ClickEvent>(ev =>
            {
                gamePresenter.Pause();
                pauseButton.enabled = false;
            })),
            root.Q<ButtonThatCanBeDisabled>("button-ultimate").ToComponent().Init((comp) =>
            {
                ultimateButton = comp.visualElement as ButtonThatCanBeDisabled;
                ultimateButton.enabled = true;
            }).AddClickEvent(new EventCallback<ClickEvent> (ev =>
            {
                gamePresenter.PressUltimate();
                ultimateButton.enabled = false;
            }))
        });

        gamePresenter.SetOnUltimateAvailable(() =>
        {
            ultimateButton.enabled = true;
        });
    }

    internal override void OnReEnable()
    {
        pauseButton.enabled = true;
    }

    private void OnAddPowerup(string powerup, Dictionary<string, Action<float>> listeners)
    {
        if (!powerupItems.ContainsKey(powerup))
        {
            powerupItems[powerup] = powerupItem.Instantiate();
            powerupItems[powerup].Q<Label>("powerup-label").text = powerup;
            powerupItems[powerup].Q<VisualElement>("powerup-bar").style.scale = new Scale(new Vector3(1, 1, 1));
            root.Q<VisualElement>("sidebar-left").Add(powerupItems[powerup]);
        }

        if(!listeners.ContainsKey(powerup)) listeners.Add(powerup, (float scale) =>
        {
            powerupItems[powerup].Q<VisualElement>("powerup-bar").style.scale = new Scale(new Vector3(scale, 1, 1));
            if(scale <= 0)
            {
                root.Q<VisualElement>("sidebar-left").Remove(powerupItems[powerup]);
                powerupItems.Remove(powerup);
                listeners.Remove(powerup);
            }
        });
    }
}
