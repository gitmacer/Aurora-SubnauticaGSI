using System;
using System.IO;

namespace SubnauticaGSI
{
    public class Main
    {

        public static void Patch()
        {
            // Load controller
            AuroraController.Load();
        }

        //Just for Debugging
        public static void Savetofile(string x = "NA")
        {
            string path = @"./QMods/SubnauticaGSI/Log.txt";
            // Create a file to write to.

            string createText = x + Environment.NewLine;
            File.WriteAllText(path, createText);
        }

    }
}


