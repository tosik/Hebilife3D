using UnityEngine;
using System.Collections;
using Hebilife;

public class Game : MonoBehaviour
{
    [SerializeField]
    long SizeX = 50;

    [SerializeField]
    long SizeY = 50;

    Schale _schale = new Schale();
    View _view;

    void Start()
    {
        _view = new View(SizeX, SizeY);
        _schale.GenerateSnakes(10);
        _schale.GenerateFeeds(10);
    }

    void Update()
    {
        _schale.Step();

        _view.Reflect(_schale);
    }

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
                }
                GUI.Label(new Rect(x * 10, y * 10, 30, 30), character);
            }
        }
    }
}
