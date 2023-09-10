﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Translate_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Идентификатор - название переменных
        //Ключевое слово - список
        //Разделитель - пробел, таб, перенос
        //Оператор - список
        //Литерал - значение переменных
        //Комментарий - отделён // или /* */

        string[] keywords = { "alignas", "alignof", "andB", "and_eqB", "asma", "auto", "bitandB", "bitorB", "bool", "break", "case", "catch", "char", "char8_tc", "char16_t", "char32_t", "class", "complB", "conceptc", "const", "const_cast", "constevalc", "constexpr", "constinitc", "continue", "co_awaitc", "co_returnc", "co_yieldc", "decltype", "default", "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit", "exportc", "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int", "long", "mutable", "namespace", "new", "noexcept", "notB", "not_eqB", "nullptr", "operator", "orB", "or_eqB", "private", "protected", "public", "register reinterpret_cast", "requiresc", "return", "short", "signed", "sizeof", "static", "string", "static_assert", "static_cast", "struct", "switch", "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using Декларации", "using Директива", "virtual", "void", "volatile", "wchar_t", "while", "xorB", "xor_eqB", "cout", "using", "main", "endl", "cin" };
        string[] operators = { ";", "->", "[", "]", "(", ")", "++", "--", "typeid", "const_cast", "dynamic_cast", "reinterpret_cast", "static_cast", "sizeof", "~", "!", "-", "+", "&", "*", "/", "new", "delete", ">>", "<<", ">", "<", "=>", "<=", "==", "!=", "^", "|", "||", "&&", "?:", "=", "*=", "/=", "+=", "-=", "%=", ">>=", "<<=", "&=", "|=", "^=", "throw", ",", ".", ".*", "->*", "?", "" };
        string[] names = { "Ключевые слова", "Операторы", "Идентификаторы", "Литералы", "Комментарии", "Разделители" };

        private void add_output(ref int i, ref int j, string word)
        {
            output_textbox.Text += word + " ";
            i += j - 1;
            j = 0;
        }
        private bool skip_include(ref int i, ref int j)
        {
            if (string.Compare(input_textbox.Text.Substring(i, j), "#include") == 0)
            {
                while (input_textbox.Text[i] != '>' && input_textbox.Text[i] != '"') //скип до > или до первой " 
                    i++;

                if (input_textbox.Text[i] == '"')  //если скипнули до ", то идём до следующей кавычки
                    while (input_textbox.Text[i] != '"')
                        i++;
                return true;
            }
            return false;
        }
        private bool skip_comment(ref int i, ref int j)
        {
            if (string.Compare(input_textbox.Text.Substring(i, j), "//") == 0)
            {
                while (input_textbox.Text[i] != '\n') //просто скип до конца строки
                    i++;
                return true;
            }
            if (string.Compare(input_textbox.Text.Substring(i, j), "/*") == 0)
            {
                while (input_textbox.Text[i] != '*' && input_textbox.Text[i + 1] != '/') //скид до конца комментария
                    i++;
                i++;
                return true;
            }
            return false;
        }
        private bool skip_text(ref int i, ref int j)
        {
            if (input_textbox.Text[i] == '"' && i + 1 < input_textbox.Text.Length)
            {
                while (input_textbox.Text[i + 1] != '"' && i + 2 < input_textbox.Text.Length)
                    i++;
                i++;
                return true;
            }
            return false;
        }
        private bool find_keyword(ref int i, ref int j, string[] keywords)
        {
            if (skip_comment(ref i, ref j))
                return true;
            if (skip_include(ref i, ref j))
                return true;
            if (skip_text(ref i, ref j))
                return true;

            if (i != 0 && j < input_textbox.Text.Length - i)
            {
                char symb_after = input_textbox.Text[j + i];
                char symb_before = input_textbox.Text[i - 1];

                if (Char.IsSeparator(symb_after) == true || symb_after == '(' || symb_after == ')' || symb_after == ';')
                {
                    if (Char.IsSeparator(symb_before) == true || symb_before == '(' || symb_before == ')')
                    {
                        for (int z = 0; z < keywords.Length; z++)
                        {
                            if (string.Compare(input_textbox.Text.Substring(i, j), keywords[z]) == 0)
                            {
                                add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int z = 0; z < keywords.Length; z++)
                {
                    if (string.Compare(input_textbox.Text.Substring(i, j), keywords[z]) == 0)
                    {
                        output_textbox.Text += keywords[z] + "\n";
                        i += j - 1;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool find_identifier(ref int i, ref int j, string[] keywords, string[] operators)
        {
            if (skip_comment(ref i, ref j))
                return true;
            if (skip_include(ref i, ref j))
                return true;
            if (skip_text(ref i, ref j))
                return true;

            if (i != 0 && j < input_textbox.Text.Length - i - 1)
            {
                char symb_after = input_textbox.Text[j + i];
                char symb_before = input_textbox.Text[i - 1];
                string word = input_textbox.Text.Substring(i, j);

                if (Char.IsSeparator(symb_after) == true || input_textbox.Text[j + i + 1] == '\n' || Regex.IsMatch(symb_after.ToString(), @"[\ \+\-\/\*\=\(\)\[\]\{\}\<\>\;]") == true)
                {
                    if (Char.IsSeparator(symb_before) == true || symb_before == '\n' || Regex.IsMatch(symb_before.ToString(), @"[\ \+\-\/\*\=\(\)\[\]\{\}\<\>\;]") == true)
                    {
                        bool flag = true;

                        for (int f = 0; f < keywords.Length; f++)
                            if (string.Compare(word, keywords[f]) == 0) //чек на несовпадение с ключевыми словами
                                flag = false;

                        for (int f = 0; f < operators.Length; f++)
                            if (string.Compare(word, operators[f]) == 0) //чек на несовпадение с операторами
                                flag = false;

                        for (int k = 0; k < word.Length; k++)
                            if (Char.IsLetter(word[k]) == false && word[k] != '_' && word[k] != '-') //чек на отсутсвие символов
                                flag = false;

                        if (flag)
                        {
                            add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                            return true;
                        }
                    }
                }


            }
            return false;
        }
        private bool find_operator(ref int i, ref int j, string[] operators)
        {
            if (skip_comment(ref i, ref j))
                return false;
            if (skip_include(ref i, ref j))
                return false;
            if (skip_text(ref i, ref j))
                return true;

            if (i != 0 && j < input_textbox.Text.Length - i - 1)
            {
                char symb_after = input_textbox.Text[j + i];
                char symb_before = input_textbox.Text[i - 1];

                if (Char.IsSeparator(symb_after) == true || Char.IsLetter(symb_after) == true || Char.IsNumber(symb_after) == true || Regex.IsMatch(symb_after.ToString(), @"[\(\)\[\]\{\}\'\ ]$") == true || input_textbox.Text[j + i + 1] == '\n' || symb_after == '"' || symb_after == '\n')
                    if (Char.IsSeparator(symb_before) == true || Char.IsLetter(symb_before) == true || Char.IsNumber(symb_before) == true || Regex.IsMatch(symb_before.ToString(), @"[\(\)\[\]\{\}\'\ ]$") == true || symb_before == '"')
                        for (int z = 0; z < operators.Length; z++)
                        {
                            if (string.Compare(input_textbox.Text.Substring(i, j), operators[z]) == 0)
                            {
                                add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                                return true;
                            }
                        }
            }
            else
            {
                for (int z = 0; z < operators.Length; z++)
                {
                    if (string.Compare(input_textbox.Text.Substring(i, j), operators[z]) == 0)
                    {
                        add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                        return true;
                    }
                }
            }
            return false;
        }
        private bool find_literal(ref int i, ref int j)
        {
            if (skip_comment(ref i, ref j))
                return true;
            if (skip_include(ref i, ref j))
                return true;

            if (i != 0 && j < input_textbox.Text.Length - i)
            {
                char symb_after = input_textbox.Text[j + i];
                char symb_before = input_textbox.Text[i - 1];
                string word = input_textbox.Text.Substring(i, j);

                if (Char.IsSeparator(symb_after) == true || Regex.IsMatch(symb_after.ToString(), @"[\}\)\]\'\ \,\;]$") == true)
                {
                    if (Char.IsSeparator(symb_before) == true || Regex.IsMatch(symb_after.ToString(), @"[\{\(\[\'\ \,\;]$") == true)
                    {
                        bool flag = true;
                        for (int k = 0; k < word.Length; k++)
                            if (Char.IsNumber(word[k]) == false && word[k] != '.') //чек на отсутсвие символов
                                flag = false;

                        if (flag)
                        {
                            add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                            return true;
                        }
                    }
                }
                if (symb_after == '"' && symb_before == '"')
                {
                    add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                    i += 2;
                    return true;
                }
            }
            return false;
        }
        private bool find_comment(ref int i)
        {
            string word = input_textbox.Text.Substring(i, 2);
            int j = 2;

            if (string.Compare(word, "//") == 0)
            {
                while (input_textbox.Text[i + j] != '\n' && j < input_textbox.Text.Length - i)
                    j++;

                j--;

                add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                return true;
            }
            if (string.Compare(word, "/*") == 0)
            {
                while (input_textbox.Text.Substring(i + j - 2, 2) != "*/" && j < input_textbox.Text.Length - i)
                    j++;

                add_output(ref i, ref j, input_textbox.Text.Substring(i, j));
                return true;
            }

            return false;
        }
        private void find_separator(ref int i)
        {

            int count_probel = 0, count_enter = 0;

            for (i = 0; i < input_textbox.Text.Length - 2; i++)
            {
                string word = input_textbox.Text.Substring(i, 1);

                if (string.Compare(word, " ") == 0)
                {
                    count_probel++;
                }
                if (string.Compare(word, "\n") == 0)
                {
                    count_enter++;
                }
            }
            output_textbox.Text += "Пробелов: " + count_probel.ToString();
            output_textbox.Text += "\nПереносов строк: " + count_enter.ToString();
        }


        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";
            int i, j, n;

            for (n = 0; n < 4; n++)
            {
                output_textbox.Text += "● " + names[n] + "\n\n";

                for (i = 0; i < input_textbox.Text.Length; i++)
                {
                    for (j = 1; j < input_textbox.Text.Length - i && j < 20; j++)
                    {
                        if (n == 0)
                            if (find_keyword(ref i, ref j, keywords))
                                break;

                        if (n == 1)
                            if (find_operator(ref i, ref j, operators))
                                break;

                        if (n == 2)
                            if (find_identifier(ref i, ref j, keywords, operators))
                                break;

                        if (n == 3)
                            if (find_literal(ref i, ref j))
                                break;
                    }
                }
                output_textbox.Text += "\n\n";
            }

            output_textbox.Text += "● " + names[4] + "\n\n";

            for (i = 0; i < input_textbox.Text.Length - 2; i++)
            {
                find_comment(ref i);
            }

            output_textbox.Text += "\n\n● " + names[5] + "\n\n";

            find_separator(ref i);

        }
    }
}