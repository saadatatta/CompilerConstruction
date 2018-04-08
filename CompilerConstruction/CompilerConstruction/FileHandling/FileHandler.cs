using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerConstruction.FileHandling
{
    /// <summary>
    /// This class handles the reading from and writing data to a file.
    /// </summary>

    public class FileHandler
    {
        /// <summary>
        /// Reads the data from the text file.
        /// </summary>
        /// <returns>The stream of bytes which can by read by caller object.</returns>

        public StreamReader ReadData()
        {
            Directory.SetCurrentDirectory(@"D:\all semesters\8th semester\Compiler Construction\Assignments\CompilerConstruction");

            string filePath = Directory.GetCurrentDirectory() + @"\Code File.txt";

            if (!File.Exists(filePath))
                throw new Exception("File was not present at the specified location");

            return new StreamReader(filePath);

        }

    }
}
