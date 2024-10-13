using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BatchCteateGrid
{
    internal class ExternalCommand4 : IExternalEventHandler
    {
        Command command;
        CreateForm _createForm;
        public ExternalCommand4(CreateForm createForm, Command myCom)
        {
            command = myCom;
            _createForm = createForm;
        }
        public void Execute(UIApplication app)
        {
            ClearGrid();
            _createForm.getInfo();
            try
            {
                XYZ point = new XYZ(0, 0, 0);
                command.CreateGrids(_createForm.XInfo, _createForm.YInfo, new UIDocument(command.predoc), command.predoc, point);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("tip", ex.Message);
            }
        }
        public string GetName()
        {
            return "preGrid";
        }
        public void ClearGrid()
        {
            using (Transaction trans = new Transaction(command.predoc, "清除轴网"))
            {
                trans.Start();
                FilteredElementCollector collector = new FilteredElementCollector(command.predoc);
                List<Element> elements = collector.OfClass(typeof(Grid)) .ToList();
                foreach (var item in elements)
                {
                    command.predoc.Delete(item.Id);
                }
                trans.Commit();
            }
        }
    }
}