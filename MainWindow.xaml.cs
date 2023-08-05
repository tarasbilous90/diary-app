using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfDiaryApp
{
    public partial class MainWindow : Window
    {
        const string filePath = "diary.data"; // user input data file  
        DateTime currentDateTime = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            ReadFromFile(currentDateTime.ToString("dd/MM/yyyy"));
        }
        // function that handles "Write Down" button click
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //write user input to diary.data file 
            if (File.Exists(filePath))
            {
                WriteToFile(textBox.Text, true);
            }
            else
            {
                WriteToFile(textBox.Text, false);
            }
            //read form diary.data file
            ReadFromFile(currentDateTime.ToString("dd/MM/yyyy"));
            textBox.Text = string.Empty;
        }

        private void WriteToFile(string text, bool appendToFile)
        {
            currentDateTime = DateTime.Now;
            string date = currentDateTime.ToString("dd/MM/yyyy");
            string time = currentDateTime.ToString("HH:mm");
            if (textBox.Text != string.Empty)
            {
                using (StreamWriter sw = new StreamWriter(filePath, appendToFile))
                {
                    sw.WriteLine($"[{date}][{time}] {text}");
                }
            }

        }

        private void ReadFromFile(string selectedDate)
        {
            if (File.Exists(filePath)) 
            {
                textBlock.Text = string.Empty;
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    string lineOutput;
                    string prevLine = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string date = line.Substring(1, 10);
                        string time = line.Substring(13, 5);
                        if (selectedDate == date)
                        {
                            lineOutput = line.Replace($"[{date}]", string.Empty);
                            if (prevLine != string.Empty && prevLine.Substring(13, 5) == time)
                            {
                                lineOutput = lineOutput.Replace(time, "...");
                            }
                            textBlock.Text += lineOutput + "\n";
                            prevLine = line;
                        }
                    }
                }
            }
        }
        //reading user data that match a date selected with the calendar
        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue)
            {
                DateTime selectedDateTime = calendar.SelectedDate.Value;
                ReadFromFile(selectedDateTime.ToString("dd/MM/yyyy"));
            }
        }
    }
}
