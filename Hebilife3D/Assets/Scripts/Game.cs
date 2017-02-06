using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public Field Field = new Field();

    void Start()
    {
        for (var i = 0; i < 100; i++)
        {
            Field.makeSnake();
        }
    }

    void Update()
    {
        Field.step();
    }

    string GetCell(int x, int y)
    {
        if (Field.bodies[x, y] < 0)
            return "o";
        if (Field.wall[x, y])
            return "@";
        if (Field.foods[x, y])
            return ".";
        else
            return "";
    }

    void OnGUI()
    {
        for ( int y = 0 ; y < Field.size_y ; y ++ )
        {
            for ( int x = 0 ; x < Field.size_x ; x ++ )
            {
                var cell = GetCell(x, y);

                GUI.Label(new Rect(x * 10, y * 10, 30, 30), cell);
            }
        }
    }
}
