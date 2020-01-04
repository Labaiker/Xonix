using UnityEngine;

/// <summary>
/// Class for building and maintaining map
/// </summary>

public class MapManager : MonoBehaviour
{
    private const int width = 100;
    private const int height = 60;
    private const int offset = 5;
    private const int WaterArea = (width - offset) * (height - offset);

    private const int black = 0;
    private const int cyan = 1;
    private const int magenta = 2;
    private const int white = 3;

    public static Tile[,] Map;

    private int[,] _map;

    [SerializeField]
    private GameObject tilePref;
    [SerializeField]
    private Transform parent;

    public UIManager UI;

    private float currentWaterArea;

    private int paintedTiles = 0;

    public void Initialize()
    {
        Map = new Tile[width, height];
        _map = new int[width, height];

        currentWaterArea = WaterArea;

        BuildMap();
        SetCamera();
        ShowWaterArea();
    }

    private void BuildMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = Instantiate(tilePref, new Vector2(x, y), Quaternion.identity) as GameObject;
                tile.transform.SetParent(parent);
                SetTileColor(x, y, tile);
                Map[x, y] = tile.GetComponent<Tile>();
            }
        }
    }

    private void SetCamera()
    {
        Camera.main.transform.position = new Vector3((width) / 2.0f, (height) / 2.0f, -1.0f);
    }

    private void SetTileColor(int x, int y, GameObject tile)
    {
        if (x > offset && y > offset && x < width - offset && y < height - offset)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            tile.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    private void ShowWaterArea() => UI.SetFillText(GetWaterAreaPerCent());

    public int GetWaterAreaPerCent() => (int)Mathf.Round(100f - currentWaterArea / WaterArea * 100);

    public void ResetWaterArea() 
    {
        currentWaterArea = WaterArea;
        ShowWaterArea();
    } 

    public int GetWidth() => width;

    public int GetHeight() => height;

    public int GetOffset() => offset;

    public void ClearXonixTrace()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (Map[x, y].GetColor() == Color.magenta)
                    Map[x, y].SetColor(Color.black);
    }

    private void ListMapToIntArray()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Map[x, y].GetColor() == Color.black) _map[x, y] = black;
                if (Map[x, y].GetColor() == Color.cyan) _map[x, y] = cyan;
                if (Map[x, y].GetColor() == Color.magenta) _map[x, y] = magenta;
            }
        }
    }

    private void ArrayMapToListMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_map[x, y] == black) Map[x, y].SetColor(Color.black);
                if (_map[x, y] == cyan) Map[x, y].SetColor(Color.cyan);
                if (_map[x, y] == magenta) Map[x, y].SetColor(Color.magenta);
            }
        }
    }

    public void FillTile(int x, int y)
    {
        if (_map[x, y] > black)
            return;

        _map[x, y] = white;

        for (int nx = -1; nx < 2; nx++)
            for (int ny = -1; ny < 2; ny++)
                FillTile(x + nx, y + ny);
    }

    public void FillArea()
    {
        ListMapToIntArray();
        currentWaterArea = 0;

        foreach (var fish in GameManager.fishes)
        {
            FillTile((int)fish.GetPosition().x, (int)fish.GetPosition().y);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_map[x, y] == magenta || _map[x, y] == black)
                {
                    _map[x, y] = cyan;
                    paintedTiles++;
                    UI.SetScoresText(paintedTiles);
                }

                if (_map[x, y] == white)
                {
                    _map[x, y] = black;
                    currentWaterArea++;
                }
            }
        }

        ArrayMapToListMap();
        ShowWaterArea();
    }

    public void RebuildMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SetTileColor(x, y, Map[x, y].gameObject);
            }
        }
    }
}
