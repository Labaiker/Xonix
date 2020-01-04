using UnityEngine;

/// <summary>
///  Represent pixel of a game board 
/// </summary>

public class Tile : MonoBehaviour
{
    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
    public Color GetColor() => GetComponent<SpriteRenderer>().color;

}
