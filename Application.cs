using System;

namespace d4lilah
{
#if WINDOWS || LINUX             
    public static class Application
    {                               
        [STAThread]
        static void Main(string[] arg)
        {            
            using(var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}
