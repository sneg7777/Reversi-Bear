using System.Collections;
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

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject stone1Prefab;
    [SerializeField] private GameObject stone2Prefab;
    private GameObject tilesParent;
    //private List<Tile> tiles = new List<Tile>();
    private Tile[,] tiles;
    [SerializeField] private List<Tile> player1Tile = new List<Tile>();
    [SerializeField] private List<Tile> player2Tile = new List<Tile>();
    private List<PlaceableTile> placeableTiles = new List<PlaceableTile>();

    public Tile[,] Tiles { get { return tiles; } }

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

    public bool PlaceStone(Team team, int column, int row)
    {
        Tile tile = tiles[column, row];
        if(tile.Team != Team.None)
        {
            return false;
        }

        tile.Team = team;
        tile.PlaceObject(team);

        if(team == Team.Player1)
        {
            player1Tile.Add(tile);
        }
        else
        {
            player2Tile.Add(tile);
        }
        FindStoneTurnOver(team, column, row);

        return true;
    }

    public void FindStoneTurnOver(Team team, int column, int row)
    {
        List<Tile> stoneTurnOver = new List<Tile>();
        List<Tile> tempList = new List<Tile>();
        int count;
        Tile tile;
        // 9시 방향
        while (true)
        {
            count = tempList.Count + 1;
            if(row - count >= 0)
            {
                tile = tiles[column, row - count];
                if (tile.Team == Team.None)
                {
                    tempList.Clear();
                    break;
                }
                else if(tile.Team == Team.Impediments)
                {
                    tempList.Clear();
                    break;
                }
                else if (tile.Team == team)
                {
                    foreach(Tile temp in tempList)
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
            turnTile.PlaceObject(team);
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
    }

    public void ClearForPossiblePlaced()
    {
        foreach (PlaceableTile placeableTile in placeableTiles)
        {
            placeableTile.tile.ShowPossiblePlacement(false);
        }
        placeableTiles.Clear();
    }

    public bool SearchForPossiblePlaced(Team team)
    {
        List<Tile> tempList = new List<Tile>();
        List<Tile> myStone;
        if(team == Team.Player1)
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

            if(placeableTiles.Count == 0)
            {
                return false;
            }

            foreach (PlaceableTile placeableTile in placeableTiles)
            {
                placeableTile.tile.ShowPossiblePlacement(true);
            }
            return true;
        }
    }

}
