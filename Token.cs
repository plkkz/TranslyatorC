using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace laba1_v45_6
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);
        }
        public enum TokenType
        {
            AS, MAIN, THEN, INT, BOOL, LITERAL, 
            IDENTIFIER, CURLYBRACECLOSE, STRING,  
            IF, ELSE, TRUE, FALSE, PLUS, MORE, LESS, 
            SEMICOLON, CURLYBRACEOPEN, AND, EXPR,
            MINUS, EQUAL, MULTIPLY, RPAR, LPAR, ENTER, 
            DIVISION, COMMA, VARIABLE, OR, NETERM
        }

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
             { "int",  TokenType.INT },
             { "string",  TokenType.STRING },
             { "bool", TokenType.BOOL },
             { "if",   TokenType.IF },
             { "else", TokenType.ELSE },
             { "&&", TokenType.AND },
             { "||", TokenType.OR },
             { "main", TokenType.MAIN },
             { "as",   TokenType.AS },
             { "then", TokenType.THEN },
        };

        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return SpecialWords.ContainsKey(word);
        }

        public static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
             { '#', TokenType.ENTER },
             { '(', TokenType.LPAR },
             { ')', TokenType.RPAR },
             { '-', TokenType.MINUS },
             { '=', TokenType.EQUAL },
             { '>', TokenType.MORE },
             { '<', TokenType.LESS },
             { '+', TokenType.PLUS },
             { '*', TokenType.MULTIPLY },
             { '/', TokenType.DIVISION },
             { ',', TokenType.COMMA },
             { ';', TokenType.SEMICOLON },
             { '{', TokenType.CURLYBRACEOPEN },
             { '}', TokenType.CURLYBRACECLOSE }
        };

        public static Dictionary<string, TokenType> Neterminals = new Dictionary<string, TokenType>()
        {
            { "<программа>", TokenType.NETERM },
            { "<спис_опис>", TokenType.NETERM },
            { "<опис>", TokenType.NETERM },
            { "<тип>", TokenType.NETERM },
            { "<спис_перем>", TokenType.NETERM },
            { "<спис_опер>", TokenType.NETERM },
            { "<опер>", TokenType.NETERM },
            { "<условн>", TokenType.NETERM },
            { "<присв>", TokenType.NETERM },
            { "<блок.опер>", TokenType.NETERM },
            { "<знак>", TokenType.NETERM },
            { "<операнд>", TokenType.NETERM }
        };

        public static bool IsSpecialSymbol(char ch)
        {
            return SpecialSymbols.ContainsKey(ch);
        }

        public static void PrintTokens(RichTextBox box3, List<Token> list, bool check)
        {
            int i = 0;
            box3.Clear();

            if (check == false)
            {
                foreach (var t in list)
                {
                    i++;

                    box3.Text += $"{i} {t} ";
                    box3.Text += Environment.NewLine;
                }
                Recognizer recognizer = new Recognizer(list);
                recognizer.Start();
            }

            if (check == true)
            {
                foreach (var t in list)
                {
                    i++;

                    box3.Text += $"{i} {t} ";
                    box3.Text += Environment.NewLine;
                }
                LR lr = new LR(list);
                lr.Start();
            }
        }
    }
}
