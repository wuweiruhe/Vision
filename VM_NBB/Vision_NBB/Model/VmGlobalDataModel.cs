using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{
    public class VmGlobalDataModel : VmGlobalData
    {



        #region camera1

        public float BasePointX { get; set; }
        public float BasePointY { get; set; }
        public float BaseAngle { get; set; }

        public int TrigCts { get; set; }

        public float XCalibInterval { get; set; }

        public float YCalibInterval { get; set; }

        public float RCalibInterval { get; set; }


        /// <summary>
        /// 垂直丝印与硅片边缘距离最小值
        /// </summary>
        public float VDistanceMin { get; set; }

        /// <summary>
        /// 垂直丝印与硅片边缘距离最大值
        /// </summary>
        public float VDistanceMax { get; set; }

        /// <summary>
        /// 水平丝印与硅片边缘距离最小值
        /// </summary>
        public float HDistanceMin { get; set; }

        /// <summary>
        /// 水平丝印与硅片边缘距离最大值
        /// </summary>
        public float HDistanceMax { get; set; }



        /// <summary>
        /// 机械手抓取偏移设置X
        /// </summary>

        public float TPointX { get; set; }
        public float TPointY { get; set; }
        public float TPointR { get; set; }



        //  机械手  基准位   判断取治具一次或二次
        public float RobotBaseX { get; set; }
        public float RobotPosMinX { get; set; }
        public float RobotPosMaxX { get; set; }


        public float RobotBaseY { get; set; }
        public float RobotPosMinY { get; set; }
        public float RobotPosMaxY { get; set; }



        public float RobotBaseR { get; set; }

        public float RobotPosMinR { get; set; }
        public float RobotPosMaxR { get; set; }




        #endregion


        #region camera2

        /// <summary>
        /// 治具是教点X
        /// </summary>
        public float FixturePointX { get; set; }


        public float FixtureMinPointX { get; set; }
        public float FixtureMaxPointX { get; set; }

        /// <summary>
        /// 治具是教点Y
        /// </summary>
        public float FixturePointY { get; set; }

        public float FixtureMinPointY { get; set; }
        public float FixtureMaxPointY { get; set; }

        /// <summary>
        /// 治具是教点R
        /// </summary>
        public float FixturePointR { get; set; }


        public float FixtureMinPointR { get; set; }
        public float FixtureMaxPointR { get; set; }



        #endregion



        #region camera3


        /// <summary>
        /// 多少个焊带
        /// </summary>
        public int WeldingCount { get; set; }


        /// <summary>
        /// 上点胶像素精度
        /// </summary>
        public float UpGluePixel { get; set; }
        public float UpGlueNgPosX { get; set; }
        public float UpGlueNgPosY { get; set; }

       /// <summary>
       /// 上点胶 焊带间距
       /// </summary>
        public float UpGapDistnce { get; set; }

        #endregion



        #region camera4

        
        public float DownGlueNgPosX { get; set; }
        public float DownGlueNgPosY { get; set; }

        /// <summary>
        /// 背点胶像素精度
        /// </summary>
        public float BackGluePixel { get; set; }


        /// <summary>
        /// 背点胶 焊带间距
        /// </summary>
        public float BackGapDistnce { get; set; }
        #endregion




        private static VmGlobalDataModel _VmGlobalDataModel = null;

        public VmGlobalDataModel()
        {

        }

        public static VmGlobalDataModel Instance()
        {

            if (_VmGlobalDataModel == null)
            {
                lock (locks)
                {
                    if (_VmGlobalDataModel == null)
                    {
                        _VmGlobalDataModel = new VmGlobalDataModel();
                    }
                }
            }
            return _VmGlobalDataModel;




        }

    }
}
