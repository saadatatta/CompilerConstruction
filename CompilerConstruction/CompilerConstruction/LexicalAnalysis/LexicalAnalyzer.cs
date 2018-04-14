using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CompilerConstruction.Enums;
using CompilerConstruction.Structs;

namespace CompilerConstruction.LexicalAnalysis
{
    class LexicalAnalyzer
    {
        private StreamReader dataToRead;
        private KeywordHashing keywordHashing;

        public List<Token> tokensList = new List<Token>();

        public LexicalAnalyzer(StreamReader dataToRead,KeywordHashing keywordHashing)
        {
            this.dataToRead = dataToRead;
            this.keywordHashing = keywordHashing;
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

                    if (IsSmallLetter(characterToRead))
                    {
                        string temp = "";

                        while (IsSmallLetter(characterToRead))
                        {
                            temp += characterToRead;
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';
                            continue;
                        }

                        bool isKeyword = keywordHashing.IsKeyWord(temp);

                        if(isKeyword == true)
                        {
                            tokensList.Add(new Token(ETokenType.Keyword, Keyword.GetKeywordEnum(temp)));
                            continue;
                        }
                        else // Handle it as identifier.
                        {
                            bool specialCharacterFound = false; // Is there any special character other than "_"? Error must be generated.

                            while (!IsWhiteSpace(characterToRead))
                            {
                                temp += characterToRead;
                                characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                                if (IsSpecialCharacter(characterToRead))
                                {
                                    specialCharacterFound = true;
                                }

                                continue;
                            }

                            if (specialCharacterFound == false)
                            {
                                tokensList.Add(new Token(ETokenType.Identifier, temp));
                            }
                            else
                            {
                                //Todo:Generate an error of invalid identifier.
                            }

                            continue;
                        }
                    } //Handles keywords and identifier

                    if (characterToRead is '_')
                    {
                        string temp = "";
                        bool isValidIdentifier = false;

                        while (!IsWhiteSpace(characterToRead))
                        {
                            temp += characterToRead;
                            characterToRead = dataToRead.Peek() > 0 ? (char)dataToRead.Read() : '\0';

                            if (Regex.IsMatch(characterToRead.ToString(), "[a-z0-9 _]+", RegexOptions.IgnoreCase))
                            {
                                isValidIdentifier = true;
                            }
                            else
                            {
                                isValidIdentifier = false;
                            }
                            continue;
                        }

                        if(isValidIdentifier == true)
                        {
                            tokensList.Add(new Token(ETokenType.Identifier, temp));
                        }
                        else
                        {
                            //Todo:Generate an error of invalid identifier.
                        }
                        continue;
                    } // Handles identifiers only.
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

        /// <summary>
        /// Check if supplied character is a small letter.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns>True if the character is a small letter,otherwise,false.</returns>
        private bool IsSmallLetter(char c)
        {
            return (int)c >= 97 && (int)c < 123; // Range must be a-z;
            
        }

        /// <summary>
        /// This method checks if a special character is present in an identifier.
        /// </summary>
        /// <param name="c">The character to be checked.</param>
        /// <returns>True if it is a special character ,otherwise, false.</returns>
        private bool IsSpecialCharacter(char c)
        {
            string specialChar = @"\|!#$%&/()?»«@£§€{}.-'<>,";

            foreach (char character in specialChar)
            {
                if (c == character)
                    return true;
            }
            return false;
        }

        public void print()
        {
            foreach (var item in tokensList)
            {
                Console.WriteLine(item.OperatorType + "      " + item.OperatorValue );
            }
        }

    }
}
