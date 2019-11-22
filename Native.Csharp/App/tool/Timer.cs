using System;
using System.Collections;

namespace Native.Csharp.App.Tool
{
    class Timer
    {
        private static bool start = false;
        public static ArrayList timers; //定时器任务列表
        private static uint longcount = 60;
        public static void TimerRun()
        {
            if (start)
                return;
            start = true;

            LuaEnv.LuaApi.UpdateTimerTask(); //更新定时器任务列表

            System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Enabled = true;
            timer.Interval = 1000;//1s
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
            timer.Start();
        }

        public static void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)  //1s定时程序
        {
            // 得到 hour minute second  如果等于某个值就开始执行某个程序。  
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            longcount++;
            if (longcount >= 60)//每分钟执行脚本
            {
                longcount = 0;

                foreach (var time in timers)
                {
                    
                    string[] t = time.ToString().Split(':');
                    if (t[0] == intHour+"" & t[1]== intMinute+"" ){
                        LuaEnv.LuaEnv.RunLua("", "envent/TimerRun.lua",
                        new ArrayList() {
                        "time", time.ToString(),
                        });
                    }
                    //Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "时间", t[0] + t[1]);
                }
                //Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Error, "时间", e.ToString()+ intHour+ intMinute+ intSecond);
            }
        }
    }
}
