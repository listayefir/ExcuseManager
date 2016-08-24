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
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace ExcuseManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Excuse currentExcuse = new Excuse(); 
        private bool formChanged = false;
        private string folderName="";
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            datetimeLastUsed.DisplayDate=DateTime.Now;
            currentExcuse.LastUsed = datetimeLastUsed.DisplayDate;
        }

        private void UpdateForm(bool changed)
        {
            if (!changed)
            {
                tbExcuse.Text = currentExcuse.Description;
                tbResults.Text = currentExcuse.Results;
                datetimeLastUsed.SelectedDate = currentExcuse.LastUsed;
                
                if (!string.IsNullOrEmpty(currentExcuse.ExcusePath))
                    tbFileDate.Text = File.GetLastWriteTime(currentExcuse.ExcusePath).ToString();
                Title= "Excuse Manager";
                formChanged = changed;
            }
            else
            {
                Title = "Excuse Manager*";
                formChanged = changed;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbExcuse.Text) || string.IsNullOrEmpty(tbResults.Text))
            {
                MessageBox.Show("Please specify an excuse and the result", "Unable to save", MessageBoxButton.OK);
                return;
            }
                tbFileDate.Text = DateTime.Now.ToString();
                currentExcuse.LastUsed = (DateTime)datetimeLastUsed.SelectedDate;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = folderName;
                sfd.Filter = "Text documents (.txt)|*.txt|All files (*.*)|*.*";
            sfd.FileName = tbExcuse.Text + ".txt";
                currentExcuse.ExcusePath = string.Format(folderName + @"\" + tbExcuse.Text + ".txt");
                if (sfd.ShowDialog() == true)
                {
                    currentExcuse.Save(sfd.FileName);
                UpdateForm(false);
                    MessageBox.Show("Excuse written");
                }
        }

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();
            dlg.SelectedPath = folderName;
            var result = dlg.ShowDialog();
            if (result == true)
            {
                folderName = dlg.SelectedPath;
                btnOpen.IsEnabled = true;
                btnSave.IsEnabled = true;
                btnRandom.IsEnabled = true;
            }
            
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (CheckChanged())
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = folderName;
                dlg.Filter = "Text documents (.txt)|*.txt|All files (*.*)|*.*";
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    currentExcuse = new Excuse(dlg.FileName);
                    UpdateForm(false);
                }
            }
        }

        private bool CheckChanged()
        {
            if (formChanged)
            {
                var result = MessageBox.Show("The current excuse has not been saved. Continue?",
                    "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result==MessageBoxResult.No) return false;
            }
            return true;
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            if (CheckChanged())
            {
                currentExcuse = new Excuse(random, folderName);
                UpdateForm(false);
            }
        }

        private void tbExcuse_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentExcuse.Description = tbExcuse.Text;
            UpdateForm(true);
        }

        private void tbResults_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentExcuse.Results = tbResults.Text;
            UpdateForm(true);
        }

        private void datetimeLastUsed_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            currentExcuse.LastUsed = datetimeLastUsed.DisplayDate;
            UpdateForm(true);
        }
    }
}
