using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftKarinokoModAssistance
{
    class FabricLoader : IModLoader
    {
        public string LoaderName => BaseBehaviour.LauncherType.Fabric.ToString();
        
        public string GetModsDirectory() => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft", "installations");
    }
}
