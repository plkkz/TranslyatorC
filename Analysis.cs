using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1_v45_6
{
    class Analysis
    {
        Dictionary<string, string> parts = new Dictionary<string, string>();
        

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

        public void Gate(RichTextBox box1, RichTextBox box2)
        {
            box1.Clear();

            parts = new Dictionary<string, string>();
            int i = 0;
            string subText = "";

            bool IsErrorFlag = false;

            foreach (char s in box2.Text)
            {
                try
                {
                    if (Lexems.IsOperator(subText) && (s == ' ' || s == '<' || s == '>' || s == ';'))
                    {
                        i++;
                        parts.Add(i.ToString() + " ", subText + " - Идентификатор - оператор;");
                        subText = "";
                    }
                    else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')'))
                    {
                        i++;
                        parts.Add(i.ToString() + " ", subText + " - Литератор;");
                        subText = "";
                    }
                    else if (Lexems.IsSeparator(subText) && (s == ' ' || s == ')' || char.IsDigit(s) || char.IsLetter(s)))
                    {
                        i++;
                        if (subText != "\n")
                        {
                            parts.Add(i.ToString() + " ", subText + " - Разделитель;");
                        }
                        else
                        {
                            i--;
                            //subText = "'/n'";
                            //parts.Add(i.ToString() + " ", subText + " - Разделитель (символ новой строки);");
                        }
                        subText = "";
                    }
                    else if (Lexems.IsIDVariable(subText) && !Lexems.IsOperator(subText) && (s == ' ' || s == '<' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == ',' || s == '(' || s == ')'))
                    {
                        i++;
                        parts.Add(i.ToString() + " ", subText + " - Идентификатор - переменная;");
                        subText = "";
                    }
                    else if (Lexems.IsErrorLetter(subText))
                    {
                        MessageBox.Show($"Ошибка в программе. Неизвестный символ '{subText}'.");
                        IsErrorFlag = true;
                        subText = "";
                        break;
                    }
                    else if (subText == Environment.NewLine || subText == " ")
                    {
                        subText = "";
                    }
                    subText += s;
                }
                catch (Exception)
                {
                    IsErrorFlag = true;
                    MessageBox.Show("Не удалось закончить лексический анализ.");
                }
            }
            box1.Clear();

            foreach (KeyValuePair<string, string> pair in parts)
            {
                box1.Text += pair.Key.PadRight(10) + " " + pair.Value + Environment.NewLine;
            }
            if(IsErrorFlag == false)
                MessageBox.Show("Лексический анализ закончен");
            else { MessageBox.Show("Лексический анализ закончен с ошибкой"); }
        }
    }
}

