using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlaceableTile
{
    public Tile tile = null;
    public int countCanTurnOver;
}

public class Board : MonoBehaviour
{
    public const int MaxCountOfTiles = 64;
    public const int CountLines = 8;
    public const int ScoreUnit = 10;
    public const float TurningOverDelay = 0.15f;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject stone1Prefab;
    [SerializeField] private GameObject stone2Prefab;

    [SerializeField] private GameObject showCurrentTurnImage;
    [SerializeField] private TMP_Text textPlayer1Score;
    [SerializeField] private TMP_Text textPlayer2Score;


    private GameObject tilesParent;
    private Tile[,] tiles;
    private List<Tile> player1Tile = new List<Tile>();
    private List<Tile> player2Tile = new List<Tile>();
    private List<PlaceableTile> placeableTiles = new List<PlaceableTile>();

    private Queue<Tile> stonesTurningOver = new Queue<Tile>();
    private float turningOverTick;



    public GameObject ShowTurnImage { get { return showCurrentTurnImage; } }
    public TMP_Text TextPlayer1Score { get { return textPlayer1Score; } }
    public TMP_Text TextPlayer2Score { get { return textPlayer2Score; } }
    public Tile[,] Tiles { get { return tiles; } }
    public int CountStoneTurningOver { get { return stonesTurningOver.Count; } }

    public void Awake()
    {
        tilesParent = transform.GetChild(0).gameObject;

        tiles = new Tile[CountLines, CountLines];

        for (int i = 0; i < MaxCountOfTiles; i++)
        {
            GameObject obj = GameObject.Instantiate(tilePrefab);
            Tile tile = obj.GetComponent<Tile>();
            tile.Board = this;
            tile.Number = i;
            tile.Team = Team.None;
            obj.transform.SetParent(tilesParent.transform);
            obj.transform.localScale = Vector3.one;

            tiles[i / CountLines, i % CountLines] = tile;
        }
        turningOverTick = TurningOverDelay;
    }

    public void Update()
    {
        if (stonesTurningOver.Count > 0)
        {
            turningOverTick -= Time.deltaTime;
            if(turningOverTick < 0)
            {
                Tile tile = stonesTurningOver.Dequeue();
                turningOverTick = TurningOverDelay;
                tile.PlaceObject(tile.Team);
            }
        }
    }

    public int PlaceStone(Team team, int column, int row)
    {
        if (stonesTurningOver.Count > 0)
        {
            return -1;
        }

        Tile tile = tiles[column, row];
        if (tile.Team != Team.None)
        {
            return -1;
        }

        tile.Team = team;
        tile.PlaceObject(team);

        if (team == Team.Player1)
        {
            player1Tile.Add(tile);
        }
        else if(team == Team.Player2)
        {
            player2Tile.Add(tile);
        }
        int score = FindStoneTurnOver(team, column, row);

        return score;
    }

    public int PlaceStoneByAi()
    {
        List<PlaceableTile> selectedTile = new List<PlaceableTile>();
        foreach (PlaceableTile placeableTile in placeableTiles)
        {
            if (selectedTile.Count == 0)
            {
                selectedTile.Add(placeableTile);
                continue;
            }

            if (selectedTile[0].countCanTurnOver < placeableTile.countCanTurnOver)
            {
                selectedTile.Clear();
                selectedTile.Add(placeableTile);
            }
            else if (selectedTile[0].countCanTurnOver == placeableTile.countCanTurnOver)
            {
                selectedTile.Add(placeableTile);
            }
        }

        if (selectedTile.Count == 0)
        {
            return -1;
        }

        int random = Random.Range(0, selectedTile.Count);
        int tileNumber = selectedTile[random].tile.Number;
        int score = PlaceStone(Team.Player2, tileNumber / CountLines, tileNumber % CountLines);

        return score;
    }

    public void SettingPlacedImpediments(int count)
    {
        int random;
        List<int> numberAlreadyPlaced = new List<int>();
        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                random = Random.Range(0, CountLines * CountLines);

                if(random == 27 || random == 28 || random == 35 || random == 36)
                {
                    continue;
                }

                bool checkAlreadyPlaced = false;
                foreach(int number in numberAlreadyPlaced)
                {
                    if(random == number)
                    {
                        checkAlreadyPlaced = true;
                        break;
                    }
                }
                if(!checkAlreadyPlaced)
                {
                    break;
                }
            }

            Tile tile = tiles[random / CountLines, random % CountLines];

