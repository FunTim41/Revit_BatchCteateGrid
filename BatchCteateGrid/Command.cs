using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BatchCteateGrid
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        const double distance = 6000 / 304.8;//轴线交点到轴号名字之间的距离为distance/2
        public double XtotalDistances;
        public double YtotalDistances;
        public List<Grid> XGrid;
       public  List<Grid> YGrid;
        public XYZ O;
        //ui应用程序
        public  UIApplication uiapp;
        //应用程序
        public Application app;
        //ui文档
        public UIDocument uidoc;
        //文档
        public Document doc;
        CreateForm createForm = null;
        public static  PreviewControl previewcontrol;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //ui应用程序
            uiapp = commandData.Application;
            //应用程序
            app = uiapp.Application;
            //ui文档
            uidoc = uiapp.ActiveUIDocument;
            //文档
            doc = uidoc.Document;
            if (createForm == null)
            {
                 createForm = new CreateForm(this);
                createForm.Show();
            }
            CreatePreviewForm();
            return Result.Succeeded;
        }
        internal void CreateGrids(List<GridInfo> xInfo, List<GridInfo> yInfo,UIDocument uidoc,Document doc,XYZ point)
        {
            XtotalDistances = 0;
            YtotalDistances = 0;
            foreach (var item in xInfo)
            {
                XtotalDistances += item.Distance * item.Count;
            }
            foreach (var item in yInfo)
            {
                YtotalDistances += item.Distance * item.Count;
            }
            using (Transaction trans = new Transaction(doc, "创建轴网"))
            {
                string X = "X";
                string Y = "Y";
                trans.Start();
                O = point;
                 XGrid = new List<Grid>();
                Grid gridx = CreateGrid(X, O,doc);
                XGrid.Add(gridx);
                gridx.Name = "1";
                double x = 0;
                foreach (var item in xInfo)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item.Distance != 0)
                        {
                            x += item.Distance;
                            Grid grid1 = CreateGrid(X, new XYZ(O.X + x, O.Y, 0),doc);
                            XGrid.Add(grid1);
                        }
                    }
                }
                YGrid = new List<Grid>();
                Grid gridy = CreateGrid(Y, O, doc);
                YGrid.Add(gridy);
                gridy.Name = "A";
                double y = 0;
                foreach (var item in yInfo)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (item.Distance != 0)
                        {
                            y += item.Distance;
                            Grid grid1 = CreateGrid(Y, new XYZ(O.X, O.Y + y, 0), doc);
                            YGrid.Add(grid1);
                        }
                    }
                }
                trans.Commit();
            }
        }
        public Grid CreateGrid(string Direction, XYZ xyz,Document doc)
        {// Create the geometry line which the grid locates
            XYZ start = null;
            XYZ end = null;
            if (Direction == "X")
            {
                start = new XYZ(xyz.X, xyz.Y - distance / 2, 0);
                end = new XYZ(xyz.X, xyz.Y + YtotalDistances+ distance / 2, 0);
            }
            else if (Direction == "Y")
            {
                start = new XYZ(xyz.X - distance / 2, xyz.Y, 0);
                end = new XYZ(xyz.X + XtotalDistances+ distance / 2, xyz.Y, 0);
            }
            Line geomLine = Line.CreateBound(start, end);
            // Create a grid using the geometry line
            Grid lineGrid = Grid.Create(doc, geomLine);
            if (null == lineGrid)
            {
                throw new Exception("Create a new straight grid failed.");
            }
            // Modify the name of the created grid
            GridType gridType = doc.GetElement(lineGrid.GetTypeId()) as GridType;
            Parameter gridParameter1 = gridType.get_Parameter(BuiltInParameter.GRID_CENTER_SEGMENT_STYLE);
            gridParameter1.Set(0);
            Parameter gridParameter2 = gridType.get_Parameter(BuiltInParameter.GRID_BUBBLE_END_1);
            gridParameter2.Set(1);
            Parameter gridParameter3 = gridType.get_Parameter(BuiltInParameter.GRID_BUBBLE_END_2);
            gridParameter3.Set(1);
            return lineGrid;
        }
        public XYZ getOriginPoint(UIDocument uidoc)
        {
            XYZ xyz = uidoc.Selection.PickPoint();
            return xyz;
        }
        /// <summary>
        /// 由predoc创建预览视图
        /// </summary>
       public  Document predoc = null;
        public void CreatePreviewForm()
        {
            DocumentSet docs = app.Documents;
            foreach (Document item in docs)
            {
                if (item.Title != doc.Title)
                {
                    predoc = item;
                }
            }
            View viewPlan = null;
            FilteredElementCollector elements1 = new FilteredElementCollector(predoc);
            List<View> views = elements1.OfCategory(BuiltInCategory.OST_Views).Cast<View>().ToList();
            foreach (var item in views)
            {
                Element element = predoc.GetElement(item.GetTypeId());
                if (item.Name == "标高 1" && element.Name == "楼层平面")
                {
                    viewPlan = item;
                }
            }
            try
            {
                using (Transaction trans = new Transaction(doc, "d"))
                {
                    trans.Start();
                    previewcontrol = new PreviewControl(predoc, viewPlan.Id);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Tip", ex.Message.ToString());
            }
        }
    }
}
