using System.Collections.Generic;

namespace VFXSelect.Data.Rows {
    public class XivNpc : XivNpcBase {
        public bool CSV_Defined = false;

        public XivNpc( Lumina.Excel.Sheets.ModelChara npc, Dictionary<int, string> NpcIdToName ) : base( npc ) {
            CSV_Defined = NpcIdToName.ContainsKey( RowId );
            if( CSV_Defined ) {
                Name = NpcIdToName[RowId];
            }
        }
    }
}
