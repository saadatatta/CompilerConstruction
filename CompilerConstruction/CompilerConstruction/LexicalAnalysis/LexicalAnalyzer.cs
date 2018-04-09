using System;
using System.Collections.Generic;
using System.IO;
using CompilerConstruction.Enums;
using CompilerConstruction.Structs;

namespace CompilerConstruction.LexicalAnalysis
{
    class LexicalAnalyzer
    {
        private StreamReader dataToRead;

        public List<Token> tokensList = new List<Token>();

        /// <param name="dataToRead">The associated streamreader object which contain data.
        /// </param>

        public LexicalAnalyzer(StreamReader dataToRead)
        {
            this.dataToRead = dataToRead;
        }

        /// <summary>
        /// Performs Lexical Analysis on data.
        /// </summary>
        public void PerformAnalysis()
        {
            if (dataToRead == null)
            {
                throw new Exception("Data was not present");
                
            }

            using (dataToRead)
            {
                // Read until end of file marker is reached.
                while (dataToRead.Peek() > 0)
                {
                    char characterToRead = (char)dataToRead.Read();

                    if (IsWhiteSpace(characterToRead))
                    {
                        continue;
                    }

                    if(characterToRead is  '<')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is '=')
                        {
                            tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.LessThanEqualTo));
                            continue;
                        }

