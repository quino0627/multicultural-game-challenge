using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DragHandlerScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static int quizsize;

    public static GameObject itemBeingDragged;
    public static int puzzleNumber;
    private string puzzleName;
    public static Transform startParent;
    Vector3 startPosition;
    public static string questionName, slotName;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        puzzleName = itemBeingDragged.transform.parent.name.Remove(0, itemBeingDragged.transform.parent.name.Length - 1);
        puzzleNumber = Int32.Parse(puzzleName);

        questionName = itemBeingDragged.transform.parent.name.Remove(itemBeingDragged.transform.parent.name.Length - 1);

        gameObject.transform.SetParent(gameObject.transform.parent.parent.parent.parent.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        if (transform.parent.name == "Canvas")
        { 
            transform.position = startPosition;
            gameObject.transform.SetParent(startParent);
        }
    }
}