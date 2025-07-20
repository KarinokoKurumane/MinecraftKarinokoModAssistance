using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using LoaderSettingsBehaviour;

namespace MinecraftKarinokoModAssistance
{
    internal static class BaseBehaviour
    {
        public const string DIRECTORY_JSON_CONFIG_FILE_NAME = "directory.json";
             
        public enum LauncherType
        {
            Forge,
            Fabric
        }

        /// <summary>
        /// Zawiera domyślną ścieżkę do folderu z modami Minecraft.
        /// </summary>
        [Obsolete]
        public static string defaultModsPath = string.Empty;
        /// <summary>
        /// Zawiera domyślną ścieżkę do folderu pobierania modów.
        /// </summary>
        [Obsolete]
        public static string defaultDownloadsPath = string.Empty;

        /// <summary>
        /// Zawiera typ modowania Minecraft, np. Forge, Fabric itp.
        /// </summary>
        public static LauncherType chosenModType = LauncherType.Forge;

        /// <summary>
        /// Lista modów Minecraft po stronie serwera.
        /// </summary>
        public static string[] serwerModsList = [];
        /// <summary>
        /// Lista plików modów Minecraft posiadanych.
        /// </summary>
        public static string[] ownModsList = [];

        /// <summary>
        /// Przygotowuje domyślne ścieżki dla folderów z modami i pobierania.
        /// </summary>
        [Obsolete]
        public static void SetDefaultPaths()
        {
            // Folder z modami Minecraft
            defaultModsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft", "mods");
            // Domyślny folder pobierania
            defaultDownloadsPath = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
               "Download");
        }

        /// <summary>
        /// Pobiera listę plików modów w podanym folderze.
        /// </summary>
        /// <param name="_folderPath"></param>
        /// <returns></returns>
        public static string[] GetFilesModsInDirectory(string _folderPath)
        {
            if (!Directory.Exists(_folderPath))
            {
                MessageBox.Show("Podany folder z modami nie istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return [];
            }

            string[] _files = Directory.GetFiles(_folderPath, "*.jar");
            string[] _result = [];
            foreach (string file in _files)
            {
                FileInfo _fi = new(file);
                _result = _result.Append(_fi.Name).ToArray();
            }

            return _result;
        }

        /// <summary>
        /// Weryfikuje i inicjalizuje folder z modami dla danego loadera.
        /// </summary>
        /// <param name="_loader"></param>
        /// <returns></returns>
        public static bool InitializeModsDirectory(IModLoader _loader)
        {
            string _modsPath = _loader.GetModsDirectory();

            if (!Directory.Exists(_modsPath))
            {
                return SelectNewModsDirectory(_loader);
            }
            return true;
        }

        /// <summary>
        /// Wybieranie nowego folderu z modami zależnie od Loadera
        /// </summary>
        /// <param name="_loader"></param>
        public static bool SelectNewModsDirectory(IModLoader _loader)
        {
            string _modsPath = string.Empty;
            MessageBox.Show(
                    $"Nie odnaleziono domyślnej ścieżki dla loadera {_loader.LoaderName}. Proszę wskazać folder ręcznie.",
                    "Folder modów nie istnieje",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            var _dialog = new OpenFolderDialog()
            {
                Title = $"Wskaż folder modów dla {_loader.LoaderName}",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            };

            try
            {
                if (_dialog.ShowDialog() == true)
                {
                    _modsPath = _dialog.FolderName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            LoaderOptions _options = new()
            {
                ModsDirectory = _modsPath
            };
            LoaderSettings.SaveLoaderSettings(_loader.LoaderName, _options);
            return true;
        }

        /// <summary>
        /// Usawia aktualnego loadera modów na podstawie klucza.
        /// </summary>
        /// <param name="_selectedKey"></param>
        /// <returns></returns>
        public static IModLoader? SetLoaderFromKey(string _selectedKey)
        {
            if (ModLoaderRegistry.ModLoaders.TryGetValue(_selectedKey, out var __loader))
            {
                ModLoaderRegistry.CurrentLoader = __loader;
                return __loader;
            }
            return null;
        }
    }
}
