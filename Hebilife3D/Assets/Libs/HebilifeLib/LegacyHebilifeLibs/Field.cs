using System;
using System.Collections.Generic;
using System.Linq;

namespace Legacy.Hebilife
{
    public class Field
    {
        public readonly int cnt_max = 1000;
        public readonly int length_max = 10;


        public List<Snake> snakes = new List<Snake>();

        public int steps;

        public static readonly int size_x = 55;
        public static readonly int size_y = 43;

        public int[,] bodies = new int[size_x, size_y];
        public bool[,] foods = new bool[size_x, size_y];
        public bool[,] wall = new bool[size_x, size_y];

        public Random _random = new Random();

        int rand()
        {
            return _random.Next(0, 256);
        }


        public Field()
        {
            steps = 0;
            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    foods[x, y] = false;
                    wall[x, y] = false;
                }
            }

            for (int i = 0; i < 20; i++)
            {
                int r = rand() % 3;

                if (r == 0)
                {
                    int x = rand() % (size_x - 8), y = rand() % (size_y - 8);

                    wall[x, y + 0] = true;
                    wall[x, y + 1] = true;
                    wall[x, y + 2] = true;
                    wall[x, y + 3] = true;
                    wall[x, y + 4] = true;
                    wall[x, y + 5] = true;
                    wall[x, y + 6] = true;
                    wall[x, y + 7] = true;
                    wall[x, y + 8] = true;

                    wall[x + 1, y] = true;
                    wall[x + 2, y] = true;
                    wall[x + 3, y] = true;
                    wall[x + 4, y] = true;
                    wall[x + 5, y] = true;
                    wall[x + 6, y] = true;
                    wall[x + 7, y] = true;
                    wall[x + 8, y] = true;

                    wall[x + 8, y + 1] = true;
                    wall[x + 8, y + 2] = true;
                    wall[x + 8, y + 3] = true;
                    wall[x + 8, y + 4] = true;
                    wall[x + 8, y + 5] = true;
                    wall[x + 8, y + 6] = true;
                    wall[x + 8, y + 7] = true;
                    wall[x + 8, y + 8] = true;

                    wall[x + 1, y + 8] = true;
                    wall[x + 7, y + 8] = true;
                }

                if (r == 1)
                {
                    int x = rand() % (size_x - 2), y = rand() % (size_y - 2);

                    wall[x + 0, y + 0] = true;
                    wall[x + 0, y + 1] = true;
                    wall[x + 1, y + 0] = true;
                    wall[x + 1, y + 1] = true;
                }

                if (r == 2)
                {
                    int x = rand() % (size_x - 5), y = rand() % (size_y - 3);

                    wall[x + 0, y + 0] = true;
                    wall[x + 1, y + 0] = true;
                    wall[x + 2, y + 0] = true;
                    wall[x + 3, y + 0] = true;
                    wall[x + 4, y + 0] = true;

                    wall[x + 0, y + 2] = true;
                    wall[x + 1, y + 2] = true;
                    wall[x + 2, y + 2] = true;
                    wall[x + 3, y + 2] = true;
                    wall[x + 4, y + 2] = true;

                    wall[x + 4, y + 1] = true;
                }
            }
        }

        int _lastGeneratedId = 0;
        int GenerateId()
        {
            return _lastGeneratedId++;
        }

        void RemoveDeadSnakes()
        {
            foreach (var snake in snakes.ToArray())
            {
                if (snake.is_dead)
                {
                    snakes.Remove(snake);
                }
            }
        }

        public void makeSnake(Snake origin)
        {
            RemoveDeadSnakes();

            var id = GenerateId();
            snakes.Add(new Snake(id, this, origin));
        }

        public void makeSnake()
        {
            RemoveDeadSnakes();

            var id = GenerateId();
            snakes.Add(new Snake(id, this));
        }

        public void step()
        {
            steps++;
            foreach (var snake in snakes.ToArray())
            {
                snake.pre_move();
            }
            foreach (var snake in snakes.ToArray())
            {
                snake.step();
            }
        }

        public Snake GetSnake(int id)
        {
            return snakes.First(x => x.id == id);
        }

        public bool isDead(int id)
        {
            int x = GetSnake(id).pre_body_x[0];
            int y = GetSnake(id).pre_body_y[0];

            if (x < 0 || x >= size_x || y < 0 || y >= size_y)
                return true;
            else if (wall[x, y])
                return true;
            else
            {
                foreach (var snake in snakes)
                {
                    if (snake.is_dead)
                        continue;
                    for (int i = 0; i < snake.length; i++)
                    {
                        if (snake.pre_body_x[i] == x && snake.pre_body_y[i] == y)
                        {
                            if (snake.id != id || i != 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool canEat(int id)
        {
            int x = GetSnake(id).pre_body_x[0];
            int y = GetSnake(id).pre_body_y[0];
            return foods[x, y];
        }

        public int random(int n)
        {
            return rand() % n;
        }

        public bool getHazard(int x, int y)
        {
            // wrap
            if (x <= -1)
                x = size_x - 1;
            if (y <= -1)
                y = size_y - 1;
            if (x >= size_x)
                x = 0;
            if (y >= size_y)
                y = 0;

            return bodies[x, y] != -1 || wall[x, y];
        }

        public int getBodies(int x, int y)
        {
            // wrap
            if (x == -1)
                x = size_x - 1;
            if (y == -1)
                y = size_y - 1;
            if (x == size_x)
                x = 0;
            if (y == size_y)
                y = 0;

            return bodies[x, y];
        }

        public bool getFoods(int x, int y)
        {
            // wrap
            if (x == -1)
                x = size_x - 1;
            if (y == -1)
                y = size_y - 1;
            if (x == size_x)
                x = 0;
            if (y == size_y)
                y = 0;

            return foods[x, y];
        }
    }
}