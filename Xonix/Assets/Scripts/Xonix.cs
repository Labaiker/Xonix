using System.Collections;
using UnityEngine;

/// <summary>
/// The main character/tile we control during game
/// </summary>

public class Xonix : BaseCreature
{
    private Direction direction;

    private int lives;

    public bool isSelfCross;

    public UIManager UI;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        UI = FindObjectOfType<UIManager>();

        lives = 3;
        UI.SetLivesText(lives);

        SetUpPosition(50, 5);
        SetUpDirection(Direction.STOP);
        GetComponent<SpriteRenderer>().color = Color.white;

    }

    public void SetUpDirection(Direction dir)
    {
        if (dir != direction)
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        direction = dir;
    }

    public int GetLives() => lives;
    public void TakeLives() => lives--;

    public void PaintTrace()
    {
        foreach (Tile tile in MapManager.Map)
        {
            if (Mathf.Round(tile.transform.position.x) == Mathf.Round(transform.position.x)
                && Mathf.Round(tile.transform.position.y) == Mathf.Round(transform.position.y)
                && tile.GetComponent<SpriteRenderer>().color == Color.black)
            {
                tile.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
    }

    public override void Move()
    {
        if (isWait) moveRoutine = StartCoroutine(MoveRoutine());
    }

    public override IEnumerator MoveRoutine()
    {
        int prevX = (int)transform.position.x;
        int prevY = (int)transform.position.y;

        isWait = false;

        yield return new WaitForSeconds(speed);

        if (direction == Direction.LEFT)
            transform.position += Vector3.left;

        if (direction == Direction.RIGHT)
            transform.position += Vector3.right;

        if (direction == Direction.UP)
            transform.position += Vector3.up;

        if (direction == Direction.DOWN)
            transform.position += Vector3.down;


        if (transform.position.x >= mapManager.GetWidth())
            transform.position = new Vector3(mapManager.GetWidth() - 1, transform.position.y);
        if (transform.position.x <= 0.0f)
            transform.position = new Vector3(0.0f, transform.position.y);

        if (transform.position.y >= mapManager.GetHeight())
            transform.position = new Vector3(transform.position.x, mapManager.GetHeight() - 1);
        if (transform.position.y <= 0.0f)
            transform.position = new Vector3(transform.position.x, 0.0f);


        int x = (int)transform.position.x;
        int y = (int)transform.position.y;


        if (MapManager.Map[prevX, prevY].GetColor() == Color.magenta && MapManager.Map[x, y].GetColor() == Color.cyan)
        {
            direction = Direction.STOP;
            mapManager.FillArea();
            mapManager.ClearXonixTrace();
        }
        else if(MapManager.Map[x, y].GetColor() == Color.magenta)
        {
            isSelfCross = true;
            lives--;
            UI.SetLivesText(lives);
        }

        isWait = true;

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
    }

}
