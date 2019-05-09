using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearnTest
{
    public partial class Form2 : Form
    {
        public delegate void btnClick(string msg);
        public event btnClick btnClickEvent = null;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EventArgsUser myUSer = new EventArgsUser();
            myUSer.User = "刘";
            btnClickEvent?.Invoke("消息");
        }
    }

    public class EventArgsUser:EventArgs
    {
        public string User { get; set; }
    }
}
