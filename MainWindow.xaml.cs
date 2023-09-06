using System;
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

namespace Translate_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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


        string[] key_words = { "alignas", "alignof", "andB", "and_eqB", "asma", "auto", "bitandB", "bitorB", "bool", "break", "case", "catch", "char", "char8_tc", "char16_t", "char32_t", "class", "complB", "conceptc", "const", "const_cast", "constevalc", "constexpr", "constinitc", "continue", "co_awaitc", "co_returnc", "co_yieldc", "decltype", "default", "delete", "do", "double", "dynamic_cast", "else", "enum", "explicit", "exportc", "extern", "false", "float", "for", "friend", "goto", "if", "inline", "int", "long", "mutable", "namespace", "new", "noexcept", "notB", "not_eqB", "nullptr", "operator", "orB", "or_eqB", "private", "protected", "public", "register reinterpret_cast", "requiresc", "return", "short", "signed", "sizeof", "static", "static_assert", "static_cast", "struct", "switch", "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "union", "unsigned", "using Декларации", "using Директива", "virtual", "void", "volatile", "wchar_t", "while", "xorB", "xor_eqB" };
        string[] operators = { ";", "->", "[", "]", "(", ")", "++", "--", "typeid", "const_cast", "dynamic_cast", "reinterpret_cast", "static_cast", "sizeof", "~", "!", "-", "+", "&", "*", "/", "new", "delete", ">>", "<<", ">", "<", "=>", "<=", "==", "!=", "^", "|", "||", "&&", "?:", "=", "*=", "/=", "+=", "-=", "%=", ">>=", "<<=", "&=", "|=", "^=", "throw", ",", ".", ".*", "->*", "?", "" };

        private void parsing(object sender, RoutedEventArgs e)
        {
            output_textbox.Text = "";

            int i, j, z;
            char symb_before, symb_after;

            output_textbox.Text += "Ключевые слова\n\n";

            for (i = 0; i <= input_textbox.Text.Length; i++)
            {
                for (j = 1; j <= input_textbox.Text.Length - i && j < 20; j++)
                {
                    for (z = 0; z < key_words.Length; z++)
                    {
                        if (i != 0 && j != input_textbox.Text.Length - i)
                        {
                            symb_after = input_textbox.Text[j + i];
                            symb_before = input_textbox.Text[i - 1];
                            if (Char.IsSeparator(symb_after) == true || symb_after == '(' || symb_after == ')')
                                if (Char.IsSeparator(symb_before) == true || symb_before == '(' || symb_before == ')')
                                    if (string.Compare(input_textbox.Text.Substring(i, j), key_words[z]) == 0)
                                    {
                                        output_textbox.Text += key_words[z] + "\n";
                                        i += j;
                                        j = 1;
                                    }
                        }
                        else
                        {
                            if (string.Compare(input_textbox.Text.Substring(i, j), key_words[z]) == 0)
                            {
                                output_textbox.Text += key_words[z] + "\n";
                                i += j;
                                j = 1;
                            }
                            if (string.Compare(input_textbox.Text.Substring(i, j), "#include") == 0)
                            {
                                while (input_textbox.Text[i] != '>' && input_textbox.Text[i] != '"')
                                    i++;
                                if (input_textbox.Text[i] == '"')
                                    while (input_textbox.Text[i] != '"')
                                        i++;
                            }
                        }

                    }
                }
            }

            output_textbox.Text += "\n\nОператоры\n\n";

            for (i = 0; i <= input_textbox.Text.Length; i++)
            {
                for (j = 1; j <= input_textbox.Text.Length - i && j < 20; j++)
                {
                    for (z = 0; z < operators.Length; z++)
                    {
                        if (i != 0 && j < input_textbox.Text.Length - i - 1)
                        {
                            symb_after = input_textbox.Text[j + i];
                            symb_before = input_textbox.Text[i - 1];

                            if (Char.IsSeparator(symb_after) == true || Char.IsLetter(symb_after) == true || System.Text.RegularExpressions.Regex.IsMatch(symb_after.ToString(), @"[\(\)\[\]\ ]$") == true || input_textbox.Text[j + i + 1] == '\n')
                                if (Char.IsSeparator(symb_before) == true || Char.IsLetter(symb_before) == true || System.Text.RegularExpressions.Regex.IsMatch(symb_before.ToString(), @"[\(\)\[\]\ ]$") == true)
                                    if (string.Compare(input_textbox.Text.Substring(i, j), operators[z]) == 0)
                                    {
                                        output_textbox.Text += operators[z] + "\n";
                                        i += j;
                                        j = 1;
                                    }
                        }
                        else
                        {
                            if (string.Compare(input_textbox.Text.Substring(i, j), operators[z]) == 0)
                            {
                                output_textbox.Text += operators[z] + "\n";
                                i += j;
                                j = 1;
                            }
                            if (string.Compare(input_textbox.Text.Substring(i, j), "#include") == 0)
                            {
                                while (input_textbox.Text[i] != '>' && input_textbox.Text[i] != '"')
                                    i++;
                                if (input_textbox.Text[i] == '"')
                                    while (input_textbox.Text[i] != '"')
                                        i++;
                            }
                        }
                    }
                }
            }
        }
    }
}