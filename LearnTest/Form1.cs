using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HIX;

namespace LearnTest
{
    public partial class MainForm : Form
    {

        public static MainForm instance = null;

        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }

        public static MainForm GetInstance()
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Publisher myPublisher = new Publisher();
            Subscriber mySubscriber_1 = new Subscriber_1(myPublisher);
            Subscriber mySubscriber_2 = new Subscriber_2(myPublisher);

            myPublisher.SendMsgSpecific("News Appear");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ChangeUIClass myChangeUIClass = new ChangeUIClass();
            //myChangeUIClass.changeUIEventHandler += new ChangeUIDelegate(ChangeUI);

            //myChangeUIClass.ChangeUIFun(MainForm.GetInstance().label1, "改变");
            TaskRun2.eventHandlerChangeUI += new EventHandler(ChangeUIFun);
            TaskRun2.Start2();
            
        }

        public void ChangeUIFun(object sender, EventArgs e)
        {
            Hanlder myHandler = e as Hanlder;
            label1.Text = myHandler.myString;
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            string path = "D:FAI.csv";
            CsvHelper.StringToCsv(this.textBox1.Text, path);
            
        }
    }






    public delegate void ChangeUIDelegate(Control uiControl, object obj);
    public class ChangeUIClass
    {
        public event ChangeUIDelegate changeUIEventHandler;

        public void ChangeUIFun(Control uiControl, object obj)
        {
            changeUIEventHandler?.Invoke(uiControl, obj);
        }

        public void ChangeUI(Control myControl, object obj)
        {
            if(myControl.InvokeRequired)
            {
                ChangeUIDelegate myChangeUIDelegate = new ChangeUIDelegate(ChangeUI);
                myControl.Invoke(myChangeUIDelegate, new object[] { myControl, obj });
            }
           else
            {
                SpecificChange(myControl, obj);
            }
        }

        public void SpecificChange(Control myControl, object obj)
        {
            myControl.Text = obj.ToString();
        }
    }


    public class TaskRun
    {
        
        public static void Start2()
        {
            Task myRun = new Task(() => Start());
            myRun.Start();
        }

        public static void Start()
        {
            ChangeUIClass myChangeUIClass = new ChangeUIClass();
            //myChangeUIClass.changeUIEventHandler += new ChangeUIDelegate(myChangeUIClass.ChangeUI);

            myChangeUIClass.ChangeUI(MainForm.GetInstance().label1, "改变");
        }
    }

    public class TaskRun2
    {
        public delegate void ChangeUI(Control myControl, object obj);
        public static EventHandler eventHandlerChangeUI;

        public static void Start2()
        {
            Task myRun = new Task(() => Start());
            myRun.Start();
        }

        public static void Start()
        {
            if (eventHandlerChangeUI != null)
            {
                Hanlder myHandler = new Hanlder();
                myHandler.myControl = MainForm.GetInstance().label1;
                myHandler.myString = "事件改变";
                eventHandlerChangeUI(null, myHandler);
            }
        }
    }

    public class Hanlder : EventArgs
    {
        public Control myControl { get; set; }
        public string myString { get; set; }
    }

    public delegate void SendMsg(string msg);
    public class Publisher
    {
        public event SendMsg eventHandle;

        public void SendMsgSpecific(string msg)
        {
            MessageBox.Show("发布消息：" + msg);
            eventHandle?.Invoke(msg);
        }
    }


    interface Subscriber
    {
        void AcceptMsg(string msg);
    }

    public class Subscriber_1: Subscriber
    {
        public Subscriber_1(Publisher myPublisher)
        {
            myPublisher.eventHandle += new SendMsg(AcceptMsg);
            myPublisher.eventHandle += new SendMsg(AcceptMsg2);
        }

        public void AcceptMsg(string msg)
        {
            MessageBox.Show("订阅者1接收消息1：" + msg);
        }

        public void AcceptMsg2(string msg)
        {
            MessageBox.Show("订阅者1接收消息2：" + msg);
        }
    }

    public class Subscriber_2: Subscriber
    {
        public Subscriber_2(Publisher myPublisher)
        {
            myPublisher.eventHandle += new SendMsg(AcceptMsg);
        }

        public void AcceptMsg(string msg)
        {
            MessageBox.Show("订阅者2接收消息：" + msg);
        }
    }
}
