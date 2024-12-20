using EasyEyes;
using Lumina.Excel.Sheets;
using System;
using System.Linq;
using VFXSelect.Data.Rows;

namespace VFXSelect.Data.Sheets {
    public class MountSheeetLoader : SheetLoader<XivMount, XivMountSelected> {
        public override void OnLoad() {
            var sheet = SheetManager.DataManager.GetExcelSheet<Mount>().Where( x => !string.IsNullOrEmpty( x.Singular.ExtractText() ) );
            foreach( var item in sheet ) {
                Items.Add( new XivMount( item ) );
            }
        }

        public override bool SelectItem( XivMount item, out XivMountSelected selectedItem ) {
            selectedItem = null;
            var imcPath = item.GetImcPath();
            var result = SheetManager.DataManager.FileExists( imcPath );
            if( result ) {
                try {
                    var file = SheetManager.DataManager.GetFile<Lumina.Data.Files.ImcFile>( imcPath );
                    selectedItem = new XivMountSelected( file, item );
                }
                catch( Exception e ) {
                    Services.Error( e, "Error loading IMC file " + imcPath );
                    return false;
                }
            }
            return result;
        }
    }
}
