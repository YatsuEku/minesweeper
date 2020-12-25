using System;

namespace Saper
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SaperGame())
                game.Run();
        }
    }
}
