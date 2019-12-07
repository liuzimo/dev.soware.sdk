using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Native.Csharp.App.LuaEnv
{
    class Cap : WebSocketBehavior
    {
        private static WebSocketServer wssv;
        protected override void OnOpen()
        {
            Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "websocket server", "建立连接");
            Send(Encoding.Default.GetBytes("cap"));
        }
        protected override void OnMessage(MessageEventArgs e)
        {
            string dpath = Common.AppDirectory;
            dpath = dpath.Substring(0, dpath.LastIndexOf("\\"));
            dpath = dpath.Substring(0, dpath.LastIndexOf("\\"));
            dpath = dpath.Substring(0, dpath.LastIndexOf("\\") + 1);
            dpath = dpath + ID + "\\";
            if (!Directory.Exists(dpath))
            {
                Directory.CreateDirectory(dpath);
            }
            byte[] bt = Convert.FromBase64String(Encoding.Default.GetString(e.RawData));
            File.WriteAllBytes(dpath + Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString() + ".jpg", bt);
            Send(Encoding.Default.GetBytes("cap"));
        }
        public static void WebSocketStart()
        {
            wssv = new WebSocketServer(2000,true); 
            wssv.SslConfiguration.ServerCertificate = new X509Certificate2("C:\\Users\\Administrator\\Desktop\\nginx-1.16.1\\html\\rps\\3168162_0nly.cn.pfx", "123456");
            wssv.AddWebSocketService<Cap>("/cap");
            wssv.Start();
            Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "websocket server", "服务开启");
        }
        public static void WebSocketStop()
        {
            wssv.Stop();
            Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "websocket server", "服务关闭");
        }

        public static void Run()
        {
            try { 
            // Load the cascades
            var haarCascade = new CascadeClassifier("F:/desk/haarcascade_frontalface_alt.xml");
            //var lbpCascade = new CascadeClassifier("Data/Text/lbpcascade_frontalface.xml");

            // Detect faces
            Mat haarResult = DetectFace(haarCascade);
            //Mat lbpResult = DetectFace(lbpCascade);

            Cv2.ImShow("Faces by Haar", haarResult);
            //Cv2.ImShow("Faces by LBP", lbpResult);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
            haarCascade.Dispose();
            //lbpCascade.Dispose();
            }
            catch(Exception e)
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "opencv", e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cascade"></param>
        /// <returns></returns>
        private static Mat DetectFace(CascadeClassifier cascade)
        {
            Mat result;

            using (var src = new Mat("F:/desk/报名照片.jpg", ImreadModes.Color))
            using (var gray = new Mat())
            {
                result = src.Clone();
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                // Detect faces
                Rect[] faces = cascade.DetectMultiScale(
                    gray, 1.08, 2, HaarDetectionType.ScaleImage, new Size(30, 30));

                // Render all detected faces
                foreach (Rect face in faces)
                {
                    var center = new Point
                    {
                        X = (int)(face.X + face.Width * 0.5),
                        Y = (int)(face.Y + face.Height * 0.5)
                    };
                    var axes = new Size
                    {
                        Width = (int)(face.Width * 0.5),
                        Height = (int)(face.Height * 0.5)
                    };
                    Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
                }
            }
            return result;
        }
    }
}