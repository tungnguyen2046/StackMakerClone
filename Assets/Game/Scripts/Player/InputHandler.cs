using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public readonly int threshhold = 125;

    public static Action<Direction> OnSwipe; //0: Forward, 1: Back, 2: Left, 3: Right

    public Direction Direction { get { return direction; } }

    private Direction direction = Direction.None;
    private Direction lastDirection = Direction.None;

    private Vector2 touchPos;
    private Vector2 touchDis;

    private bool isDragging;
    private float touchX, touchY;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            touchPos = Vector2.zero;
            touchDis = Vector2.zero;
        }

        touchDis = Vector2.zero;
        if (isDragging)
        {
            if (Input.GetMouseButton(0))
            {
                touchDis = (Vector2)Input.mousePosition - touchPos;
            }
        }

        if (touchDis.magnitude > threshhold)
        {
            touchX = touchDis.x;
            touchY = touchDis.y;

            if (Mathf.Abs(touchX) > Mathf.Abs(touchY))    //Vuốt trái hoặc phải
            {
                if (touchX < 0)
                {
                    UpdateDirection(Direction.Left);
                    //Debug.Log("Left");
                }
                else
                {
                    UpdateDirection(Direction.Right);
                    //Debug.Log("Right");
                }
            }
            else  //Vuốt lên hoặc xuống
            {
                if (touchY < 0)
                {
                    UpdateDirection(Direction.Back);
                    //Debug.Log("Back");
                }
                else
                {
                    UpdateDirection(Direction.Forward);
                    //Debug.Log("Forward");
                }
            }
        }
    }

    void UpdateDirection(Direction dir)
    {
        if (direction != dir)
        {
            lastDirection = direction;
            direction = dir;
            OnSwipe(direction);
        }
    }
}

public enum Direction
{
    Forward,
    Back,
    Left,
    Right, 
    None
}
