using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelAPI;

public class PowerupController
{   
    private Dictionary<string, Action<float>> powerupListeners;
    private Dictionary<string, Powerup> powerupDict;

    public class Powerup
    {
        internal float duration;
        internal float timer;
        internal Action onActivate;
        internal Action onDeactivate;
        internal bool inActive;

        public Powerup(float duration, Action onActivate, Action onDeactivate)
        {
            this.duration = duration;
            timer = duration;
            this.onActivate = onActivate;
            onActivate?.Invoke();
            this.onDeactivate = onDeactivate;
        }
        internal void Update(Action<float> listener)
        {
            if (inActive) return;
            timer -= Time.deltaTime;
            listener?.Invoke(timer / duration);
            if(timer <= 0)
            {
                inActive = true;
                onDeactivate?.Invoke();
            }
        }

        internal void Reset()
        {
            if (inActive)
            {
                inActive = false;
                onActivate?.Invoke();
            }

            timer = duration;
        }
    }

    public PowerupController()
    {
        powerupListeners = new Dictionary<string, Action<float>>();
        powerupDict = new Dictionary<string, Powerup>();
    }
    
    internal void Update()
    {
        powerupDict.Keys.ToList().ForEach(powerupName =>
        {
            powerupDict[powerupName].Update(powerupListeners.ContainsKey(powerupName) ? powerupListeners[powerupName] : null);
        });
    }

    internal void AddPowerUp(string name, IPlayer player, IUIController uIController)
    {
        if (powerupDict.ContainsKey(name))
        {
            powerupDict[name].Reset();
        }
        else
        {
            Action onActivate = null;
            Action onDeactivate = null;

            switch (name)
            {
                case "MovSpd":
                    onActivate = () => player.SetMoveSpeedMult(2f);
                    onDeactivate = () => player.SetMoveSpeedMult(1f);
                    break;
                case "AtkSpd":
                    onActivate = () => player.SetAttackSpeedMult(2f);
                    onDeactivate = () => player.SetAttackSpeedMult(1f);
                    break;
                default:
                    break;
            }

            powerupDict[name] = new Powerup(5f, onActivate, onDeactivate);
        }

        uIController.AddPowerup(name, powerupListeners);
    }
}
