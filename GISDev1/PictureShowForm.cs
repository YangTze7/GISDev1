using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GISDev1
{
    public partial class PictureShowForm : Form
    {
        private string _pname;

        public string Pname
        {
            get
            {
                return _pname;
            }

            set
            {
                _pname = value;
            }
        }

        public PictureShowForm(string name)
        {
            Pname = name;
            InitializeComponent();
        }

        private void PictureShowForm_Load(object sender, EventArgs e)
        {
            string pPath = null;
            switch (this.Pname)
            {
                case "南门":
                    pPath = @"data\pic\南门.jpg";
                    break;
                case "名人园":
                    pPath = @"data\pic\名人园.jpg";
                    break;
                case "笔架山":
                    pPath = @"data\pic\笔架山.jpg";
                    break;
                case "花海":
                    pPath = @"data\pic\花海.jpg";
                    break;
                case "晨读园":
                    pPath = @"data\pic\晨读园.jpg";
                    break;
                case "泰山广场":
                    pPath = @"data\pic\泰山广场.jpg";
                    break;
            }

            if (this.Pname=="开封市")
                pPath = @"data\pic\B餐.jpg";    //获得文件的绝对路径
            //else
            //   pPath = @"data\pic\test.tif";
            if(pPath!=null)
            this.pictureBox1.Load(pPath);



        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
