using System;
using System.Collections.Generic;
using System.IO;
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

namespace WordleSovlerHelper
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var enteredUnknowns = Unknowns.Text;
            var splitEnteredText = enteredUnknowns.Split(',');
            var UnknownsText = splitEnteredText.ToList();


            var enteredValidLetters = ValidLetters.Text;
            var splitEnteredCharacters = enteredValidLetters.Split(',');
            var validLetters = splitEnteredCharacters.ToList();

            var foundPossiblities = Calculate(UnknownsText, validLetters);

            PossibleWords.Clear();
            foreach (var word in foundPossiblities)
            {
                PossibleWords.Text = (PossibleWords.Text + " " + word).ToUpperInvariant();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        static List<string> Calculate(List<string> unknowns, List<string> validLetters)
        {

            StreamReader sr = new StreamReader("validwordlewords.txt");

            string line = sr.ReadLine();

            List<string> validWordleWord = new List<string>();

            List<string> foundWords = new List<string>();

            while (line != null)
            {
                validWordleWord.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();


            List<string> list = unknowns;

            bool ListClean = true;
            if (DoesListHaveUnknowns(list))
            {
                while (ListClean)
                {
                    var NewList = new List<string>();
                    //Loop through each item in the main list and replace the unknowns with every valid letter
                    foreach (var item in list)
                    {
                        if (item.Contains("?"))
                        {
                            NewList.AddRange(CreateList(item, validLetters));
                        }
                    }
                    list = NewList;

                    ListClean = DoesListHaveUnknowns(list);

                }
                foreach (var item in list)
                {
                    if (validWordleWord.Contains(item.ToLower()))
                    {
                        foundWords.Add(item);
                    }
                }
            }
            return foundWords;
        }



        public static List<string> CreateList(string wordToIterate, List<string> validCharacters)
        {
            var list = new List<string>();
            for (var i = 0; i < wordToIterate.Length; i++)
            {
                if (wordToIterate[i] == '?')
                {
                    //Create new list replacing this character with all valid letters
                    foreach (var character in validCharacters)
                    {
                        StringBuilder newWord = new StringBuilder(wordToIterate);
                        newWord[i] = char.Parse(character);
                        list.Add(newWord.ToString());
                    }
                    break;
                }
            }

            return list;
        }

        public static bool DoesListHaveUnknowns(List<string> listToCheck)
        {
            bool unknownFound = false;

            foreach (var item in listToCheck)
            {
                if (item.Contains("?"))
                {
                    unknownFound = true;
                    break;
                }
            }

            return unknownFound;
        }
    }
}
