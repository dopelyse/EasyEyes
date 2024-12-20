using EasyEyes;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using VFXSelect.Data.Rows;

namespace VFXSelect.Data.Sheets {
    public class EmoteSheetLoader : SheetLoader<XivEmote, XivEmoteSelected> {
        public override void OnLoad() {
            var sheet = SheetManager.DataManager.GetExcelSheet<Emote>().Where( x => !string.IsNullOrEmpty( x.Name.ExtractText() ) );
            foreach( var item in sheet ) {
                var i = new XivEmote( item );
                if( i.PapFiles.Count > 0 ) {
                    Items.Add( i );
                }
            }
        }

        public override bool SelectItem( XivEmote item, out XivEmoteSelected selectedItem ) {
            selectedItem = null;
            var files = new List<Lumina.Data.FileResource>();
            try {
                foreach( var path in item.PapFiles ) {
                    var result = SheetManager.DataManager.FileExists( path );
                    if( result ) {
                        files.Add( SheetManager.DataManager.GetFile( path ) );
                    }
                }
                selectedItem = new XivEmoteSelected( item, files );
            }
            catch( Exception e ) {
                Services.Error( e, "Error reading emote file" );
                return false;
            }
            return true;
        }
    }
}
