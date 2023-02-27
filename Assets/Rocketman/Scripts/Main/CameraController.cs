using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, LevelAPI.ICamera
{
    public float camSpeed;

    private new Camera camera;
    private Vector2 bounds;

    public SpriteRenderer limitDisplay;
    void Awake()
    {
        camera = Camera.main;
        float height = camera.orthographicSize * 2;
        float width = height * camera.aspect;
        bounds = new Vector2(width, height);
    }
    public void MoveTo(Vector2 position, Vector2 verticalBounds)
    {
        Vector3 targetPos = new Vector3(position.x, position.y, camera.transform.position.z);

        float deltaX = Mathf.Abs(targetPos.x - camera.transform.position.x) / (bounds.x * 0.5f);
        float deltaY = Mathf.Abs(targetPos.y - camera.transform.position.y) / (bounds.y * 0.5f);

        //if (Mathf.Abs(targetPos.x - camera.transform.position.x) < bounds.x * 0.5) targetPos.x = camera.transform.position.x;
        //if(Mathf.Abs(targetPos.y - camera.transform.position.y) < bounds.y * 0.5) targetPos.y = camera.transform.position.y;

        camera.transform.position = Vector3.MoveTowards(camera.transform.position, targetPos, new Vector2(deltaX, deltaY).magnitude * camSpeed * Time.deltaTime);

        float lowerBoundDist = Mathf.Abs(position.y - verticalBounds.x);
        float upperBoundDist = Mathf.Abs(position.y - verticalBounds.y);

        float opacity;

        if (lowerBoundDist < upperBoundDist) {
            limitDisplay.transform.position = new Vector3(camera.transform.position.x, verticalBounds.x, 0);

            opacity = Mathf.Clamp(1f - (lowerBoundDist / (bounds.y/4f)), 0f, 1f);
        }
        else {
            limitDisplay.transform.position = new Vector3(camera.transform.position.x, verticalBounds.y, 0);
            opacity = Mathf.Clamp(1f - (upperBoundDist / (bounds.y / 4f)), 0f, 1f);
        }

        Color color = limitDisplay.color;
        limitDisplay.color = new Color(color.r, color.g, color.b, opacity);
    }
    public Vector2 GetBounds()
    {
        return bounds;
    }

    public Vector2 GetPosition()
    {
        return camera.transform.position;
    }
}
