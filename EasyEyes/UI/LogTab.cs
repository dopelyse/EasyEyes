using EasyEyes.Structs.Vfx;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static EasyEyes.Plugin;

namespace EasyEyes.UI {
    public class LogTab {
        public Plugin _plugin;
        public BaseVfx SpawnVfx = null;

        public LogTab(Plugin plugin ) {
            _plugin = plugin;
        }

        public string SelectedLogPath = "";
        public void Draw() {
            var ret = ImGui.BeginTabItem( "Recent VFXs##MainInterfaceTabs" );
            if( !ret ) return;

            var Id = "##Log";
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );
            if( _plugin.DoRecord ) {
                if(MainInterface.RemoveButton("Stop" + Id ) ) {
                    _plugin.DoRecord = false;
                }
            }
            else {
                if(MainInterface.OkButton("Record" + Id ) ) {
                    _plugin.DoRecord = true;
                }
            }
            ImGui.SameLine();
            if( ImGui.Button( "Reset" + Id ) ) {
                _plugin.ClearRecord();
                SelectedLogPath = "";
            }
            var disabled = string.IsNullOrEmpty( SelectedLogPath );
            // ========= ADD ========
            if( disabled ) ImGui.PushStyleVar( ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f );
            ImGui.SameLine();
            if( ImGui.Button( "Add To Blacklist" + Id ) && !disabled ) {
                _plugin.Configuration.AddPath( SelectedLogPath, out var newItem );
            }
            if( disabled ) ImGui.PopStyleVar();
            // ======== SPAWN / REMOVE =========
            if( SpawnVfx == null ) {
                if( disabled ) ImGui.PushStyleVar( ImGuiStyleVar.Alpha, ImGui.GetStyle().Alpha * 0.5f );
                ImGui.SameLine();
                if( ImGui.Button( "Spawn" + Id ) && !disabled ) {
                    ImGui.OpenPopup( "Spawn_Popup" );
                }
                if( disabled ) ImGui.PopStyleVar();
            }
            else {
                ImGui.SameLine();
                if( ImGui.Button( "Remove" + Id ) ) {
                    SpawnVfx?.Remove();
                    SpawnVfx = null;
                }
            }
            if( ImGui.BeginPopup( "Spawn_Popup" ) ) {
                if( ImGui.Selectable( "On Ground" ) ) {
                    SpawnVfx = new StaticVfx( _plugin, SelectedLogPath, _plugin.PluginInterface.ClientState.LocalPlayer.Position );
                }
                if( ImGui.Selectable( "On Self" ) ) {
                    SpawnVfx = new ActorVfx( _plugin, _plugin.PluginInterface.ClientState.LocalPlayer, _plugin.PluginInterface.ClientState.LocalPlayer, SelectedLogPath );
                }
                ImGui.EndPopup();
            }

            //=======================
            ImGui.BeginChild( Id + "Tree", new Vector2(-1, -1), true );
            List<RecordedItem> items = _plugin.Recorded; // TODO: filtering
            if( items.Count > 0 ) {
                MainInterface.DisplayVisible( items.Count, out int preItems, out int showItems, out int postItems, out float itemHeight );
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + preItems * itemHeight );
                int idx = 0;
                foreach( var item in items ) {
                    if( idx < preItems || idx > ( preItems + showItems ) ) { idx++; continue; }
                    if( ImGui.Selectable( item.path + Id + idx, SelectedLogPath == item.path ) ) {
                        SelectedLogPath = item.path;
                    }
                    idx++;
                }
                ImGui.SetCursorPosY( ImGui.GetCursorPosY() + postItems * itemHeight );
            }
            else {
                ImGui.Text( "Press [Record] to view recent VFXs..." );
            }
            ImGui.EndChild();
            ImGui.EndTabItem();
        }
    }
}
