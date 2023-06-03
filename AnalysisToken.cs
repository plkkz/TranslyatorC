using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace laba1_v45_6
{
    internal class AnalysisToken
    {
        public List<string> forToken = new List<string>();
        public List<string> listBuf = new List<string>();
        public List<char> forChar = new List<char>();
        public List<Token> tokens = new List<Token>();

        string str;
        string type;
        char ch;
        Token token;

        public static void PathToFile(System.Windows.Forms.TextBox box1, RichTextBox box2)
        {
            string text = "";

            using (StreamReader fs = new StreamReader(box1.Text))
            {
                while (true)
                {
                    string temp = fs.ReadLine();
                    if (temp == null) break;
                    text += temp;
                    text += " \n  ";
                }
                box2.Text = text;
            }
        }

        public void ReWork(RichTextBox box2, RichTextBox box3, bool check)
        {
            box3.Clear();

            listBuf = new List<string>();
            forToken = new List<string>();
            forChar = new List<char>();

            int i = 0;
            string subText = "";

            foreach (char s in box2.Text)
            {
                //try
                {
                    if (Lexems.IsOperator(subText) && (s == '(' || s == ' ' || s == '<' || s == '>' || s == ';'))
                    {
                        i++;
                        listBuf.Add(subText + " "); // - Идентификатор - оператор;
                        forToken.Add("I");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')'))
                    {
                        i++;
                        listBuf.Add(subText + " "); // - Литерал;
                        forToken.Add("D");
                        forChar.Add(' ');

                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText))
                    {
                        i++;
                        if (subText != "\n")
                        {
                            listBuf.Add(subText + " "); // - Разделитель;
                            forToken.Add("R");
                            forChar.Add(s);
                        }
                        //else
                        //{
                        //    subText = "#";
                        //    listBuf.Add(subText + " "); // - разделитель (символ новой строки);
                        //    forToken.Add("r");
                        //    forChar.Add(s);
                        //}
                        subText = "";
                    }
                    else if (Lexems.IsIDVariable(subText) && !Lexems.IsOperator(subText) && (s == ' ' || s == '<' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == ',' || s == '(' || s == ')'))
                    {
                        i++;
                        listBuf.Add(subText + " "); // - Идентификатор - переменная;
                        forToken.Add("P");
                        forChar.Add(' ');
                        subText = "";
                    }
                    else if (subText == Environment.NewLine || subText == " ")
                    {
                        subText = "";
                    }
                    subText += s;
                }
                //catch (Exception) { box3.Text += $"Не удалось идентифицировать тип токена.\n"; }
            }
            MessageBox.Show("Лексический анализ закончен");

            //-------------------------------------------------------------//

            tokens.Clear();

            for (i = 0; i < listBuf.Count; i++)
            {
                str = listBuf[i].Split(' ')[0];
                type = forToken[i];

                if (type == "I")
                {
                    try
                    {
                        if (Token.IsSpecialWord(str))
                        {
                            Token token = new Token(Token.SpecialWords[str]);
                            token.Value = str;
                            tokens.Add(token);
                        }
                    }
                    catch (Exception)
                    {
                        box3.Text += $"Непредвиденная ошибка в поиске специального слова\n";
                    }
                }
                else if (type == "D")
                {
                    try
                    {
                        token = new Token(Token.TokenType.LITERAL);
                        token.Value = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        box3.Text += $"Непредвиденная ошибка в поиске литерала";
                    }
                }
                else if (type == "P")
                {
                    try
                    {
                        token = new Token(Token.TokenType.VARIABLE);
                        token.Value = str;
                        tokens.Add(token);
                        continue;
                    }
                    catch (Exception)
                    {
                        box3.Text += $"Непредвиденная ошибка в поиске переменной";
                    }
                }
                else if (type == "R")
                {
                    try
                    {
                        for (int j = 0; j < str.Length; j++)
                        {
                            ch = str[j];
                            if (Token.IsSpecialSymbol(ch))
                            {
                                token = new Token(Token.SpecialSymbols[ch]);
                                token.Value = str;
                                tokens.Add(token);
                                continue;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        box3.Text += $"Непредвиденная ошибка в поиске разделителей";
                    }
                }
                else if (type == "r")
                {
                    //try
                    //{
                    //    token = new Token(Token.TokenType.ENTER);
                    //    tokens.Add(token);
                    //    continue;
                    //}
                    //catch (Exception)
                    //{
                    //    box3.Text += $"Непредвиденная ошибка в поиске символа новой строки";
                    //}
                }
            }
            //try
            {
                Token.PrintTokens(box3, tokens, check);
            }
            //catch(Exception)
            {
                
            }
        }
    }
}     