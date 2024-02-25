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
    public class CalibrationModel
    {
        /// <summary>
        /// 机械手标定X
        /// </summary>
        private float _robotX;

        public float RobotX
        {
            get { return _robotX; }
            set { _robotX = value; }
        }


        /// <summary>
        /// 机械手标定Y
        /// </summary>
        private float _robotY;

        public float RobotY
        {
            get { return _robotY; }
            set { _robotY = value; }
        }

        /// <summary>
        /// 机械手标定R
        /// </summary>
        private float _robotR;

        public float RobotR
        {
            get { return _robotR; }
            set { _robotR = value; }
        }

        /// <summary>
        /// 像素X
        /// </summary>
        private float _pixelX;

        public float PixelX
        {
            get { return _pixelX; }
            set { _pixelX = value; }
        }


        /// <summary>
        /// 像素Y
        /// </summary>
        private float _pixelY;

        public float PixelY
        {
            get { return _pixelY; }
            set { _pixelY = value; }
        }

        /// <summary>
        /// 像素R
        /// </summary>
        private float __pixelR;

        public float PixelR
        {
            get { return __pixelR; }
            set { __pixelR = value; }
        }

        /// <summary>
        /// 抓取状态OK/NG
        /// </summary>
        private string _stastus;

        public string Status
        {
            get { return _stastus; }
            set { _stastus = value; }
        }

    }



    /// <summary>
    /// 机械手工装标定
    /// </summary>
    public class RobotToolingCalibration
    {
        /// <summary>
        /// 工装孔矩形中心X
        /// </summary>
        private float _circleX;

        public float CircleX
        {
            get { return _circleX; }
            set { _circleX = value; }
        }


        /// <summary>
        /// 工装孔矩形中心Y
        /// </summary>
        private float _circleY;

        public float CircleY
        {
            get { return _circleY; }
            set { _circleY = value; }
        }

        /// <summary>
        /// 工装孔矩形中心R
        /// </summary>
        private float _circleR;

        public float CircleR
        {
            get { return _circleR; }
            set { _circleR = value; }
        }

        /// <summary>
        /// 相机视野角度X
        /// </summary>
        private float _cameraVisionX;

        public float CameraVisionX
        {
            get { return _cameraVisionX; }
            set { _cameraVisionX = value; }
        }

        /// <summary>
        /// 相机视野角度Y
        /// </summary>
        private float _cameraVisionY;

        public float CameraVisionY
        {
            get { return _cameraVisionY; }
            set { _cameraVisionY = value; }
        }

        /// <summary>
        /// 相机视野角度R
        /// </summary>
        private float _cameraVisionR;

        public float CameraVisionR
        {
            get { return _cameraVisionR; }
            set { _cameraVisionR = value; }
        }
    }


}
