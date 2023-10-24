using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] private GameObject stone_Impediments;
    [SerializeField] private GameObject stoneCanPlaced;
    [SerializeField] private Image stoneCanPlacedImage;

    private Board board;
    private int number;
    private Team team;
    private bool isCanPlaced = false;
    private float stoneCanPlacedImageAlpha = 1.0f;
    private bool isStoneCanPlacedAlpha;
    private bool isTurn;
    private bool isHalfTurn;
    private float rotateY;
    private Team nextTeam;

    public Board Board { set { board = value; } }
    public int Number { get { return number; } set { number = value; } }
    public Team Team { get { return team; } set { team = value; } }

    private void Update()
    {
        StoneCanPlacedEffect();
        TurnStoneEffect();
    }


    public void PlaceObject(Team team)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(this.team == Team.Player1 || this.team == Team.Player2)
        {
            isTurn = true;
            isHalfTurn = false;
            rotateY = 0f;
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
            case Team.Impediments:
                {
                    stone_Impediments.SetActive(true);
                    break;
                }
        }

        this.team = team;
    }

    public void TurningOverStone(Team team)
    {
        if (this.team != Team.Player1 && this.team != Team.Player2)
        {
            return;
        }

        isTurn = true;
        isHalfTurn = false;
        rotateY = 0f;

        switch (team)
        {
            case Team.Player1:
                {
                    nextTeam = Team.Player1;
                    break;
                }
            case Team.Player2:
                {
                    nextTeam = Team.Player2;
                    break;
                }
        }

        this.team = team;
    }

    public void ShowPossiblePlacement(bool possible)
    {
        isCanPlaced = possible;
        if (isCanPlaced)
        {
            stoneCanPlaced.SetActive(true);
            stoneCanPlacedImageAlpha = 1f;
            stoneCanPlacedImage.color = new Color(1f, 1f, 1f, stoneCanPlacedImageAlpha);
        }
        else
        {
            stoneCanPlaced.SetActive(false);
        }
    }

    private void StoneCanPlacedEffect()
    {
        if (!isCanPlaced)
        {
            return;
        }

        if (isStoneCanPlacedAlpha)
        {
            stoneCanPlacedImageAlpha -= Time.deltaTime * 0.8f;
            if (stoneCanPlacedImageAlpha < 0.1f)
            {
                isStoneCanPlacedAlpha = false;
            }
        }
        else
        {
            stoneCanPlacedImageAlpha += Time.deltaTime * 0.8f;
            if (stoneCanPlacedImageAlpha > 1f)
            {
                isStoneCanPlacedAlpha = true;
            }
        }
        stoneCanPlacedImage.color = new Color(1f, 1f, 1f, stoneCanPlacedImageAlpha);
    }

    private void TurnStoneEffect()
    {
        if (!isTurn)
        {
            return;
        }

        if(!isHalfTurn)
        {
            rotateY += Time.deltaTime * 400f;
            if(rotateY > 90f)
            {
                isHalfTurn = true;
                rotateY = 90f;
                switch(nextTeam)
                {
                    case Team.Player1:
                        {
                            stone_Player1.SetActive(true);
                            stone_Player2.SetActive(false);
                            break;
                        }
                    case Team.Player2:
                        {
                            stone_Player1.SetActive(false);
                            stone_Player2.SetActive(true);
                            break;
                        }
                }
            }
        }
        else
        {
            rotateY -= Time.deltaTime * 400f;
            if(rotateY < 0f)
            {
                isTurn = false;
                rotateY = 0f;
            }
        }

        transform.rotation = Quaternion.Euler(0, rotateY, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isCanPlaced)
        {
            return;
        }

        GameManager.Instance.GameController.ClickOnTile(this);
    }

}
