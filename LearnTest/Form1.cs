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
using System.Threading;

namespace LearnTest
{
    public partial class MainForm : Form
    {

        public static MainForm instance = null;
        public SynchronizationContext m_SyncContext = null;
        public MainForm()
        {
            InitializeComponent();
            instance = this;
            m_SyncContext = SynchronizationContext.Current;
        }

        public static MainForm GetInstance()
        {
            if (instance != null)
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
            ChangeUIClass myChangeUIClass = new ChangeUIClass();
            //myChangeUIClass.changeUIEventHandler += new ChangeUIDelegate(ChangeUI);

            //myChangeUIClass.ChangeUIFun(MainForm.GetInstance().label1, "改变");
            //     TaskRun2.eventHandlerChangeUI += new EventHandler(ChangeUIFun);
            TaskRun.Start2();

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

        private void button3_Click(object sender, EventArgs e)
        {
            TaskRun4 myTaskRun4 = new TaskRun4();
            myTaskRun4.ThreadFun();
        }

        public void ChangeBtnText(string Text)
        {
            Action DoAction = delegate ()
            {
                label1.Text = Text;
            };

            if (this.InvokeRequired)
            {
                ControlExtensions.UIThreadInvoke(this, delegate
                {
                    DoAction();
                });
            }
            else
            {
                DoAction();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 myForm2 = new Form2();
            myForm2.btnClickEvent += Change;
            myForm2.Show();
        }

        public void Change(string msg)
        {
            this.label1.Text = "事件改变！" + msg;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TaskRun5 myTask = new TaskRun5();
            myTask.ThreadFun();
        }
    }


    public static class ControlExtensions
    {
        /// <summary>
        /// 同步执行 注：外层Try Catch语句不能捕获Code委托中的错误
        /// </summary>
        static public void UIThreadInvoke(this Control control, Action Code)
        {
            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(Code);
                    return;
                }
                Code.Invoke();
            }
            catch
            {
                /*仅捕获、不处理！*/
            }
        }
    }

    public class TaskRun4
    {
        public void ThreadFun()
        {
            Task newTask = new Task(() => { MainForm.GetInstance().ChangeBtnText("改变啦"); });
            newTask.Start();
        }

        public void myFun()
        {
            MainForm.GetInstance().ChangeBtnText("改变啦");
        }
    }



    public class TaskRun5
    {
        public void ThreadFun()
        {
            Task newTask = new Task(() => myFun());
            newTask.Start();
        }

        public void myFun()
        {
            ChangeUIClass myChangeUIClass = new ChangeUIClass();
            myChangeUIClass.ChangeUI(MainForm.GetInstance().label1, "委托改变！");
            //myChangeUIClass.SpecificChange(MainForm.GetInstance().label1, "委托改变！");
        }
    }


    public class ChangeUIClass
    {
        public delegate void ChangeUIDelegate(Control uiControl, object obj);

        public void ChangeUI(Control myControl, object obj)
        {
            if (myControl.InvokeRequired)
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
            //ChangeUIClass myChangeUIClass = new ChangeUIClass();
            ////myChangeUIClass.changeUIEventHandler += new ChangeUIDelegate(myChangeUIClass.ChangeUI);

            //myChangeUIClass.ChangeUI(MainForm.GetInstance().label1, "改变");
            object a = "hh";
            object b = "hh";
            MainForm.GetInstance().m_SyncContext.Post(ChangeText, new object[] { a, b });
        }

        public static void ChangeText(object text)
        {
            object[] a = (object[])text;
            a[0].ToString();
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

    public class Subscriber_1 : Subscriber
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

    public class Subscriber_2 : Subscriber
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
