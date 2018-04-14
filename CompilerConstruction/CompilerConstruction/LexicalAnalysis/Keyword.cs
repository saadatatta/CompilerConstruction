using CompilerConstruction.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerConstruction.LexicalAnalysis
{
    public static class Keyword
    {
        public static string[] keywords = new string[]
        {
            "and","array","begin","do","div","end","else","function","if","integer",
            "mod","not","or","of","program","procedure","real","then","var","while"
        };

        public static EKeywords GetKeywordEnum(string keyword)
        {
            switch (keyword)
            {
                case "and":
                    return EKeywords.And;
                case "array":
                    return EKeywords.Array;
                case "begin":
                    return EKeywords.Begin;
                case "do":
                    return EKeywords.Do;
                case "div":
                    return EKeywords.Div;
                case "end":
                    return EKeywords.End;
                case "else":
                    return EKeywords.Else;
                case "function":
                    return EKeywords.Function;
                case "if":
                    return EKeywords.If;
                case "integer":
                    return EKeywords.Integer;
                case "mod":
                    return EKeywords.Mod;
                case "not":
                    return EKeywords.Not;
                case "or":
                    return EKeywords.Or;
                case "of":
                    return EKeywords.Of;
                case "program":
                    return EKeywords.Program;
                case "procedure":
                    return EKeywords.Procedure;
                case "real":
                    return EKeywords.Real;
                case "then":
                    return EKeywords.Then;
                case "var":
                    return EKeywords.Var;
                case "while":
                    return EKeywords.While;
            }
            return EKeywords.And;
        }
    }
}