            tile.Team = Team.Impediments;
            tile.PlaceObject(Team.Impediments);
            numberAlreadyPlaced.Add(random);
        }


    }

    public int FindStoneTurnOver(Team team, int column, int row)
    {
        List<Tile> stoneTurnOver = new List<Tile>();
        List<Tile> tempList = new List<Tile>();
        int count;
        Tile tile;
        // 9시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (row - count >= 0)
            {
                tile = tiles[column, row - count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 11시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column - count >= 0 && row - count >= 0)
            {
                tile = tiles[column - count, row - count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 12시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column - count >= 0)
            {
                tile = tiles[column - count, row];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 1시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column - count >= 0 && row + count < CountLines)
            {
                tile = tiles[column - count, row + count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 3시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (row + count < CountLines)
            {
                tile = tiles[column, row + count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 5시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column + count < CountLines && row + count < CountLines)
            {
                tile = tiles[column + count, row + count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 6시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column + count < CountLines)
            {
                tile = tiles[column + count, row];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        // 7시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if (column + count < CountLines && row - count >= 0)
            {
                tile = tiles[column + count, row - count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach (Tile temp in tempList)
                    {
                        stoneTurnOver.Add(temp);
                    }
                    tempList.Clear();
                    break;
                }
                else if (tile.Team != team)
                {
                    tempList.Add(tile);
                    continue;
                }
            }
            else
            {
                tempList.Clear();
                break;
            }
        }

        foreach (Tile turnTile in stoneTurnOver)
        {
            turnTile.Team = team;
            //turnTile.PlaceObject(team);
            stonesTurningOver.Enqueue(turnTile);
            if (team == Team.Player1)
            {
                player2Tile.Remove(turnTile);
                player1Tile.Add(turnTile);
            }
            else
            {
                player1Tile.Remove(turnTile);
                player2Tile.Add(turnTile);
            }
        }
        return CalcScoreGet(stoneTurnOver.Count);
    }

    public int CalcScoreGet(int countStoneTurnOver)
    {
        int totalScore = 0;
        int overlapScore = 10;
        for (int i = 0; i < countStoneTurnOver; i++)
        {
            totalScore += overlapScore;
            overlapScore += 10;
        }
        return totalScore;
    }

    public void ClearForPossiblePlaced()
    {
        foreach (PlaceableTile placeableTile in placeableTiles)
        {
            placeableTile.tile.ShowPossiblePlacement(false);
        }
        placeableTiles.Clear();
    }

    public bool SearchForPossiblePlaced(Team team, bool showGuide)
    {
        List<Tile> tempList = new List<Tile>();
        List<Tile> myStone;
        if (team == Team.Player1)
        {
            myStone = player1Tile;
        }
        else
        {
            myStone = player2Tile;
        }

        int count;
        int column;
        int row;
        Tile tile;

        foreach (Tile stone in myStone)
        {
            column = stone.Number / CountLines;
            row = stone.Number % CountLines;
            // 9시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (row - count >= 0)
                {
                    tile = tiles[column, row - count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 11시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column - count >= 0 && row - count >= 0)
                {
                    tile = tiles[column - count, row - count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 12시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column - count >= 0)
                {
                    tile = tiles[column - count, row];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 1시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column - count >= 0 && row + count < CountLines)
                {
                    tile = tiles[column - count, row + count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 3시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (row + count < CountLines)
                {
                    tile = tiles[column, row + count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 5시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column + count < CountLines && row + count < CountLines)
                {
                    tile = tiles[column + count, row + count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 6시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column + count < CountLines)
                {
                    tile = tiles[column + count, row];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

            // 7시 방향
            while (true)
            {
                count = tempList.Count + 1;
                if (column + count < CountLines && row - count >= 0)
                {
                    tile = tiles[column + count, row - count];
                    if (tile.Team == Team.None)
                    {
                        if (tempList.Count == 0) break;

                        PlaceableTile placeableTile = new PlaceableTile();
                        placeableTile.tile = tile;
                        placeableTile.countCanTurnOver = tempList.Count;
                        placeableTiles.Add(placeableTile);
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == Team.Impediments)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team == team)
                    {
                        tempList.Clear();
                        break;
                    }
                    else if (tile.Team != team)
                    {
                        tempList.Add(tile);
                        continue;
                    }
                }
                else
                {
                    tempList.Clear();
                    break;
                }
            }

        }
        if (placeableTiles.Count == 0)
        {
            return false;
        }

        if (showGuide)
        {
            foreach (PlaceableTile placeableTile in placeableTiles)
            {
                placeableTile.tile.ShowPossiblePlacement(true);
            }
        }

        return true;
    }
}
