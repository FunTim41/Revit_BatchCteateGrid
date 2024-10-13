using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchCteateGrid
{
    class Eapp : IExternalApplication
    {/// <summary>
     /// 预览项目的doc
     /// </summary>
        Document predoc;

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Register event. 
                application.ControlledApplication.DocumentOpened += new EventHandler
                     <Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(application_DocumentOpened);
            }
            catch (Exception)
            {
                return Result.Failed;
            }
            return Result.Succeeded;
        }
        /// <summary>
        /// 当revit打开一个项目文件之后，新建一个空项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void application_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            Document doc = e.Document;
            Application app = doc.Application;

            using (Transaction transaction = new Transaction(doc, "Edit Address"))
            {
                if (transaction.Start() == TransactionStatus.Started)
                {
                    predoc = app.NewProjectDocument(0);
                    transaction.Commit();
                }
            }
        }
    }
}
