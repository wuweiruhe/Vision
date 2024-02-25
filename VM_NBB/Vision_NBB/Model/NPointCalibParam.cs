using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model
{
    /// <summary>
    /// 机械手标定
    /// </summary>
    public class NPointCalibParam
    {
        //
        // 摘要:
        //     CH: 标定原点 | EN: Calibration Origin
        public int CalibOrigin { get; set; }


        //
        // 摘要:
        //     CH: 基准点X | EN: Fiducial Point X
        public float BasePointX { get; set; }
        //
        // 摘要:
        //     CH: 基准点Y | EN: Fiducial Point Y
        public float BasePointY { get; set; }

        //
        // 摘要:
        //     CH: 基准角度 | EN: Fixtured Angle
        public float BaseAngle { get; set; }


        //
        // 摘要:
        //     CH: 触发次数 | EN: Trig counts
        public int TrigCts { get; set; }

        //
        // 摘要:
        //     CH: 角度偏移 | EN: Angle Offset
        public float MoveAngle { get; set; }
        //
        // 摘要:
        //     CH: 偏移Y | EN: Offset Y
        public float MoveAlignY { get; set; }
        //
        // 摘要:
        //     CH: 偏移X | EN: Offset X
        public float MoveAlignX { get; set; }


        //
        // 摘要:
        //     CH: 标定文件路径 | EN: Set Calibration File Path
        public string CalibPathName { get; set; }




        // 摘要:
        //     CH: 旋转次数 | EN: Rotation Number
        public int RotPointTotalNum { get; set; }
        //
        // 摘要:
        //     CH: 平移次数 | EN: Translation Number
        public int CalibPointTotalNum { get; set; }


        public NPointCalibParam()
        {
            BasePointX = 1;
            BasePointY = 1;
            BaseAngle = 10;

            MoveAngle = 1;
            MoveAlignY = 1;
            MoveAlignX = 1;

            RotPointTotalNum = 3;

            CalibPointTotalNum = 9;


        }


    }
}
