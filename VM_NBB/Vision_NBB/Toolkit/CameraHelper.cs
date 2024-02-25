using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vision_NBB.Model;
using VM.Core;

namespace Vision_NBB.Toolkit
{
    public static class CameraHelper
    {

        
        public static void ShowNgIcon(VmModuleCollection<VmModule> vmProcedure, string cameraSeqence, Dictionary<string, ModelBlock> ModuleInfoDict)
        {  

            Thread.Sleep(20);

           
            foreach (var item in vmProcedure)
            {
                if (cameraSeqence.Contains(item.ID.ToString()))
                {
                    if ((item.IsRunOK != null))
                    {
                        if ((bool)item.IsRunOK) //true   
                        {
                            if (ModuleInfoDict[item.ID.ToString()].isChange == true)
                                ModuleInfoDict[item.ID.ToString()].NgIcons = "";
                            ModuleInfoDict[item.ID.ToString()].isChange = false;
                        }
                        else
                        {
                            ModuleInfoDict[item.ID.ToString()].NgIcons = "/image/fail.png";
                            ModuleInfoDict[item.ID.ToString()].isChange = true;
                        }
                    }
                }
            }
        }



        public static void ShowNgIcon22(VmModuleCollection<VmModule> vmProcedure, string cameraSeqence, Dictionary<string, ModelBlock> ModuleInfoDict)
        {

            Thread.Sleep(20);


            string[] numberStrings = cameraSeqence.Trim('{', '}').Split(',');

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            for (int i = 0; i < numberStrings.Length; i++)
            {
                dictionary[numberStrings[i]] = numberStrings[i];
            }






            foreach (var item in vmProcedure)
            {     
                var objID= item.ID.ToString();

                if (cameraSeqence.Contains(objID))
                {
                    if ((item.IsRunOK != null))
                    {
                        if ((bool)item.IsRunOK) //true   
                        {
                            if (ModuleInfoDict[objID].isChange == true)
                                ModuleInfoDict[objID].NgIcons = "";
                            ModuleInfoDict[objID].isChange = false;
                        }
                        else
                        {
                            ModuleInfoDict[objID].NgIcons = "/image/fail.png";
                            ModuleInfoDict[objID].isChange = true;
                        }
                    }
                }
            }
        }






        public static void ShowNgIcon2(VmModuleCollection<VmModule> vmProcedure, Dictionary<string,string> cameraSeqence, Dictionary<string, ModelBlock> ModuleInfoDict)
        {

            Thread.Sleep(20);


            foreach (var item in vmProcedure)
            {
                var objID = item.ID.ToString();

                if(cameraSeqence.ContainsKey(objID))        
                {
                    if ((item.IsRunOK != null))
                    {
                        if ((bool)item.IsRunOK) //true   
                        {
                            if (ModuleInfoDict[objID].isChange == true)
                                ModuleInfoDict[objID].NgIcons = "";
                            ModuleInfoDict[objID].isChange = false;
                        }
                        else
                        {
                            ModuleInfoDict[objID].NgIcons = "/image/fail.png";
                            ModuleInfoDict[objID].isChange = true;
                        }
                    }
                }
            }
        }


        public static void ShowNgIconByGroup(IEnumerable<ModuInfo> ModuInfos , Dictionary<string, string> cameraSeqence, Dictionary<string, ModelBlock> ModuleInfoDict)
        {

            Thread.Sleep(20);


            //foreach (var item in ModuInfos)
            //{
            //    var objID = item.ID.ToString();

            //    if (cameraSeqence.ContainsKey(objID))
            //    {
            //        if ((item.IsRunOK != null))
            //        {
            //            if ((bool)item.IsRunOK) //true   
            //            {
            //                if (ModuleInfoDict[objID].isChange == true)
            //                    ModuleInfoDict[objID].NgIcons = "";
            //                ModuleInfoDict[objID].isChange = false;
            //            }
            //            else
            //            {
            //                ModuleInfoDict[objID].NgIcons = "/image/fail.png";
            //                ModuleInfoDict[objID].isChange = true;
            //            }
            //        }
            //    }
            //}
        }

    }
}
