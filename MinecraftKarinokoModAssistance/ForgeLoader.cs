using System.IO;

namespace MinecraftKarinokoModAssistance
{
    class ForgeLoader : IModLoader
    {
        public string LoaderName => BaseBehaviour.LauncherType.Forge.ToString();

        public string GetModsDirectory() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "mods");
    }
}