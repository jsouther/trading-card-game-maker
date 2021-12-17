using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    private GameEngine engine;
    private GameObject PlayArea;
    private bool isDragging = false;
    private bool isOverPlayArea = false;
    private Vector2 startArea;
    private int cardIndex;

    // Start is called before the first frame update
    void Start()
    {
 //       engine = gameObject.AddComponent(typeof(GameEngine)) as GameEngine;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {

            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        isOverPlayArea = true;
        PlayArea = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverPlayArea = false;
        PlayArea = null;
    }

    public void startDragging()
    {
        startArea = transform.position;
        isDragging = true;
    }
    public void stopDragging(PointerEventData eventData)
    {
        isDragging = false;
        if (isOverPlayArea )
        {
            transform.SetParent(PlayArea.transform, false);
        }
        else
        {
            transform.position = startArea;
        }
    }
}
