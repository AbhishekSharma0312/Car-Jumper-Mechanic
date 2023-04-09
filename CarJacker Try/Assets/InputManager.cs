using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SwipeDirection
{
    None,
    Left,
    Right,
    Front,
    Back
}

public enum PlayerLane
{
    LeftLane,
    MidLane,
    RightLane
}

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    SwipeDirection swipeDirection;
    PlayerLane playerLane;

    float swipeThreshold = 100f;
    Vector2 startPos, endPos;
    bool draggingStarted = false;

    private void Start()
    {

        swipeDirection = SwipeDirection.None;
        playerLane = PlayerLane.MidLane;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingStarted = true;
        startPos = eventData.pressPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingStarted)
        {
            endPos = eventData.position;

            var difference = endPos - startPos;
            if (difference.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
                {
                    swipeDirection = difference.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                }
                else if (Mathf.Abs(difference.y) > Mathf.Abs(difference.x))
                {
                    swipeDirection = difference.y > 0 ? SwipeDirection.Front : SwipeDirection.Back;
                }
            }
            else
            {
                swipeDirection = SwipeDirection.None;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingStarted && swipeDirection != SwipeDirection.None)
        {
            if (swipeDirection == SwipeDirection.Right && playerLane != PlayerLane.RightLane)
            {
                playerLane = playerLane + 1;
            }
            else if (swipeDirection == SwipeDirection.Left && playerLane != PlayerLane.LeftLane)
            {
                playerLane = playerLane - 1;
            }
            playerScript._instance.FindClosestCar(swipeDirection, playerLane);
        }

        startPos = Vector2.zero;
        endPos = Vector2.zero;
        draggingStarted = false;
    }
}
