using Microsoft.Win32;
using MinecraftKarinokoModAssistance;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using static MinecraftKarinokoModAssistance.BaseBehaviour;

namespace MinecraftKarinokoModAssistance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ModLoaderRegistry.RegisterDefaultLoaders(); //wstępnie rejestrujemy domyślne loadery modów

            var _loaderWindow = new LauncherType();
            _loaderWindow.ShowDialog();

            InitializeComponent();
            SetDefaultPathsToTextbox();
        }


        /// <summary>
        /// Ustawia domyślne ścieżki dla folderów z modami i pobierania.
        /// </summary>
        private void SetDefaultPathsToTextbox()
        {
            SetDefaultPaths();
            Tbx_ModsDirectory.Text = ModLoaderRegistry.CurrentLoader.GetModsDirectory();
            Tbx_ModsDownloadDirectory.Text = defaultDownloadsPath;
        }

        /// <summary>
        /// reakcja na kliknięcie przycisku ustawiającego folder z modami.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_SetModsDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog _dialog = new()
            {
                Title = "Wybierz folder z modami Minecraft",
                InitialDirectory = defaultModsPath
            };
            if (_dialog.ShowDialog() == true)
            {
                Tbx_ModsDirectory.Text = _dialog.FolderName;
                defaultModsPath = _dialog.FolderName;
            }
        }

        /// <summary>
        /// Reakcja na kliknięcie przycisku ustawiającego folder z popbranymi modami.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ModsDownloadDirectory_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog _dialog = new()
            {
                Title = "Wybierz folder pobierania modów Minecraft",
                InitialDirectory = defaultDownloadsPath
            };
            if (_dialog.ShowDialog() == true)
            {
                Tbx_ModsDownloadDirectory.Text = _dialog.FolderName;
                defaultDownloadsPath = _dialog.FolderName;
            }
        }

        /// <summary>
        /// Reakcja na kliknięcie przycisku ładowania listy modów z pliku.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LoadSerwerListMods_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _dialog = new()
            {
                Title = "Wybierz plik z listą modów serwera",
                Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*",
                InitialDirectory = defaultDownloadsPath
            };
            if (_dialog.ShowDialog() == true)
            {
                string _filePath = _dialog.FileName;
                if (File.Exists(_filePath))
                {
                    try
                    {
                        serwerModsList = File.ReadAllLines(_filePath, Encoding.UTF8);
                        //Tbl_LoadedListData.Text = $"Załadowano {serwerModsList.Length} modów z pliku: {_filePath}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas odczytu pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Plik nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Reakcja na kliknięcie przycisku ładowania listy modów z folderu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_LoadOwnListMods_Click(object sender, RoutedEventArgs e)
        {
            if (defaultModsPath != null)
            {
                ownModsList = GetFilesModsInDirectory(defaultModsPath);
                //Tbl_LoadedOwnListData.Text = $"Załadowano {ownModsList.Length} modów z folderu: {defaultModsPath}";
            }
        }

        private void Btn_VerifyMods_Click(object sender, RoutedEventArgs e)
        {
            if (serwerModsList == null || ownModsList == null)
            {
                MessageBox.Show("Najpierw załaduj obie listy modów.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var missingMods = serwerModsList.Except(ownModsList, StringComparer.OrdinalIgnoreCase).ToList();

            Lbx_NeedMods.Items.Clear();
            foreach (var mod in missingMods)
            {
                Lbx_NeedMods.Items.Add(mod);
            }

            if (missingMods.Count == 0)
            {
                Lbx_NeedMods.Items.Add("Wszystkie mody są obecne!");
            }

        }
    }
}