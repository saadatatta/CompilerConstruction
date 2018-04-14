using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerConstruction.LexicalAnalysis
{
    public class KeywordHashing
    {
        public List<string>[] ArrayOfKeyword = new List<string>[19];

        public KeywordHashing(string[] keywords)
        {
            StoreKeywordsAtHash(keywords);
        }

        private int GetHashCode(string keyword)
        {
            int asciiCodeOfFirstCharacterOfKeyword = (int)keyword[0];

            return asciiCodeOfFirstCharacterOfKeyword % 19;
            
        }

        private void StoreKeywordsAtHash(string[] keywords)
        {
            foreach(string keyword in keywords)
            {
                int hashCode = GetHashCode(keyword);

                if (ArrayOfKeyword[hashCode] == null)
                {
                    ArrayOfKeyword[hashCode] = new List<string>();
                }

                ArrayOfKeyword[hashCode].Add(keyword);
            }
        }

        public bool IsKeyWord(string keyword)
        {
            int index = GetHashCode(keyword);

            foreach(string kw in ArrayOfKeyword[index])
            {
                if (keyword == kw) // Values are matched.
                    return true;
            }

            return false;
        }
    }
}
