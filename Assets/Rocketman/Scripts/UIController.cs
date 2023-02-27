using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static LevelAPI;

[RequireComponent(typeof(CameraController))]
public class UIController : MonoBehaviour, IUIController
{
    private Action<Vector2> onMoveListener;
    private Action onIdleListener;
    private Action onPressUltimateListener;
    private Action<float> onUpdateHealthListener;
    private Action<float> onUpdateScoreListener;
    private Action onPauseListener;
    private Action onResumeListener;
    private Action onRetryListener;
    private Action onGameOverListener;
    private Action<string, Dictionary<string, Action<float>>> onAddPowerupListener;
    private Camera cam;

    private bool moved;

    private ICamera cameraController;
    private IJoystick joystickController;
    private Action onUltimateAvailableListener;

    public ICamera Camera => GetCamera();

    private void Awake()
    {
        cam = UnityEngine.Camera.main;
        cameraController = GetComponent<CameraController>();
        joystickController = GetComponent<JoystickController>();
        joystickController.Init(cameraController);
    }

    private void Update()
    {
        moved = false;

        bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();

        if (Input.GetMouseButtonDown(0) && !mouseOverUI)
        {
            joystickController.Begin(cam.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0))
        {
            if(joystickController.IsActive()) Move(joystickController.GetAxis(cam.ScreenToWorldPoint(Input.mousePosition)));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            joystickController.End();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(0))
            {
                joystickController.Begin(cam.ScreenToWorldPoint(touch.position));
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                joystickController.End();
            }
            else
            {
                Move(joystickController.GetAxis(cam.ScreenToWorldPoint(touch.position)));
            }
        }

        if(moved == false) {
            Idle();
        }
    }
    internal void PressUltimate()
    {
        onPressUltimateListener.Invoke();
    }
    private void Move(Vector2 axis)
    {
        onMoveListener.Invoke(axis);
        moved = true;
    }
    private void Idle()
    {
        onIdleListener.Invoke();
    }
    public void SetOnPressUltimateListener(Action onPressUltimate)
    {
        onPressUltimateListener = onPressUltimate;
    }

    public void SetOnMoveListener(Action<Vector2> onTouch)
    {
        onMoveListener = onTouch;
    }
    public void SetOnIdleListener(Action onNoTouch)
    {
        onIdleListener = onNoTouch;
    }
    public void UpdateHealth(float healthRatio)
    {
        onUpdateHealthListener?.Invoke(healthRatio);
    }
    public void SetOnUpdateHealthListener(Action<float> onUpdateHealth)
    {
        onUpdateHealthListener = onUpdateHealth;
    }

    public void UpdateScore(float score)
    {
        onUpdateScoreListener?.Invoke(score);
    }

    internal void SetOnUpdateScoreListener(Action<float> onUpdateScore)
    {
        onUpdateScoreListener = onUpdateScore;
    }

    public void SetOnPauseListener(Action onPause)
    {
        onPauseListener = onPause;
    }

    public void SetOnResumeListener(Action onResume)
    {
        onResumeListener = onResume;
    }

    public void SetOnRetryListener(Action onRetry)
    {
        onRetryListener = onRetry;
    }

    internal void Pause()
    {
        onPauseListener?.Invoke();
    }

    internal void Resume()
    {
        onResumeListener?.Invoke();
    }

    internal void Retry()
    {
        onRetryListener?.Invoke();
    }

    internal void SetOnGameOverListener(Action onGameOver)
    {
        onGameOverListener = onGameOver; 
    }

    public void GameOver()
    {
        onGameOverListener?.Invoke();
    }

    internal void SetOnAddPowerupListener(Action<string, Dictionary<string, Action<float>>> onAddPowerup)
    {
        onAddPowerupListener = onAddPowerup;
    }

    public void AddPowerup(string powerup, Dictionary<string, Action<float>> listeners)
    {
        onAddPowerupListener?.Invoke(powerup, listeners);
    }

    private ICamera GetCamera()
    {
        return cameraController;
    }

    internal void SetOnUltimateAvailable(Action onUltimateAvailable)
    {
        onUltimateAvailableListener = onUltimateAvailable;
    }

    public void SetUltimateAvailable()
    {
        onUltimateAvailableListener?.Invoke();
    }
}
