using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchCteateGrid
{
    public partial class GridPreForm : Form
    {
        CreateForm createForm;
        public GridPreForm(CreateForm createForm)
        {
            InitializeComponent();
           this. createForm = createForm;
        }

        private void GridPreForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            createForm.button6.Text = "轴网预览>>";

        }
    }
}
