using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Start point of a game
/// </summary>

public class GameManager : MonoBehaviour
{
    public MapManager mapManager;
    public UIManager UI;

    public Xonix xonix;
    public Fish fish;
    public Dog dog;

    public static List<Fish> fishes;
    public static List<Dog> dogs;

    private float timer = 60;

    private bool isGameOver;
    private bool isStart;

    public int waterAreaPercentToNewLevel = 75;
    public int enemiesAmount = 1;

    public void Awake()
    {
        fishes = new List<Fish>();
        dogs = new List<Dog>();
    }

    public void StartGame()
    {
        if (!string.IsNullOrEmpty(UI.enemiesAmountInputField.text) 
            && !string.IsNullOrEmpty(UI.fillAreaInputField.text))
        {
            mapManager.Initialize();

            enemiesAmount = int.Parse(UI.enemiesAmountInputField.text);
            waterAreaPercentToNewLevel = int.Parse(UI.fillAreaInputField.text);

            xonix = Instantiate(xonix);

            for (int i = 0; i < enemiesAmount; i++)
            {
                var _fish = Instantiate(fish);
                fishes.Add(_fish);

                var _dog = Instantiate(dog);
                dogs.Add(_dog);
            }

            isStart = true;

            UI.startScreen.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isGameOver && isStart)
        {
            if (mapManager.GetWaterAreaPerCent() > waterAreaPercentToNewLevel)
            {
                NewLevel();
                timer = 60;
                mapManager.ResetWaterArea();
            }

            TimeCount();
            MoveEnemies();
            MoveXonix();

            if (xonix.isSelfCross)
            {
                Restart();
                mapManager.ResetWaterArea();
                xonix.isSelfCross = false;
            }
        }
    }

    private void InputDirection()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) xonix.SetUpDirection(Direction.LEFT);
        if (Input.GetKeyDown(KeyCode.RightArrow)) xonix.SetUpDirection(Direction.RIGHT);
        if (Input.GetKeyDown(KeyCode.UpArrow)) xonix.SetUpDirection(Direction.UP);
        if (Input.GetKeyDown(KeyCode.DownArrow)) xonix.SetUpDirection(Direction.DOWN);
        if (Input.GetKeyDown(KeyCode.Space)) xonix.SetUpDirection(Direction.STOP);
    }
    private void MoveXonix()
    {
        InputDirection();
        xonix.PaintTrace();
        xonix.Move();
    }

    private void MoveEnemies()
    {
        foreach (Fish fish in fishes)
        {
            fish.Move();
        }

        foreach (Dog dog in dogs)
        {
            dog.Move();
        }
    }

    public void Restart()
    {
        mapManager.RebuildMap();

        xonix.SetUpPosition(50, 5);
        xonix.SetUpDirection(Direction.STOP);

        foreach (Dog dog in dogs)
        {
            dog.SetUpPosition(Random.Range(3, mapManager.GetWidth() - 3), 1);
        }

        UI.SetLivesText(xonix.GetLives());

        timer = 60;

        if (xonix.GetLives() == 0) GameOver();
    }

    public void NewLevel()
    {
        Restart();

        var _fish = Instantiate(fish);
        var _dog = Instantiate(dog);

        foreach (Dog dog in dogs)
        {
            dog.SetUpPosition(Random.Range(3, mapManager.GetWidth() - 3), 1);
        }

        fishes.Add(_fish);
        dogs.Add(_dog);
    }

    private void GameOver()
    {
        isGameOver = true;
        UI.SetActiveGameOver();
    }

    private void TimeCount()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UI.SetTimerText((int)Mathf.Round(timer));
        }
        else
        {
            NewLevel();
            timer = 60;
            UI.SetTimerText((int)Mathf.Round(timer));
        }
    }

}
