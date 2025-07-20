namespace MinecraftKarinokoModAssistance
{
    /// <summary>
    /// Interfejs dla loaderów modów Minecraft.
    /// </summary>
    public interface IModLoader
    {
        /// <summary>
        /// Zawiera nazwę loadera modów, np. Forge, Fabric itp.
        /// </summary>
        string LoaderName { get; }

        /// <summary>
        /// Metoda odpowiedzialna za zrwócenie folderu z modami Minecraft dla danego Loadera.
        /// </summary>
        /// <returns></returns>
        string GetModsDirectory();
    }
}
