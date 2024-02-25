using GlobalVariableModuleCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vision_NBB.Utility;
using VM.Core;

namespace Vision_NBB.Model
{
    /// <summary>
    ///  继承 子类的 属性名要与  vm全局 变量名 一致
    /// </summary>
    /// 

    public class VmGlobalData
    {
        protected static object locks = new object();


        /// <summary>
        /// 从类 写入vm全局变量里面
        /// </summary>
        public virtual void SetGlobalVars()
        {
            try
            {
                var globalVarTool = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];

                var items = this.GetType().GetProperties();

                foreach (var item in items)
                {
                    var name = item.Name;

                    string value = item.GetValue(this).ToString();

                    globalVarTool.SetGlobalVar(name, value); // 设置全局变量

                }
            }
            catch (Exception ex)
            {

                throw new Exception("获得Vm全局变量出错");


            }



        }


        /// <summary>
        ///  从vm全局变量里面 写入到 类中
        /// </summary>
        //public virtual void GetGloabalVars<T>(object obj)
        public virtual void GetGloabalVars<T>()
        {
            try
            {
                SetProper<T>(this);
            }
            catch(Exception ex)
            {

               
            }
            
        }

        public virtual void SetSingleGlobalVars(string name, string value)
        {
            try
            {
                GlobalVariableModuleTool globalVar = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];
                globalVar.SetGlobalVar(name, value); // 设置全局变量

            }

            catch (Exception ex)
            {

                throw new Exception("获得Vm全局变量出错");


            }

        }
        private void SetProper<T>(object obj)
        {
            string ErrorName = "";
            try
            {
                var globalVarTool = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];

                var items = this.GetType().GetProperties();

              

                foreach (var item in items)
                {

                    ErrorName=item.Name;
                    var name = item.Name;
                    Type myType = typeof(T);
                    PropertyInfo Info = myType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var Instance = (T)obj;

                    var globalval = globalVarTool.GetGlobalVar(name);
                    object val = Convert.ChangeType(globalval, item.PropertyType);
                    //object val = Convert.ChangeType("1234", item.PropertyType);

                    Info.SetValue(Instance, val);
                }
            }
            catch (Exception ex)
            {

               GetLogHelper.VisionLog.Error("获得Vm 全局变量出错:  " + ErrorName);
           
           
                throw  new Exception("获得Vm全局变量出错");


            }



        }
    }
}