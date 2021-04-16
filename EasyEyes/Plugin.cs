using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Game.Command;
using Dalamud.Plugin;
using ImGuiNET;
using System.Reflection;
using EasyEyes.UI;

namespace EasyEyes {
    public class Plugin : IDalamudPlugin {
        public string Name => "EasyEyes";
        private const string CommandName = "/easy";

        public DalamudPluginInterface PluginInterface;
        public Configuration Configuration;
        public ResourceLoader ResourceLoader;

        public MainInterface MainUI;

        public string PluginDebugTitleStr;
        public string AssemblyLocation { get; set; } = Assembly.GetExecutingAssembly().Location;
        public string FileLocation;
        public static readonly string DUMMY_VFX = "vfx/common/eff/cmma_shoot1c.avfx";

        public void Initialize( DalamudPluginInterface pluginInterface ) {
            PluginInterface = pluginInterface;

            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize( PluginInterface );
            ResourceLoader = new ResourceLoader( this );
            PluginInterface.CommandManager.AddHandler( CommandName, new CommandInfo( OnCommand ) {
                HelpMessage = "toggle ui"
            } );

            FileLocation = Path.Combine( Path.GetDirectoryName( AssemblyLocation ), "Files", "default_vfx.avfx" );

            ResourceLoader.Init();
            ResourceLoader.Enable();
            MainUI = new MainInterface( this );
            PluginInterface.UiBuilder.OnBuildUi += MainUI.Draw;
        }

        public void Dispose() {
            PluginInterface.UiBuilder.OnBuildUi -= MainUI.Draw;

            PluginInterface.CommandManager.RemoveHandler( CommandName );
            PluginInterface?.Dispose();
            MainUI?.Dispose();
            ResourceLoader?.Dispose();
        }

        private void OnCommand( string command, string rawArgs ) {
            MainUI.Visible = !MainUI.Visible;
        }

        public struct RecordedItem {
            public string path;
        }

        public List<RecordedItem> Recorded = new List<RecordedItem>();
        public bool DoRecord = false;
        public void AddRecord( string path ) {
            if( !DoRecord ) return;

            var item = new RecordedItem {
                path = path
            };
            Recorded.Add( item );
        }

        public void ClearRecord() {
            Recorded.Clear();
        }
    }
}