using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static laba1_v45_6.Token;

namespace laba1_v45_6
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        int exprCount = 0;
        public LR(List<Token> enterToken)
        {
            tokens = enterToken;
        }

        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            if (tokens.Count == nextLex)
                throw new Exception("Список лексем пуст");
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }

        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }

        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }

        private void Expression()//заглушка на ехпр
        {

            Expresion expresion = new Expresion();
            while (lexemStack.Peek().Type != TokenType.RPAR)
            {
                expresion.TakeToken(lexemStack.Peek());
                exprCount++;
                Shift();
            }
            expresion.Start();
            Token expr = new Token(TokenType.EXPR);
            lexemStack.Push(expr);
            exprCount = 0;
        }
      

        private void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        default:
                            throw new Exception($"State0\n String: {nextLex} Ожидалось нетерминал <программа>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.MAIN:
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"State0\n String: {nextLex} Ожидалось терминал MAIN но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State1()
        {
            if (nextLex == tokens.Count)
                throw new Exception($"State1\n String: {nextLex} !Ожидалось LPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                switch (lexemStack.Peek().Type)
            {
                case TokenType.MAIN:
                    Shift();
                    break;
                case TokenType.LPAR:
                    GoToState(2);
                    break;
                default:
                    throw new Exception($"State1\n String: {nextLex} Ожидалось LPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State2()
        {
            if (nextLex == tokens.Count)
                throw new Exception($"State2\n String: {nextLex} Ожидалось RPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LPAR:
                    Shift();
                    break;
                case TokenType.RPAR:
                    GoToState(3);
                    break;
                default:
                    throw new Exception($"State2\n String: {nextLex} Ожидалось RPAR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State3()
        {
            if (nextLex == tokens.Count)
                throw new Exception($"State3\n String: {nextLex} Ожидалось CURLYBRACEOPEN, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            switch (lexemStack.Peek().Type)
            {
                case TokenType.RPAR:
                    Shift();
                    break;
                case TokenType.CURLYBRACEOPEN:
                    GoToState(4);
                    break;
                default:
                    throw new Exception($"State3\n String: {nextLex} Ожидалось CURLYBRACEOPEN, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State4()
        {
           
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            GoToState(5);
                            break;
                        case "<опис>":
                            GoToState(6);
                            break;
                        case "<тип>":
                            GoToState(7);
                            break;
                        default:
                            throw new Exception($"State4\n String: {nextLex} Ожидалось нетерминал <спис_опис>, <опис>, <тип>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.CURLYBRACEOPEN: 
                    if (nextLex == tokens.Count)
                throw new Exception($"State4\n String: {nextLex} Ожидалось терминал CURLYBRACEOPEN, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    Shift();
                    break;

                case TokenType.INT:
                    GoToState(8);
                    break;

                case TokenType.BOOL:
                    GoToState(9);
                    break;

                case TokenType.STRING:
                    GoToState(10);
                    break;

                default:
                    throw new Exception($"State4\n String: {nextLex} Ожидалось терминал INT, BOOL, STRING, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State5()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    

                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            if (nextLex == tokens.Count)
                        throw new Exception($"State5\n String: {nextLex} Ожидалось нетерминал <спис_опис>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                            Shift();
                            break;
                        default:
                            throw new Exception($"State5\n String: {nextLex} Ожидалось нетерминал <спис_опис>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(11);
                    break;
                default:
                    throw new Exception($"State5\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State6()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(1, "<спис_опис>");
            else
                throw new Exception($"State6\n String: {nextLex} Ожидалось нетерминал <опис>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State7()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    

                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            if (nextLex == tokens.Count)
                                throw new Exception($"State7\n String: {nextLex} Было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value} ");
                            Shift();
                            break;
                        case "<спис_перем>":
                            GoToState(12);
                            break;
                        default:
                            throw new Exception($"State7\n String: {nextLex} Ожидалось <спис_перем>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.VARIABLE:
                    GoToState(13);
                    break;

                default:
                    throw new Exception($"State7\n String: {nextLex} Ожидалось терминал VARIABLE (ID), но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State8()
        {
            if (lexemStack.Peek().Type == TokenType.INT)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State8\n String: {nextLex} Ожидалось терминал INT, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State9()
        {
            if (lexemStack.Peek().Type == TokenType.BOOL)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State9\n String: {nextLex} Ожидалось терминал BOOL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State10()
        {
            if (lexemStack.Peek().Type == TokenType.STRING)
                Reduce(1, "<тип>");
            else
                throw new Exception($"State10\n String: {nextLex} Ожидалось терминал STRING, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State11()
        {
           

            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:

                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(14);
                            break;
                        case "<опис>":
                            GoToState(15);
                            break;

                        case "<опер>":
                            GoToState(16);
                            break;

                        case "<условн>":
                            GoToState(17);
                            break;


                        case "<тип>":
                            GoToState(7);
                            break;

                        case "<присв>":
                            GoToState(19);
                            break;

                        default:
                            throw new Exception($"{state}State11\n String: {nextLex} Ожидалось нетерминал <спис_опер>, <опис>, <опер>, <условн>, <тип>, <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.SEMICOLON:
                    if (nextLex == tokens.Count)
                throw new Exception($"{state}State11\n String: {nextLex} Ожидалось терминал SEMICOLON,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(18);
                    break;
                case TokenType.INT:
                    GoToState(8);
                    break;
                case TokenType.BOOL:
                    GoToState(9);
                    break;
                case TokenType.STRING:
                    GoToState(10);
                    break;

                case TokenType.VARIABLE:
                    GoToState(20);
                    break;

                default:
                    throw new Exception($"{state}State11\n String: {nextLex} Ожидалось терминал IF, INT, BOOL, STRING, VARIABLE но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State12()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<спис_перем>":
                                

                                switch (GetLexeme(nextLex).Type)
                                {
                                    case TokenType.COMMA:
                                        if (nextLex == tokens.Count)
                                    throw new Exception($"State12\n String: {nextLex} ");
                                        Shift();
                                        break;
                                    case TokenType.SEMICOLON:
                                        Reduce(2, "<опис>");
                                        break;
                                    default:
                                        throw new Exception($"State12\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                                }
                                break;
                            default:
                                throw new Exception($"State12\n String: {nextLex} Ожидалось, <спис_перем>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                        }
                    }
                    break;
                case TokenType.COMMA:
                    GoToState(21);
                    break;

                default:
                    throw new Exception($"State12\n String: {nextLex} Ожидалось терминал COMMA, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }

        }

        private void State13()
        {
            if (lexemStack.Peek().Type == TokenType.VARIABLE)
                Reduce(1, "<спис_перем>");
            else
                throw new Exception($"State13\n String: {nextLex} Ожидалось VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State14()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        if (nextLex == tokens.Count)
                            throw new Exception($"State14\n String: {nextLex} Ожидалось <спис_опер>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");

                        switch (lexemStack.Peek().Value)
                        {
                            case "<спис_опер>":
                                Shift();
                                break;
                            default:
                                throw new Exception($"State14\n String: {nextLex} Ожидалось <спис_опер>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                        }
                    }
                    break;
                case TokenType.CURLYBRACECLOSE:
                    GoToState(26);
                    break;
                case TokenType.SEMICOLON:
                    GoToState(22);
                    break;
                default:
                    throw new Exception($"State14\n String: {nextLex} Ожидалось CURLYBRACECLOSE, SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State15()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опис>")
                Reduce(3, "<спис_опис>");
            else
                throw new Exception($"State15\n String: {nextLex} Ожидалось <опис>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
        }

        private void State16()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(1, "<спис_опер>");
            else
                throw new Exception($"State16\n String: {nextLex} Ожидалось <опер>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
        }

        private void State17()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условн>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"State17\n String: {nextLex} Ожидалось <условн>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
        }

        private void State18()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EXPR:
                    GoToState(23);
                    break;
                case TokenType.IF:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State18\n String: {nextLex} Ожидалось  IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    Expression();
                    break;

                default:
                    throw new Exception($"State18\n String: {nextLex} Ожидалось EXPR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State19()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присв>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"State19\n String: {nextLex} Ожидалось <присв>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
        }

        private void State20()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    GoToState(24);
                    break;
                case TokenType.VARIABLE:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State20\n String: {nextLex} Ожидалось VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                default:
                    throw new Exception($"State20\n String: {nextLex} Ожидалось EQUAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        public void State21()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    GoToState(25);
                    break;
                case TokenType.COMMA:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State21\n String: {nextLex} Ожидалось COMMA, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                default:
                    throw new Exception($"State21\n String: {nextLex} Ожидалось VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State22()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            GoToState(27);
                            break;
                        case "<условн>":
                            GoToState(17);
                            break;
                        case "<присв>":
                            GoToState(19);
                            break;
                        default:
                            throw new Exception($"State22\n String: {nextLex} Ожидалось нетерминал <опер>, <условн>, <присв>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.CURLYBRACECLOSE:
                    GoToState(26);
                    break;

                case TokenType.IF:
                    GoToState(18);
                    break;

                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                case TokenType.SEMICOLON:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State22\n String: {nextLex} Ожидалось терминал SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                default:
                    throw new Exception($"State22\n String: {nextLex} Ожидалось терминал IF, VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State23()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок.опер>":
                            GoToState(28);
                            break;
                        case "<опер>":
                            GoToState(29);
                            break;
                        case "<условн>":
                            GoToState(17);
                            break;
                        case "<присв>":
                            GoToState(19);
                            break;
                        default:
                            throw new Exception($"State23\n String: {nextLex} Ожидалось нетерминал <блок.опер>, <опер>, <условн>, <присв>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.EXPR:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State23\n String: {nextLex} Ожидалось EXPR, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(18);
                    break;
                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                case TokenType.CURLYBRACEOPEN:
                    GoToState(30);
                    break;
                default:
                    throw new Exception($"State23\n String: {nextLex} Ожидалось IF, VARIABLE, CURLYBRACEOPEN, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State24()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            GoToState(31);
                            break;
                        default:
                            throw new Exception($"State24\n String: {nextLex} Ожидалось нетерминал <операнд>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.EQUAL:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State23\n String: {nextLex} Ожидалось терминал EQUAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                case TokenType.LITERAL:
                    GoToState(33);
                    break;
                case TokenType.VARIABLE:
                    GoToState(32);
                    break;
                default:
                    throw new Exception($"State23\n String: {nextLex} Ожидалось терминал VARIABLE, LITERAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State25()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    Reduce(3, "<спис_перем>");
                    break;
                default:
                    throw new Exception($"State25\n String: {nextLex} Ожидалось терминал VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State26()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.CURLYBRACECLOSE:
                    Reduce(8, "<программа>");
                    break;
                default:
                    throw new Exception($"State26\n String: {nextLex} Ожидалось CURLYBRACECLOSE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State27() //+
        {

            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<опер>")
                Reduce(3, "<спис_опер>");
            else
                throw new Exception($"State27\n String: {nextLex} Ожидалось нетерминал <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State28() //+
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<блок.опер>":
                                switch (GetLexeme(nextLex).Type)
                                {
                                    case TokenType.ELSE:
                                        if (nextLex == tokens.Count)
                                            throw new Exception($"State28\n String: {nextLex} Ожидалось ELSE, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");

                                        Shift();
                                        break;
                                    case TokenType.SEMICOLON:
                                        Reduce(3, "<условн>");
                                        break;
                                    default:
                                        throw new Exception($"State28\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                                }
                                break;
                            default:
                                throw new Exception($"State28\n String: {nextLex} Ожидалось <блок.опер>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                        }
                    }
                    break;
                case TokenType.ELSE:
                    GoToState(34);
                    break;

                default:
                    throw new Exception($"State28\n String: {nextLex} Ожидалось ELSE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
           
        }

        private void State29()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<опер>":
                                if (nextLex == tokens.Count)
                                    throw new Exception($"State29\n String: {nextLex} Ожидалось <опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                                Shift();
                                break;
                            default:
                                throw new Exception($"State29\n");
                        }
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(35);
                    break;

                default:
                    throw new Exception($"State29\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State30()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(36);
                            break;
                        case "<опер>":
                            GoToState(16);
                            break;
                        case "<условн>":
                            GoToState(17);
                            break;
                        case "<присв>":
                            GoToState(19);
                            break;
                        default:
                            throw new Exception($"State30\n String: {nextLex} Ожидалось нетерминал <спис_опер>, <опер>, <условн>, <присв>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;
                case TokenType.IF:
                    GoToState(18);
                    break;
                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                case TokenType.CURLYBRACEOPEN:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State30\n String: {nextLex} Ожидалось терминал CURLYBRACEOPEN, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;
                default:
                    throw new Exception($"State30\n String: {nextLex} Ожидалось терминал VARIABLE, IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State31()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<операнд>":
                                if (nextLex == tokens.Count)
                                    throw new Exception($"State31\n String: {nextLex} Ожидался знак, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                                switch (GetLexeme(nextLex).Type)
                                {
                                    case TokenType.PLUS:
                                        Shift();
                                        break;
                                    case TokenType.MINUS:
                                        Shift();
                                        break;
                                    case TokenType.DIVISION:
                                        Shift();
                                        break;
                                    case TokenType.MULTIPLY:
                                        Shift();
                                        break;
                                    case TokenType.SEMICOLON:
                                        Reduce(3, "<присв>");
                                        break;
                                    default:
                                        throw new Exception($"State31\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                                }
                                break;
                            case "<знак>":
                                GoToState(37);
                                break;
                            default:
                                throw new Exception($"State31\n String: {nextLex} Ожидалось <операнд>, <знак>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                        }
                    }
                    break;
                case TokenType.PLUS:
                    GoToState(38);
                    break;
                case TokenType.MINUS:
                    GoToState(39);
                    break;
                case TokenType.DIVISION:
                    GoToState(40);
                    break;
                case TokenType.MULTIPLY:
                    GoToState(41);
                    break;
                //case TokenType.VARIABLE:
                //    GoToState(32);
                //    break;
                //case TokenType.LITERAL:
                //    GoToState(33);
                //    break;
                default:
                    throw new Exception($"State31\n String: {nextLex} Ожидалось PLUS, MINUS, DIVISION, MULTIPLY,  но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }
        private void State32()
        {
            if (lexemStack.Peek().Type == TokenType.VARIABLE)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"State32\n String: {nextLex} Ожидалось VARIABLE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State34()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок.опер>":
                            GoToState(42);
                            break;
                        case "<опер>":
                            GoToState(29);
                            break;
                        case "<условн>":
                            GoToState(17);
                            break;
                        case "<присв>":
                            GoToState(19);
                            break;

                        default:
                            throw new Exception($"State34\n String: {nextLex} Ожидалось нетерминал <блок.опер>, <опер>, <условн>, <присв>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;

                case TokenType.IF:
                    GoToState(18);
                    break;

                case TokenType.VARIABLE:
                    GoToState(20);
                    break;

                case TokenType.CURLYBRACEOPEN:
                    GoToState(30);
                    break;

                case TokenType.ELSE:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State34\n String: {nextLex} Ожидалось терминал ELSE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                    Shift();
                    break;

                default:
                    throw new Exception($"State34\n String: {nextLex} Ожидалось терминал CURLYBRACEOPEN, VARIABLE, IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State33()
        {
            if (lexemStack.Peek().Type == TokenType.LITERAL)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"State33\n String: {nextLex} Ожидалось LITERAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

        }

        private void State35()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(2, "<блок.опер>");
            else
                throw new Exception($"State35\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }    

        private void State36() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<спис_опер>":
                                if (nextLex == tokens.Count)
                                    throw new Exception($"State37\n String: {nextLex} Ожидалось <спис_опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

                                Shift();
                                break;
                            default:
                                throw new Exception($"State37\n");
                        }
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(43);
                    break;

                default:
                    throw new Exception($"State36\n String: {nextLex} Ожидалось SEMICOLON, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }

        private void State37() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    {
                        switch (lexemStack.Peek().Value)
                        {
                            case "<знак>":
                                if (nextLex == tokens.Count)
                                    throw new Exception($"State37\n String: {nextLex} Ожидалось <знак>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");

                                Shift();
                                break;
                            case "<операнд>":
                                GoToState(44);
                                break;
                            default:
                                throw new Exception($"State37\n String: {nextLex} Ожидалось <операнд>, но было получено {lexemStack.Peek().Type}{lexemStack.Peek().Value}");
                        }
                    }
                    break;
                case TokenType.VARIABLE:
                    GoToState(32);
                    break;
                case TokenType.LITERAL:
                    GoToState(33);
                    break;
                default:
                    throw new Exception($"State37\n String: {nextLex} Ожидалось VARIABLE, LITERAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");

            }
        }
        
        private void State38() 
        {
            if (lexemStack.Peek().Type == TokenType.PLUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State38\n String: {nextLex} Ожидалось PLUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State39()
        {
            if (lexemStack.Peek().Type == TokenType.MINUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State39\n String: {nextLex} Ожидалось MINUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State40() 
        {
            if (lexemStack.Peek().Type == TokenType.DIVISION)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State40\n String: {nextLex} Ожидалось DIVISION, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State41() 
        {
            if (lexemStack.Peek().Type == TokenType.MULTIPLY)
                Reduce(1, "<знак>");
            else
                throw new Exception($"State41\n String: {nextLex} Ожидалось MULTIPLY, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State42()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок.опер>")
                Reduce(5, "<условн>");
            else
                throw new Exception($"State42\n String: {nextLex} Ожидалось нетерминал <блок.опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State43()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            GoToState(27);
                            break;
                        case "<условн>":
                            GoToState(17);
                            break;
                        case "<присв>":
                            GoToState(19);
                            break;
                        default:
                            throw new Exception($"State43\n String: {nextLex} Ожидалось нетерминал <опер>, <условн>, <присв>, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
                    }
                    break;               

                case TokenType.IF:
                    GoToState(18);
                    break;

                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                
                           
                case TokenType.CURLYBRACECLOSE:
                    GoToState(45);
                    break;
                case TokenType.SEMICOLON:
                    if (nextLex == tokens.Count)
                        throw new Exception($"State43\n String: {nextLex}");

                    Shift();
                    break;

                default:
                     throw new Exception($"State43\n String: {nextLex} Ожидалось IF, VARIABLE, CURLYBRACECLOSE, а получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            }
        }

        private void State44()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<операнд>")
                Reduce(5, "<присв>");
            else
                throw new Exception($"State44\n String: {nextLex} Ожидалось нетерминал <операнд>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

        private void State45()
        {
            if (lexemStack.Peek().Type == TokenType.CURLYBRACECLOSE)
                Reduce(4, "<блок.опер>");
            else
                throw new Exception($"State45\n String: {nextLex} Ожидалось CURLYBRACECLOSE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
            
            //if (lexemStack.Peek().Type == TokenType.CURLYBRACECLOSE)
            //    throw new Exception($"State45\n String: {nextLex} Ожидалось конец программы, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");


        }
        public void Start()
        {
            try
            {
                stateStack.Push(0);
                while (isEnd != true)
                    switch (state)
                    {
                        case 0:
                            State0();
                            break;
                        case 1:
                            State1();
                            break;
                        case 2:
                            State2();
                            break;
                        case 3:
                            State3();
                            break;
                        case 4:
                            State4();
                            break;
                        case 5:
                            State5();
                            break;
                        case 6:
                            State6();
                            break;
                        case 7:
                            State7();
                            break;
                        case 8:
                            State8();
                            break;
                        case 9:
                            State9();
                            break;
                        case 10:
                            State10();
                            break;
                        case 11:
                            State11();
                            break;
                        case 12:
                            State12();
                            break;
                        case 13:
                            State13();
                            break;
                        case 14:
                            State14();
                            break;
                        case 15:
                            State15();
                            break;
                        case 16:
                            State16();
                            break;
                        case 17:
                            State17();
                            break;
                        case 18:
                            State18();
                            break;
                        case 19:
                            State19();
                            break;
                        case 20:
                            State20();
                            break;
                        case 21:
                            State21();
                            break;
                        case 22:
                            State22();
                            break;
                        case 23:
                            State23();
                            break;
                        case 24:
                            State24();
                            break;
                        case 25:
                            State25();
                            break;
                        case 26:
                            State26();
                            break;
                        case 27:
                            State27();
                            break;
                        case 28:
                            State28();
                            break;
                        case 29:
                            State29();
                            break;
                        case 30:
                            State30();
                            break;
                        case 31:
                            State31();
                            break;
                        case 32:
                            State32();
                            break;
                        case 33:
                            State33();
                            break;
                        case 34:
                            State34();
                            break;
                        case 35:
                            State35();
                            break;
                        case 36:
                            State36();
                            break;
                        case 37:
                            State37();
                            break;
                        case 38:
                            State38();
                            break;
                        case 39:
                            State39();
                            break;
                        case 40:
                            State40();
                            break;
                        case 41:
                            State41();
                            break;
                        case 42:
                            State42();
                            break;
                        case 43:
                            State43();
                            break;
                        case 44:
                            State44();
                            break;
                        case 45:
                            State45();
                            break;
                    }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Error! {ex.Message}");
            }
            
        }
    }
}