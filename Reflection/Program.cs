using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Text myTest = new Text();
            Type type = myTest.GetType();

            Console.WriteLine(type.Name);
            Console.WriteLine(type.Namespace);
            Console.WriteLine(type.Assembly);
            Log.WriteLog("开111始");



            Log.WriteLog("出错", new Exception("这里有异常"));
            Log.WriteLog("出错", new Exception("这里有异常"));
            Log.WriteLog("出错", new Exception("这里有异常"));
            Log.WriteLog("出错", new Exception("这里有异常"));

            Console.WriteLine("********获取公有字段********");
            FieldInfo[] fieldInfo = type.GetFields();
            foreach (var item in fieldInfo)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("********获取公有属性********");
            PropertyInfo []propertyinfo = type.GetProperties();

            foreach (var item in propertyinfo)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine("********获取公有方法********");
            MethodInfo[] menthinfo = type.GetMethods();

            foreach (var item in menthinfo)
            {
                Console.WriteLine(item.Name);
            }


            Console.ReadKey();
        }
    }


    class Text
    {
        private int id;
        public int age;
        public int num;
        public string Name { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public void Test1() { }
        public void Text2() { }
    }

}
