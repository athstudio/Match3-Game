using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;


    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0f;

    private Gem otherGem;

    public enum GemType {blue, green, orange, purple, red}
    public GemType type;

    public bool isMatched;
    
    private Vector2Int previousPos;


    
    void Start()
    {
        
    }

   
    void Update()
    {
        if(Vector2.Distance(transform.position,posIndex) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x,posIndex.y] = this;
        }
        

        if(mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;

            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SwapeAngle();
        }
    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;

        //Debug.Log(firstTouchPosition);
    }

    private void SwapeAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * Mathf.Rad2Deg; //180 / Mathf.PI;

        if(Vector3.Distance(firstTouchPosition, finalTouchPosition) > .5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPos = posIndex;


        if(swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
            
        }
        else if(swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
            
        }
        else if(swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
            
        }
        else if(swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
            
        }

        board.allGems[posIndex.x,posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

        StartCoroutine(CheckMoveGO());
    }

    public IEnumerator CheckMoveGO()
    {
        yield return new WaitForSeconds(.5f);

        board.matchFinder.FindAllMathches();

        if(otherGem != null)
        {
            if(!isMatched && !otherGem.isMatched)
            {
                otherGem.posIndex = posIndex;
                posIndex = previousPos;

                board.allGems[posIndex.x,posIndex.y] = this;
                board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
            }
            else
            {
                board.DestroyMatches();
            }
        }
    }
}
