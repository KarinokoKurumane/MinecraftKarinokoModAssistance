using System.Windows;
using LoaderSettingsBehaviour;
using static MinecraftKarinokoModAssistance.BaseBehaviour;

namespace MinecraftKarinokoModAssistance
{
    /// <summary>
    /// Logika interakcji dla klasy LauncherType.xaml
    /// </summary>
    public partial class LauncherType : Window
    {
        public LauncherType()
        {
            InitializeComponent();
            LoadLoaders();
        }

        /// <summary>
        /// Wczytuje dostępne loadery modów z rejestru i dodaje je do listy wyboru.
        /// </summary>
        private void LoadLoaders()
        {
            try
            {
                foreach (var _key in ModLoaderRegistry.ModLoaders.Keys)
                {
                    Cbx_LoaderList.Items.Add(_key);
                }

                Cbx_LoaderList.SelectedIndex = 0;
                ModLoaderRegistry.CurrentLoader = SetLoaderFromKey(Cbx_LoaderList.SelectedItem as string); // na wypadek, gdyby użytkownik zamknął okno bez wyboru loadera
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania loaderów: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            var _loader = SetLoaderFromKey(Cbx_LoaderList.SelectedItem as string);
            ChceckLoader(_loader);
            ModLoaderRegistry.CurrentLoader = _loader;
        }

        private void ChceckLoader(IModLoader _loader)
        {
            if (_loader != null)
            {
                InitializeModsDirectory(_loader);
                this.Close();
            }
            else
            {
                var _newPath = LoaderSettings.LoadLoaderSettings(_loader.LoaderName).ModsDirectory;
                SelectNewModsDirectory(_loader);
            }
        }

    }
}
