using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            SubscribeForEvents();
        }

        private void SubscribeForEvents()
        {
            buttonTest.Click += ButtonTest_Click;
        }

        private void ButtonTest_Click(object sender, EventArgs e)
        {
            
        }
    }
}
