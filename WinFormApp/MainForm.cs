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
            // 查询 Result 属性阻止 GUI 线程返回
            // 线程在等待结果期间阻塞
            string res = GetHttp().Result;
            MessageBox.Show(res);
        }

        /// <summary>
        /// 不再是异步函数了
        /// 从方法签名中删除了 async 关键字
        /// 所以不再是等待 Task，而非返回了一个 Task
        /// </summary>
        /// <returns></returns>
        private Task<String> GetHttp()
        {
            return Task.Run(async () =>
            {
                // 运行一个无 SynchronizationContext 的线程池线程
                await Task.Delay(2000);
                return "Hello World";
            });
        }
    }
}
