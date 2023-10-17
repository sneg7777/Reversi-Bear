using System.Collections.Generic;
using UnityEngine;

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

    private GameObject tilesParent;
    private Tile[,] tiles;
    [SerializeField] private List<Tile> player1Tile = new List<Tile>();
    [SerializeField] private List<Tile> player2Tile = new List<Tile>();
    private List<PlaceableTile> placeableTiles = new List<PlaceableTile>();

    private Queue<Tile> stonesTurningOver = new Queue<Tile>();
    private float turningOverTick;

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

            tiles[i / CountLines, i % CountLines] = tile;
        }
    }

    public void Update()
    {
        turningOverTick -= Time.deltaTime;
        if (stonesTurningOver.Count > 0 && turningOverTick < 0f)
        {
            Tile tile = stonesTurningOver.Dequeue();
            turningOverTick = TurningOverDelay;
            tile.PlaceObject(tile.Team);

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
        else
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

    public int FindStoneTurnOver(Team team, int column, int row)
    {
        List<Tile> stoneTurnOver = new List<Tile>();
        List<Tile> tempList = new List<Tile>();
        int count;
        Tile tile;
        // 9�� ����
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

        // 11�� ����
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

        // 12�� ����
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

        // 1�� ����
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

        // 3�� ����
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

        // 5�� ����
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

        // 6�� ����
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

        // 7�� ����
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
            // 9�� ����
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

            // 11�� ����
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

            // 12�� ����
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

            // 1�� ����
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

            // 3�� ����
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

            // 5�� ����
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

            // 6�� ����
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

            // 7�� ����
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
