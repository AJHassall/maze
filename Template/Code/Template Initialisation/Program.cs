using System;

//container for all systems code

namespace Template
{
#if WINDOWS || XBOX
    static class Program
    {
        
        // The main entry point for the application.
        
        static void Main(string[] args)
        {
            using (GM game = new GM())
            {
                game.Run();
            }
        }
    }
#endif
}

