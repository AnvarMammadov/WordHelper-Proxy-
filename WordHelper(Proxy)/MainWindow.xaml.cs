using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WordHelper_Proxy_
{
    public partial class MainWindow : Window
    {
        private WordProxy wordProxy;

        public MainWindow()
        {
            InitializeComponent();

            wordProxy = new WordProxy();

            listBox.Focus();
        }

        private void UpdateListBox()
        {
            string enteredWord = textBox.Text;

            List<string> similarWords = wordProxy.GetSimilarWords(enteredWord);

            listBox.ItemsSource = similarWords;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWord = listBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedWord))
            {
                textBlock.Text = textBlock.Text + " " + selectedWord;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListBox();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredWord = textBox.Text;

            wordProxy.AddWord(enteredWord);

            UpdateListBox();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string selectedWord = listBox.SelectedItem as string;
                if (!string.IsNullOrEmpty(selectedWord))
                {
                    textBlock.Text = textBlock.Text + " " + selectedWord;
                }
            }
        }

        private void ListBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Up || e.Key == System.Windows.Input.Key.Down)
            {
                textBox.Focus();
            }
        }
    }

    public class RealWord
    {
        private List<string> words;

        public RealWord()
        {
            var dir = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(dir);
            var currentDir = directoryInfo.Parent.Parent.Parent;
            var path = currentDir.FullName;
            words = File.ReadAllLines(path+"/Database"+"/Words.txt").ToList();
        }

        public List<string> GetSimilarWords(string enteredWord)
        {
            List<string> similarWords = words
                .Where(w => w.StartsWith(enteredWord, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return similarWords;
        }

        public void AddWord(string word)
        {
            words.Add(word);
        }
    }

    public class WordProxy
    {
        private RealWord realWord;

        public List<string> GetSimilarWords(string enteredWord)
        {
            if (realWord == null)
            {
                realWord = new RealWord();
            }

            List<string> similarWords = realWord.GetSimilarWords(enteredWord);

            return similarWords;
        }

        public void AddWord(string word)
        {
            if (realWord == null)
            {
                realWord = new RealWord();
            }

            realWord.AddWord(word);
        }
    }
}
