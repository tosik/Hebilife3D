using System;
using Hebilife;

namespace HebilifeConsole
{
    class Program
    {
        public static void Main(string[] args)
        {
            //RunLegacy();

            Run();
        }

        static void Run()
        {
            // Schale equals a petri dish in German
            var schale = new Schale();

            schale.GenerateSnakes(1);

            for (;;)
            {
                schale.Step();
            }
        }


        static void RunLegacy()
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
