using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Team
{ 
    None,
    Player1,
    Player2,
    Impediments,
}

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject stone_Player1;
    [SerializeField] private GameObject stone_Player2;
    [SerializeField] private GameObject stoneCanPlaced;
    private Board board;
    [SerializeField] private int number;
    [SerializeField] private Team team;
    private bool isCanPlaced = false;

    public Board Board { set { board = value; } }
    public int Number { get { return number; } set { number = value; } }
    public Team Team { get { return team; } set { team = value; } }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isCanPlaced)
        {
            return;
        }

        GameManager.Instance.GameController.ClickOnTile(this);
    }

    public void PlaceObject(Team team)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        switch (team)
        {
            case Team.Player1:
                {
                    stone_Player1.SetActive(true);
                    break;
                }
            case Team.Player2:
                {
                    stone_Player2.SetActive(true);
                    break;
                }
        }
    }

    public void ShowPossiblePlacement(bool possible)
    {
        isCanPlaced = possible;
        if(isCanPlaced)
        {
            stoneCanPlaced.SetActive(true);
        }
        else
        {
            stoneCanPlaced.SetActive(false);
        }
    }

}
