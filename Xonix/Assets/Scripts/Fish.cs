using UnityEngine;

/// <summary>
/// White movable hostile tile on a black area
/// </summary>

public class Fish : BaseCreature
{
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        xonix = FindObjectOfType<Xonix>();

        SetUpPosition(Random.Range(0 + mapManager.GetOffset(), mapManager.GetWidth() - mapManager.GetOffset()),
                      Random.Range(0 + mapManager.GetOffset(), mapManager.GetHeight() - mapManager.GetOffset()));

        GetComponent<SpriteRenderer>().color = Color.white;

        vectorDirection = new Vector3(-1.0f, -1.0f);
    }

    public override void MovementImplementation(int x, int y, ref int dx, ref int dy)
    {
        if (MapManager.Map[x + dx, y].GetColor() == Color.cyan) dx = -dx;
        if (MapManager.Map[x, y + dy].GetColor() == Color.cyan) dy = -dy;
    }

}