                        if (characterToRead is '>')
                        {
                            tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.NotEqualTo));
                            continue;
                        }

                        if (!IsValidSeperator(characterToRead))
                        {
                            // TODO:Generate an error.
                            continue;
                        }

                        tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.LessThan));
                        continue;
                    }

                    if (characterToRead is '>')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is '=')
                        {
                            tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.GreaterThanEqualTo));
                            continue;
                        }

                        if (!IsValidSeperator(characterToRead))
                        {
                            // TODO:Generate an error.
                            continue;
                        }

                        tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.GreaterThan));
                        continue;
                    }

                    if(characterToRead is '=')
                    {
                        tokensList.Add(new Token(ETokenType.RelationalOperator, ERelationalOperator.EqualTo));
                        continue;
                    }

                    if(!IsValidSeperator(characterToRead))
                    {
                        //TODO: Generate an error.
                        continue;
                    }

                    if(characterToRead is '{') // Handle comments
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                        while (!(characterToRead is '}'))
                        {
                            if (characterToRead is '{') // Nested comments are not allowed.
                            {
                                // Todo: Generate an error for nested comment.
                                continue;
                            }
                            else
                                continue;
                        }
                        continue;
                    } // Comment

                    if (IsDigit(characterToRead))
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        while(IsDigit(characterToRead))
                        {
                            continue;
                        }

                        if (characterToRead is '.')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                            if (IsDigit(characterToRead))
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                while (IsDigit(characterToRead))
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                    continue;
                                }

                                if (characterToRead is 'e' || characterToRead is 'E')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is '+' || characterToRead is '-' || IsDigit(characterToRead))
                                    {
                                        if (characterToRead is '+' || characterToRead is '-')
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                            if (IsDigit(characterToRead))
                                            {
                                                while (IsDigit(characterToRead))
                                                {
                                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                                    continue;
                                                }

                                                tokensList.Add(new Token(ETokenType.Number, ENumber.FloatAndExponential));
                                                continue;
                                            }
                                            else
                                            {
                                                //Todo:Throw an error.
                                            }
                                        }

                                        while (IsDigit(characterToRead))
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                            continue;
                                        }

                                        tokensList.Add(new Token(ETokenType.Number, ENumber.FloatAndExponential));
                                        continue;
                                    }
                                }

                                tokensList.Add(new Token(ETokenType.Number, ENumber.Float));
                                continue;
                            }
                            else
                            {
                                //TODO: Generate an error.
                            }
                        }

                        if (characterToRead is 'e' || characterToRead is 'E')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is '+' || characterToRead is '-' || IsDigit(characterToRead))
                            {
                                if(characterToRead is '+' || characterToRead is '-')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (IsDigit(characterToRead))
                                    {
                                        while (IsDigit(characterToRead))
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                            continue;
                                        }

                                        tokensList.Add(new Token(ETokenType.Number, ENumber.IntegerAndExponential));
                                        continue;
                                    }
                                    else
                                    {
                                        //Todo:Throw an error.
                                    }
                                }

                                while (IsDigit(characterToRead))
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                    continue;
                                }

                                tokensList.Add(new Token(ETokenType.Number, ENumber.IntegerAndExponential));
                                continue;
                            }
                        }

                        tokensList.Add(new Token(ETokenType.Number, ENumber.Integer));
                        continue;
                    } // Handles numbers.

                    if (characterToRead is '+' || characterToRead is '-')
                    {
                        if (characterToRead is '+')
                        {
                            tokensList.Add(new Token(ETokenType.AddOperator, EAddOperator.Add));
                            continue;
                        }

                        if (characterToRead is '-')
                        {
                            tokensList.Add(new Token(ETokenType.AddOperator, EAddOperator.Subtract));
                            continue;
                        }
                    }

                    if (characterToRead is '*' || characterToRead is '/')
                    {
                        if (characterToRead is '*')
                        {
                            tokensList.Add(new Token(ETokenType.MultiplyOperator, EMultiplyOperator.Multiply));
                            continue;
                        }

                        if (characterToRead is '/')
                        {
                            tokensList.Add(new Token(ETokenType.MultiplyOperator, EMultiplyOperator.Divide));
                            continue;
                        }
                    }

                    if(characterToRead is ':')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is '=')
                        {
                            tokensList.Add(new Token(ETokenType.AssignmentOperator, null));
                            continue;
                        }
                        else
                        {
                            //Todo: Generate an error.
                            continue;
                        }
                    }   //Assignment operator

                    #region keywords handling

                    if (characterToRead is 'a')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'n')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'd')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.And));
                                continue;
                            }
                        }

                        if(characterToRead is 'r')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is 'r')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if(characterToRead is 'a')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if(characterToRead is 'y')
                                    {
                                        tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Array));
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    if (characterToRead is 'b')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'e')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'g')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'i')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is 'n')
                                    {
                                        tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Array));
                                        continue;
                                    }
                                }
                               
                            }
                        }
                    }

                    if(characterToRead is 'd')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                        
                        if(characterToRead is 'o')
                        {
                            tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Do));
                            continue;
                        }

                        if(characterToRead is 'i')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is 'v')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Div));
                                continue;
                            }
                        }
                    }

                    if(characterToRead is 'e')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if(characterToRead is 'n')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is 'd')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.End));
                                continue;
                            }
                        }

                        if (characterToRead is 'l')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is 's')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if(characterToRead is 'e')
                                {
                                    tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Else));
                                    continue;
                                }
                            }
                        }
                    }

                    if(characterToRead is 'f')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if(characterToRead is 'u')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if(characterToRead is 'n')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if(characterToRead is 'c')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if(characterToRead is 't')
                                    {
                                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                        if(characterToRead is 'i')
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                            if(characterToRead is 'o')
                                            {
                                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                                if(characterToRead is 'n')
                                                {
                                                    tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Function));
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(characterToRead is 'i')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if(characterToRead is 'f')
                        {
                            tokensList.Add(new Token(ETokenType.Keyword, EKeywords.If));
                            continue;
                        }

                        if (characterToRead is 'n')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 't')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'e')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is 'g')
                                    {
                                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                        if (characterToRead is 'e')
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                            if (characterToRead is 'r')
                                            {
                                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Integer));
                                                continue;
                                            }
                                        }
                                        }
                                    }
                                }
                            }
                        }

                    if (characterToRead is 'm')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'o')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'd')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Mod));
                                continue;
                            }
                        }
                    }

                    if (characterToRead is 'n')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'o')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 't')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Not));
                                continue;
                            }
                        }
                    }

                    if (characterToRead is 'o')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'r')
                        {
                            tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Or));
                            continue;
                        }

                        if(characterToRead is 'f')
                        {
                            tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Of));
                            continue;
                        }

                    }

                    if (characterToRead is 'p')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'r')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'o')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'g')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is 'r')
                                    {
                                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                        if (characterToRead is 'a')
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                            if (characterToRead is 'm')
                                            {
                                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Program));
                                                continue;
                                            }
                                        }
                                    }
                                }

                                if (characterToRead is 'c')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is 'e')
                                    {
                                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                        if (characterToRead is 'd')
                                        {
                                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                            if (characterToRead is 'u')
                                            {
                                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                                                {
                                                    if (characterToRead is 'r')
                                                    {
                                                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                                        if (characterToRead is 'e')
                                                        {
                                                            tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Procedure));
                                                            continue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (characterToRead is 'r')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'e')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'a')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'l')
                                {
                                    tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Real));
                                    continue;
                                    
                                }
                            }
                        }
                    }

                    if (characterToRead is 't')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'h')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'e')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'n')
                                {
                                    tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Then));
                                    continue;

                                }
                            }
                        }
                    }

                    if (characterToRead is 'v')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'a')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'r')
                            {
                                tokensList.Add(new Token(ETokenType.Keyword, EKeywords.Var));
                                continue;

                            }
                            
                        }
                    }

                    if (characterToRead is 'w')
                    {
                        characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                        if (characterToRead is 'h')
                        {
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (characterToRead is 'i')
                            {
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (characterToRead is 'l')
                                {
                                    characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                    if (characterToRead is 'e')
                                    {
                                        tokensList.Add(new Token(ETokenType.Keyword, EKeywords.While));
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }

            }
        }

        /// <summary>
        /// Check if charater is a white space character.
        /// </summary>
        /// <param name="c">The charater to be read.</param>
        /// <returns>True if the character is a white space.</returns>
        
        private bool IsWhiteSpace(char c)
        {
            if (c is ' ')       // white space character.
                return true;
            else if (c is '\t') // tab space character.
                return true;
            else if (c is '\r') // carriage return character.
                return true;
            else if (c is '\n') //new line character.
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the seperator is a valid token seperator.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns>True if it is a valid seperator, otherwise , false.</returns>

        private bool IsValidSeperator(char c)
        {
            if (c is '@') // TODO: All invalid seperators should be included here.Unknown yet.
                return false;
            return true;
        }

        /// <summary>
        /// Checks if the supplied character is a valid digit.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns>True if the character is a valid digit,otherwise,false.</returns>

        private bool IsDigit(char c)
        {
            HashSet<char> digitsHashSet = new HashSet<char>()
            {
                '0','1','2','3','4','5','6','7','8','9'
            };

            if (digitsHashSet.Contains(c))
                return true;

            return false;
        }

    }
}
