using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace BatchCteateGrid
{
    internal class ExternalCommand3 : IExternalEventHandler
    {
        Command mycom;
        public ExternalCommand3(Command command)
        {
            mycom = command;

        }
        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            View view = doc.ActiveView;
            ReferenceArray refArray1 = new ReferenceArray();
            ReferenceArray refArray2 = new ReferenceArray();
            ReferenceArray refArray3 = new ReferenceArray();
            ReferenceArray refArray4 = new ReferenceArray();
            foreach (var item in mycom.XGrid)
            {
                refArray1.Append(new Reference(item));
            }
            refArray2.Append(new Reference(mycom.XGrid.First()));
            refArray2.Append(new Reference(mycom.XGrid.Last()));

            foreach (var item in mycom.YGrid)
            {
                refArray3.Append(new Reference(item));
            }
            refArray4.Append(new Reference(mycom.YGrid.First()));
            refArray4.Append(new Reference(mycom.YGrid.Last()));

            Line xline1 = Line.CreateBound(new XYZ(mycom.O.X, mycom.O.Y - 1800 / 304.8, 0),
                new XYZ(mycom.O.X + mycom.XtotalDistances, mycom.O.Y - 1800 / 304.8, 0));

            Line xline2 = Line.CreateBound(new XYZ(mycom.O.X, mycom.O.Y - 2500 / 304.8, 0),
                new XYZ(mycom.O.X + mycom.XtotalDistances, mycom.O.Y - 2500 / 304.8, 0));

            Line xline3 = Line.CreateBound(new XYZ(mycom.O.X + mycom.XtotalDistances, mycom.O.Y + mycom.YtotalDistances + 1800 / 304.8, 0),
                new XYZ(mycom.O.X, mycom.O.Y + mycom.YtotalDistances + 1800 / 304.8, 0));

            Line xline4 = Line.CreateBound(new XYZ(mycom.O.X + mycom.XtotalDistances, mycom.O.Y + mycom.YtotalDistances + 2500 / 304.8, 0)
                , new XYZ(mycom.O.X, mycom.O.Y + mycom.YtotalDistances + 2500 / 304.8, 0));

            Line yline1 = Line.CreateBound(new XYZ(mycom.O.X - 1800 / 304.8, mycom.O.Y, 0),
                new XYZ(mycom.O.X - 1800 / 304.8, mycom.O.Y + mycom.YtotalDistances, 0));

            Line yline2 = Line.CreateBound(new XYZ(mycom.O.X - 2500 / 304.8, mycom.O.Y, 0),
                new XYZ(mycom.O.X - 2500 / 304.8, mycom.O.Y + mycom.YtotalDistances, 0));
            Line yline3 = Line.CreateBound(new XYZ(mycom.O.X + mycom.XtotalDistances + 1800 / 304.8, mycom.O.Y + mycom.YtotalDistances, 0)
    , new XYZ(mycom.O.X + mycom.XtotalDistances + 1800 / 304.8, mycom.O.Y, 0));

            Line yline4 = Line.CreateBound(new XYZ(mycom.O.X + mycom.XtotalDistances + 2500 / 304.8, mycom.O.Y + mycom.YtotalDistances, 0)
                , new XYZ(mycom.O.X + mycom.XtotalDistances + 2500 / 304.8, mycom.O.Y, 0));
            using (Transaction trans = new Transaction(doc, "标注轴网"))
            {
                trans.Start();
                doc.Create.NewDimension(view, xline1, refArray1);
                doc.Create.NewDimension(view, xline2, refArray2);
                doc.Create.NewDimension(view, xline3, refArray1);
                doc.Create.NewDimension(view, xline4, refArray2);

                doc.Create.NewDimension(view, yline1, refArray3);
                doc.Create.NewDimension(view, yline2, refArray4);
                doc.Create.NewDimension(view, yline3, refArray3);
                doc.Create.NewDimension(view, yline4, refArray4);
                trans.Commit();
            }
        }
        public string GetName()
        {
            return "标注轴网";
        }
    }
}