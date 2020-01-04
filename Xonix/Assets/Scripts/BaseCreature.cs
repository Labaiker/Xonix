using System.Collections;
using UnityEngine;

/// <summary>
/// Base class of behavior for all creatures
/// </summary>


public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
    STOP
}


public abstract class BaseCreature : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected Vector2 position;

    protected Vector3 vectorDirection;

    protected MapManager mapManager;

    protected Xonix xonix;

    protected bool isWait = true;

    protected Coroutine moveRoutine;

    public void SetUpPosition(int x, int y)
    {
        position.x = x;
        position.y = y;

        transform.position = position;
    }

    public Vector2 GetPosition() => transform.position;

    public virtual void Move()
    {
        if (isWait) moveRoutine = StartCoroutine(MoveRoutine());
        CheckForHit();
    }

    public virtual IEnumerator MoveRoutine()
    {
        isWait = false;

        yield return new WaitForSeconds(speed);

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int dx = (int)vectorDirection.x;
        int dy = (int)vectorDirection.y;

        MovementImplementation(x, y, ref dx, ref dy);

        vectorDirection.x = dx;
        vectorDirection.y = dy;

        transform.position += vectorDirection;

        isWait = true;

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
    }

    /// <summary>
    /// Essential logic for movement behavior of derivative classes
    /// </summary>
    /// <param name="x"> vector x position </param>
    /// <param name="y"> vector y position </param>
    /// <param name="dx"> vector x direction </param>
    /// <param name="dy"> vector y direction </param>
    public virtual void MovementImplementation(int x, int y, ref int dx, ref int dy) { }

    public virtual void CheckForHit()
    {
        if (xonix != null)
        {
            if (transform.position == xonix.transform.position)
            {
                xonix.TakeLives();
                xonix.isSelfCross = true;
            }
        }

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        if (MapManager.Map[x, y].GetColor() == Color.magenta)
        {
            if (xonix != null)
            {
                xonix.TakeLives();
                xonix.isSelfCross = true;
            }
        }
    }

}
