using UnityEngine;
using System.Collections;
using Hebilife;
using System.Collections.Generic;
using System.Linq;

public class Game : Controller
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

    Object _snakePrefab;
    Object _feedPrefab;
    Object _wallPrefab;

    Dictionary<Position, GameObject> _feeds = new Dictionary<Position, GameObject>();

    public override void Start()
    {
        base.Start();

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

        if (_schale.Snakes.Count() == 0)
        {
            _schale.GenerateSnakes(NumberOfInitialSnakes, SizeX, SizeY);
        }

        Context.Status.Reflect(_schale);
        Context.Status.Steps++;
    }

    public void GenerateRandomSnakes()
    {
        _schale.GenerateSnakes(20, SizeX, SizeY);
    }
}
