using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace BatchCteateGrid
{
    internal class ExternalCommand2 : IExternalEventHandler
    {
        private Command myCom;

        public ExternalCommand2(Command myCom)
        {
            this.myCom = myCom;
        }

        public void Execute(UIApplication app)
        {
            using (Transaction trans = new Transaction(myCom.doc, "清除轴网"))
            {
                trans.Start();
                FilteredElementCollector collector = new FilteredElementCollector(myCom.doc);
                List<Element> elements = collector.OfClass(typeof(Grid)).ToList();
                foreach (var item in elements)
                {
                    myCom.doc.Delete(item.Id);
                }
                trans.Commit();
            }
        }

        public string GetName()
        {
            return "删除轴网";
        }
    }
}