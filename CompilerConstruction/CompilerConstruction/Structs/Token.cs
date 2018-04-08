using CompilerConstruction.Enums;
using System;

namespace CompilerConstruction.Structs
{
    public struct Token
    {
        private ETokenType operatorType;
        private Object operatorValue;

        public Token(ETokenType operatorType,Object operatorValue)
        {
            this.operatorType = operatorType;
            this.operatorValue = operatorValue;
        }
    }
}
