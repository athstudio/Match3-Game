using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMathches()
    {
        //currentMatches.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currenGem = board.allGems[x,y];
                if(currenGem != null)
                {
                    if(x > 0 && x < board.width - 1)
                    {
                        Gem leftGem = board.allGems[x - 1,y];
                        Gem rightGem = board.allGems[x + 1,y];
                        if(leftGem != null && rightGem !=null)
                        {
                            if(leftGem.type == currenGem.type && rightGem.type == currenGem.type)
                            {
                                currenGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currenGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }
                    if(y > 0 && y < board.height - 1)
                    {
                        Gem topGem = board.allGems[x,y + 1];
                        Gem bottomGem = board.allGems[x,y - 1];
                        if(topGem != null && bottomGem !=null)
                        {
                            if(topGem.type == currenGem.type && bottomGem.type == currenGem.type)
                            {
                                currenGem.isMatched = true;
                                topGem.isMatched = true;
                                bottomGem.isMatched = true;

                                currentMatches.Add(currenGem);
                                currentMatches.Add(topGem);
                                currentMatches.Add(bottomGem);
                            }
                        }
                    }
                }
            }
        } 
            if(currentMatches.Count > 0)
            {
                currentMatches = currentMatches.Distinct().ToList();
            }

    }
}
