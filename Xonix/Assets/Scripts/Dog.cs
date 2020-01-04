using UnityEngine;

/// <summary>
/// Black movable hostile tile on a cyan area
/// </summary>

public class Dog : BaseCreature
{
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        xonix = FindObjectOfType<Xonix>();

        SetUpPosition(Random.Range(3, mapManager.GetWidth() - 3), 1);
        GetComponent<SpriteRenderer>().color = Color.black;

        vectorDirection = new Vector3(1.0f, 1.0f);
    }

    public override void MovementImplementation(int x, int y, ref int dx, ref int dy)
    {
        if (transform.position.x >= mapManager.GetWidth() - 1) dx = -dx;
        if (transform.position.x <= 0.0f) dx = -dx;
        if (transform.position.y >= mapManager.GetHeight() - 1) dy = -dy;
        if (transform.position.y <= 0.0f) dy = -dy;

        if (MapManager.Map[x + dx, y].GetColor() == Color.black) dx = -dx;
        if (MapManager.Map[x, y + dy].GetColor() == Color.black) dy = -dy;
    }
}
