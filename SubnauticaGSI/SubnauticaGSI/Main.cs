using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            string path = @"c:\temp\Debug.txt";
            // Create a file to write to.

            string createText = x + Environment.NewLine;
            File.WriteAllText(path, createText);
        }

    }
}


