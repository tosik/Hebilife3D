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

        ApplyGene();

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

    void ApplyGene()
    {
        var gene = "1.58046875\n1.091015625\n0.317578125\n-0.2326171875\n0.34609375\n0.4677734375\n0.6484375\n0.5283203125\n0.994140625\n0.799609375\n0.239453125\n0.54453125\n0.2728515625\n0.2455078125\n0.8296875\n0.4689453125\n0.8236328125\n0.33125\n0.2689453125\n0.7970703125\n0.230078125\n1.3521484375\n0.306640625\n0.0416015625\n0.3076171875\n0.4671875\n0.71015625\n-0.14609375\n0.5908203125\n0.4291015625\n0.0611328125\n0.2736328125\n0.2642578125\n0.2078125\n0.0390625\n0.496484375\n-0.3453125\n0.24296875\n0.43984375\n1.102734375\n0.84140625\n1.0326171875\n0.5072265625\n1.2716796875\n1.0177734375\n0.14453125\n0.669140625\n1.162890625\n0.74375\n0.86953125\n0.13984375\n0.74609375\n0.7212890625\n-0.2765625\n0.0962890625\n0.6068359375\n0.52578125\n0.509765625\n0.7283203125\n0.0486328125\n0.7130859375\n0.50546875\n0.0884765625\n-0.00859375000000004\n0.7572265625\n0.5068359375\n0.736328125\n0.201171875\n0.3267578125\n0.5841796875\n0.898828125\n0.759765625\n";
        foreach (var snake in _schale.Snakes)
        {
            snake.Brain.NeuralNetwork.SetGene(gene);
        }
    }
}
