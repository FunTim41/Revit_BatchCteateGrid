using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BatchCteateGrid
{
    class ExternalCommand : IExternalEventHandler
    {
        CreateForm _createForm;
        Command _myCom;
        public ExternalCommand(CreateForm createForm, Command myCom)
        {
            this._createForm = createForm;
            this._myCom = myCom;
        }
        public void Execute(UIApplication app)
        {
            XYZ point= _myCom.getOriginPoint(_myCom.uidoc);
            try
            {
                _myCom.CreateGrids(_createForm.XInfo, _createForm.YInfo, _myCom.uidoc, _myCom.doc,point);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("tip", ex.Message);
            }
        }
        public string GetName()
        {
            return "创建轴网";
        }
    }
}
