using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1_v45_6
{
    internal class Recognizer
    {
        //throw new Exception($"{i + 1} : {tokens[i].Type}");

        List<Token> tokens;

        int i;

        public Recognizer(List<Token> tokens)
        {
            this.tokens = tokens;
        }


        public void Start()
        {
            i = 0;
            try
            {
                Program();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR!\nNUMBER: {ex.Message}");
            }
        }

        public void Program()
        {
            if (tokens[i].Type != Token.TokenType.MAIN)
                throw new Exception($"1\nSTRING: {i + 1} - Ожидалось: MAIN, а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.LPAR)
                throw new Exception($"2\nSTRING: {i + 1} - Ожидалось: LPAR, а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.RPAR)
                throw new Exception($"3\nSTRING: {i + 1} - Ожидалось: RPAR, а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.CURLYBRACEOPEN && tokens[i].Type != Token.TokenType.ENTER)
                throw new Exception($"4\nSTRING: {i + 1} - Ожидалось: CURLYBRACEOPEN, а получено: {tokens[i].Type}");
            Next();
            SkipEnter();

            SpisOpis();
            SpisOper();

            SkipEnter();

            if (tokens[i].Type != Token.TokenType.CURLYBRACECLOSE && tokens[i].Type != Token.TokenType.ENTER)
                throw new Exception($"5\nSTRING: {i + 1} - Ожидалось: CURLYBRACECLOSE, а получено: {tokens[i].Type}");

            SkipEnter();

            if (tokens[i].Type != Token.TokenType.CURLYBRACECLOSE && tokens[i].Type != Token.TokenType.ENTER)
                throw new Exception($"6\nSTRING: {i + 1} - Ожидалось: CURLYBRACECLOSE, а получено: {tokens[i].Type}");
        }

        public void SpisOpis()
        {
            if (tokens[i].Type != Token.TokenType.INT &&
               tokens[i].Type != Token.TokenType.BOOL &&
               tokens[i].Type != Token.TokenType.STRING)
                throw new Exception($"7\nSTRING: {i + 1} - Ожидалось: INT, BOOL, STRING, а получено: {tokens[i].Type}");
            Opis();
            SkipEnter();
            DopOpis();
            SkipEnter();
        }

        public void Opis()
        {
            if (tokens[i].Type != Token.TokenType.INT &&
               tokens[i].Type != Token.TokenType.BOOL &&
               tokens[i].Type != Token.TokenType.STRING)
                throw new Exception($"8\nSTRING: {i + 1} - Ожидалось: INT, BOOL, STRING, а получено: {tokens[i].Type}");
            Next();
            SpisPerem();
            SkipEnter();
        }

        public void SpisPerem()
        {
            if (tokens[i].Type == Token.TokenType.VARIABLE)
            {
                Next();
                X();
            }
            else throw new Exception($"9\nSTRING: {i + 1} - Ожидалось: VARIABLE (ID), а получено: {tokens[i].Type}");
            SkipEnter();
        }

        public void X()
        {
            switch (tokens[i].Type)
            {
                case Token.TokenType.COMMA:
                    alt2();
                    break;

                case Token.TokenType.SEMICOLON:
                    Next();
                    SkipEnter();
                    if (tokens[i].Type == Token.TokenType.INT ||
                        tokens[i].Type == Token.TokenType.STRING ||
                        tokens[i].Type == Token.TokenType.BOOL)
                    {
                        Opis();
                    }
                    Next();
                    break;

                default: throw new Exception($"10\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {tokens[i].Type}");
            }
            SkipEnter();
        }

        public void alt2()
        {
            if (tokens[i].Type != Token.TokenType.COMMA)
                throw new Exception($"11\nSTRING: {i + 1} - Ожидалось: COMMA, а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"12\nSTRING: {i + 1} - Ожидалось: VARIABLE (ID), а получено: {tokens[i].Type}");
            Next();
            X();

            SkipEnter();
        }

        public void DopOpis()
        {
            Next();
            switch (tokens[i].Type)
            {
                case Token.TokenType.COMMA:
                    DopOpis2();
                    Next();
                    break;

                case Token.TokenType.SEMICOLON:
                    Next();
                    SkipEnter();
                    break;

                default: throw new Exception($"13\nSTRING: {i + 1} - Ошибка в присвоении! получено {tokens[i].Type}");
            }
            SkipEnter();
        }

        public void DopOpis2()
        {
            if (tokens[i].Type != Token.TokenType.COMMA)
                throw new Exception($"14\nSTRING: {i + 1} - Ожидалось: COMMA, а получено: {tokens[i].Type}");
            Next();

            if(tokens[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"15\nSTRING: {i + 1} - Ожидалось: COMMA, а получено: {tokens[i].Type}");
            Oper();
            DopOper();
            SkipEnter();
        }

        public void Oper()
        {
            switch (tokens[i].Type)
            {
                case Token.TokenType.IF:
                    _If();
                    break;

                case Token.TokenType.VARIABLE:
                    Prisv();
                    break;

                case Token.TokenType.ELSE:
                    break;

                case Token.TokenType.CURLYBRACECLOSE:
                    break;

                //case Token.TokenType.ENTER:
                //    SkipEnter();
                //    break;

                default: throw new Exception($"16\nSTRING: {i + 1} - Ожидалось: IF или VARIABLE (ID), а получено: {tokens[i].Type}");
            }
        }

        public void _If()
        {
            if (tokens[i].Type != Token.TokenType.IF)
                throw new Exception($"17\nSTRING: {i + 1} - Ожидалось: IF, а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.LPAR)
                throw new Exception($"18\nSTRING: {i + 1} - Ожидалось: LPAR, а получено: {tokens[i].Type}");
            Next();

            Expr();

            //if (tokens[i].Type != Token.TokenType.VARIABLE)
            //    throw new Exception($"17\nSTRING: {i + 1} - Ожидалось: VARIABLE, а получено: {tokens[i].Type}");

            BlockOper();

            DopYslov();
            SkipEnter();
        }

        public void Expr() //expr <знак> expr //заглушка на ехпр
        {
            while (tokens[i].Type != Token.TokenType.RPAR)
            {
                Next();
            }
            Next();
            SkipEnter();
        }

        public void BlockOper()
        {
            Oper();
            Next();
            
            if (tokens[i].Type == Token.TokenType.CURLYBRACEOPEN)
                Next();
                SpisOper();
            SkipEnter();
        }

        public void DopYslov()
        {
            switch (tokens[i].Type)
            {
                case Token.TokenType.VARIABLE:
                    DopYslov2();
                    break;

                case Token.TokenType.SEMICOLON:
                    break;

                case Token.TokenType.CURLYBRACECLOSE:
                    break;

                case Token.TokenType.ENTER:
                    SkipEnter();
                    break;

                default : throw new Exception($"19\nОшибка в идентификаторе IF, получено {tokens[i].Type}");
            }
        }

        public void DopYslov2()
        {
            if (tokens[i].Type == Token.TokenType.ELSE)
                Expr();
            SkipEnter();
        }

        public void Prisv()
        {
            if (tokens[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"20\nSTRING: {i + 1} - Ожидалось: VARIABLE (ID), а получено: {tokens[i].Type}");
            Next();
            if (tokens[i].Type != Token.TokenType.EQUAL)
                throw new Exception($"21\nSTRING: {i + 1} - Ожидалось: EQUAL, а получено: {tokens[i].Type}");
            Next();

            Operand();
            DopPrisv();
            SkipEnter();
        }

        public void SpisOper()
        {
            SkipEnter();
            if (tokens[i].Type != Token.TokenType.IF && tokens[i].Type != Token.TokenType.VARIABLE 
                && tokens[i].Type != Token.TokenType.CURLYBRACECLOSE && tokens[i].Type != Token.TokenType.ENTER)
                throw new Exception($"22\nSTRING: {i + 1} - Ожидалось: IF или VARIABLE (ID), а получено: {tokens[i].Type}");
            //Next();
            Oper();
            DopOper();
            SkipEnter();
        }

        public void DopPrisv()
        {
            switch (tokens[i].Type)
            {
                case Token.TokenType.SEMICOLON:
                    Next();
                    SkipEnter();
                    Oper();
                    break;

                case Token.TokenType.VARIABLE:
                    Next();
                    DopPrisv2();
                    break;

                case Token.TokenType.PLUS:
                    DopPrisv2();
                    break;

                default: throw new Exception($"23\nSTRING: {i + 1} - Ожидалось: SEMICOLON или VARIABLE (ID), а получено: {tokens[i].Type}");
            }
        }

        public void DopPrisv2()
        {
            Sign();
            Operand();

            if (tokens[i + 1].Type == Token.TokenType.VARIABLE)
                Next();
                Prisv();
            SkipEnter();
        }

        public void DopOper()
        {
            switch (tokens[i].Type)
            {
                case Token.TokenType.SEMICOLON:
                    break;

                case Token.TokenType.COMMA:
                    Next();
                    DopOper2();
                    break;

                case Token.TokenType.ENTER:
                    SkipEnter();
                    break;

                case Token.TokenType.CURLYBRACECLOSE:
                    break;

                default: throw new Exception($"24\nSTRING: {i + 1} - Ожидалось: SEMICOLON или VARIABLE (ID), а получено: {tokens[i].Type}");
            }
        }

        public void DopOper2()
        {
            if (tokens[i].Type != Token.TokenType.COMMA)
                throw new Exception($"25\nSTRING: {i + 1} - Ожидалось: COMMA, а получено: {tokens[i].Type}");
            Next();
            Oper();
            DopOper();
            SkipEnter();
        }

        public void Operand()
        {
            if (tokens[i].Type != Token.TokenType.VARIABLE && tokens[i].Type != Token.TokenType.LITERAL)
                throw new Exception($"26\nSTRING: {i + 1} - Ожидалось: VARIABLE (ID) или LITERAL, а получено: {tokens[i].Type}");
            Next();
            SkipEnter();
        }

        public void Sign() //знак
        {
            if (tokens[i].Type != Token.TokenType.AND && tokens[i].Type != Token.TokenType.OR &&
                tokens[i].Type != Token.TokenType.PLUS && tokens[i].Type != Token.TokenType.MINUS)
                throw new Exception($"27\nSTRING: {i + 1} - Ожидалось: SIGN, а получено: {tokens[i].Type}");
            Next();
        }

        public void Next()
        {
            if (i < tokens.Count - 1)
            {
                i++;
            }

            SkipEnter();
        }

        public void SkipEnter()
        {
            while (tokens[i].Type == Token.TokenType.ENTER && i < tokens.Count - 1)
            {
                Next();
            }
        }
    }
}
