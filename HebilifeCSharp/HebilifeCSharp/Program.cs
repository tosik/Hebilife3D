using System;

namespace HebilifeCSharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var field = new Legacy.Hebilife.Field();

            for (var i = 0; i < 100; i++)
            {
                field.makeSnake();
            }

            for (;;)
            {
                field.step();
            }
        }
    }
}
