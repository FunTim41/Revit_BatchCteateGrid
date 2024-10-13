using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BatchCteateGrid.XmlSer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
namespace BatchCteateGrid
{
    public partial class CreateForm : System.Windows.Forms.Form
    {
        Command myCom;
        ExternalEvent externalevent;
        ExternalEvent externalevent2;
        ExternalEvent externalevent3;
        ExternalEvent externalevent4;
        public CreateForm(Command command)
        {
            myCom = command;
            gridPreForm = new GridPreForm(this);
            InitializeComponent();
            externalevent = ExternalEvent.Create(new ExternalCommand(this, myCom));
            externalevent2 = ExternalEvent.Create(new ExternalCommand2(myCom));
            externalevent3 = ExternalEvent.Create(new ExternalCommand3(myCom));
            externalevent4 = ExternalEvent.Create(new ExternalCommand4(this, myCom));
            this.LocationChanged += MainForm_LocationChanged;
            this.Load += button4_Click;
            this.dataGridView1.CellValueChanged += GridPreForm_ShowGrid;
            this.dataGridView2.CellValueChanged += GridPreForm_ShowGrid;
        }
        private void GridPreForm_ShowGrid(object sender, DataGridViewCellEventArgs e)
        {
                externalevent4.Raise();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            externalevent.Raise();
            if (checkBox1.Checked)
            {
                externalevent3.Raise();
            }
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            externalevent2.Raise();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            GridList listGrid = XmlUtil.DeserializeFromXml<GridList>(@"E:\★★★★REVIT 二开\自编\批量创建轴网\BatchCteateGrid\BatchCteateGrid\XMLSer\XMLFile1.xml");
            if (listGrid != null)
            {
                int i = 0;
                foreach (var item in listGrid.gridListX)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = item.Distance.ToString();
                    dataGridView1.Rows[i].Cells[1].Value = item.Count.ToString();
                    ++i;
                }
                int j = 0;
                foreach (var item in listGrid.gridListY)
                {
                    dataGridView2.Rows.Add();
                    dataGridView2.Rows[j].Cells[0].Value = item.Distance.ToString();
                    dataGridView2.Rows[j].Cells[1].Value = item.Count.ToString();
                    ++j;
                }
            }
        }
        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridPreForm.Close();
            List<GridInfo> Xinfos = new List<GridInfo>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                GridInfo info = new GridInfo();
                info.Distance = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value);
                info.Count = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                if (info.Count != 0)
                {
                    Xinfos.Add(info);
                }
            }
            List<GridInfo> Yinfos = new List<GridInfo>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                GridInfo info = new GridInfo();
                info.Distance = Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value);
                info.Count = Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value);
                if (info.Count != 0)
                {
                    Yinfos.Add(info);
                }
            }
            GridList gridList = new GridList();
            gridList.gridListX = Xinfos;
            gridList.gridListY = Yinfos;
            XmlUtil.SerializeToXml(@"E:\★★★★REVIT 二开\自编\批量创建轴网\BatchCteateGrid\BatchCteateGrid\XMLSer\XMLFile1.xml", gridList);
        }
        public static GridPreForm gridPreForm;
        private void button6_Click(object sender, EventArgs e)
        {
            gridPreForm.elementHost1.Child = Command.previewcontrol;
            button6.Text = "<<轴网预览";
            if (gridPreForm == null || gridPreForm.IsDisposed)
            {
                gridPreForm = new GridPreForm(this);
            }
            if (gridPreForm.Visible)
            {
                button6.Text = "轴网预览>>";
                gridPreForm.Hide(); // 隐藏子窗口
            }
            else
            {
                button6.Text = "<<轴网预览";
                gridPreForm.Show(); // 显示子窗口
                UpdateChildFormPosition();
            }
        }
        private void UpdateChildFormPosition()
        {
            if (gridPreForm != null && gridPreForm.Visible)
            {
                // 设置子窗口位置
                gridPreForm.Location = new System.Drawing.Point(this.Location.X + this.Width, this.Location.Y);
            }
        }
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            UpdateChildFormPosition();
        }
        public List<GridInfo> XInfo;
        public List<GridInfo> YInfo;
        public void getInfo()
        {
            XInfo = new List<GridInfo>();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                GridInfo x = new GridInfo();
                x.Distance = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value) / 304.8;
                x.Count = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                if (x.Count == 0)
                {
                    x.Count = 1;
                }
                XInfo.Add(x);
            }
            YInfo = new List<GridInfo>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                GridInfo y = new GridInfo();
                y.Distance = Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value) / 304.8;
                y.Count = Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value);
                if (y.Count == 0)
                {
                    y.Count = 1;
                }
                YInfo.Add(y);
            }
        }
    }
}
