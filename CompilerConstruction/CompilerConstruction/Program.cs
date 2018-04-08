using CompilerConstruction.FileHandling;
using CompilerConstruction.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompilerConstruction
{
    class Program
    {
        static void Main(string[] args)
        {
            FileHandler fileHandler = new FileHandler();
            StreamReader reader =  fileHandler.ReadData();

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(reader);
            lexicalAnalyzer.PerformAnalysis();

            
        }
    }
    
}
