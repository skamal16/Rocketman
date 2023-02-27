using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelAPI;

public class GameController : MonoBehaviour
{
    public LevelConfig levelConfig;
    public UIController uiController;

    private LevelController levelController;

    void Awake()
    {
        Application.targetFrameRate = 60;

        levelController = new LevelController();
        levelController.Init(uiController, levelConfig);
    }

    // Start is called before the first frame update
    void Start()
    {
        levelController.Start();
    }

    // Update is called once per frame
    void Update()
    {
        levelController.Update();
    }

    void FixedUpdate()
    {
        levelController.FixedUpdate();
    }
}
