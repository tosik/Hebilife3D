using UnityEngine;
using System.Collections;
using Hebilife;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField]
    long SizeX = 50;

    [SerializeField]
    long SizeY = 50;

    [SerializeField]
    int NumberOfInitialSnakes = 100;

    [SerializeField]
    int NumberOfInitialFeeds = 100;

    [SerializeField]
    int NumberOfRooms = 5;

    Schale _schale = new Schale();
    View _view;

    Object _snakePrefab;
    Object _feedPrefab;
    Object _wallPrefab;

    Dictionary<Position, GameObject> _feeds = new Dictionary<Position, GameObject>();

    void Start()
    {
        _view = new View(SizeX, SizeY);

        _schale.SnakeGenerated += OnSnakeGenerate;
        _schale.FeedRemoved += OnFeedRemove;
        _schale.FeedGenerated += OnFeedGenerate;

        _snakePrefab = Resources.Load("Prefabs/Snake");
        _feedPrefab = Resources.Load("Prefabs/Feed");
        _wallPrefab = Resources.Load("Prefabs/Wall");

        _schale.GenerateSnakes(NumberOfInitialSnakes, SizeX, SizeY);
        _schale.GenerateFeeds(NumberOfInitialFeeds, SizeX, SizeY);
        _schale.CreateFrame(SizeX, SizeY);
        _schale.CreateRooms(NumberOfRooms, SizeX, SizeY);

        CreateWallObjects();
    }

    void CreateWallObjects()
    {
        foreach (var wall in _schale.Walls)
        {
            var obj = (GameObject)Instantiate(_wallPrefab);
            obj.transform.position = new Vector3(wall.X, 0, wall.Y);
            obj.name = "Wall";
        }
    }

    void OnSnakeGenerate(Snake snake)
    {
        var obj = (GameObject)Instantiate(_snakePrefab);
        var behaviour = obj.GetComponent<SnakeBehaviour>();
        behaviour.Initialize(snake);
        obj.name = "Snake";
    }

    void OnFeedGenerate(Position pos)
    {
        var obj = (GameObject)Instantiate(_feedPrefab);
        obj.transform.position = new Vector3(pos.X, 0, pos.Y);
        _feeds[pos] = obj;
    }

    void OnFeedRemove(Position pos)
    {
        var obj = _feeds[pos];
        _feeds.Remove(pos);
        Destroy(obj);
    }

    void Update()
    {
        _schale.Step();

        //_view.Reflect(_schale);
    }

    /*
    void OnGUI()
    {
        for (long y = 0; y < SizeY; y++)
        {
            for (long x = 0; x < SizeX; x++)
            {
                var cell = _view.Get(x, y);

                var character = "";
                switch (cell)
                {
                    case View.Cell.Feed:
                        character = ".";
                        break;
                    case View.Cell.Snake:
                        character = "o";
                        break;
                    case View.Cell.Wall:
                        character = "X";
                        break;
                }

                if (character != "")
                {
                    GUI.Label(new Rect(x * 10, y * 10, 30, 30), character);
                }
            }
        }
    }
    */
}
