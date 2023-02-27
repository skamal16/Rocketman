using System;
using UnityEngine;
using static LevelAPI;

public class JoystickController : MonoBehaviour, IJoystick
{
    private ICamera cameraController;

    [SerializeField] private SpriteRenderer joystick;
    [SerializeField] private SpriteRenderer stick;

    private float radius;

    private Vector2 cameraVector;
    private bool joystickActive;

    private void Start()
    {
        radius = stick.transform.localScale.x;
    }

    public void End()
    {
        if (!joystickActive) return;
        joystick.gameObject.SetActive(false);
        stick.transform.localPosition = Vector3.zero;
        joystickActive = false;
    }

    public Vector2 GetAxis(Vector3 position)
    {
        if (!joystickActive) throw new Exception("joystick not active");

        joystick.transform.position = cameraController.GetPosition() + cameraVector;

        Vector2 diff = position - joystick.transform.position;

        Vector2 axis = diff.magnitude > 1 ? diff.normalized : diff;
        stick.transform.localPosition = axis * radius;

        return axis;
    }

    public void Begin(Vector2 position)
    {
        joystickActive = true;
        joystick.transform.position = position;
        cameraVector = position - cameraController.GetPosition();
        joystick.gameObject.SetActive(true);
    }

    public void Init(ICamera cameraController)
    {
        this.cameraController = cameraController;
    }

    public bool IsActive()
    {
        return joystickActive;
    }
}
