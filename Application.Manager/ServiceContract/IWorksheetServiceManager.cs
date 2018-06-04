using Application.DTO.Worksheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager.ServiceContract
{
    public interface IWorksheetServiceManager
    {
        SheetSnapshot GetSheetSnapshotbyId(string Id);
        string AddSheet(SheetSnapshot snapshot);
        SheetSnapshot UpdateSheet(SheetSnapshot snapshot);


        SheetSnapshot SaveSheet(SheetSnapshot snapshot);
        SheetSnapshot DeleteSheet(SheetSnapshot snapshot);

        ActionResultSnapshot GetActionResultSnapshotbyId(string Id);
        string AddActionResult(ActionResultSnapshot snapshot);
        ActionResultSnapshot UpdateActionResult(ActionResultSnapshot snapshot);


        ActionResultSnapshot SaveActionResult(ActionResultSnapshot snapshot);
        ActionResultSnapshot DeleteActionResult(ActionResultSnapshot snapshot);
    }
}
