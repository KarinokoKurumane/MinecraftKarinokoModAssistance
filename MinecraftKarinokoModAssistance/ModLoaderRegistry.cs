namespace MinecraftKarinokoModAssistance
{
    public static class ModLoaderRegistry
    {
        /// <summary>
        /// Lista loaderów modów Minecraft.
        /// </summary>
        public static Dictionary<string, IModLoader> ModLoaders { get; } = new(5);

        /// <summary>
        /// Wybrany aktualnie loader modów.
        /// </summary>
        public static IModLoader? CurrentLoader { get; set; }

        /// <summary>
        /// Rejestruje domyślne loadery modów Minecraft. Forge i Fabric na początek.
        /// </summary>
        public static void RegisterDefaultLoaders()
        {
            ModLoaders.Add(BaseBehaviour.LauncherType.Forge.ToString(), new ForgeLoader());
            ModLoaders.Add(BaseBehaviour.LauncherType.Fabric.ToString(), new FabricLoader());
        }
    }
}