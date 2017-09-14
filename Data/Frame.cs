using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d4lilah.Data
{
    public class Frame
    {
        public ulong Index;
        public string[] KeyStrokes;
        public int MouseX = 0;
        public int MouseY = 0;
        public float Scale = 1.0f;
        public FrameTime Time;
    }
    public class FrameMaster
    {
        public Frame[] Frame;
        public string Name;
        public string Date;
        public float Interval;
        public int Seed;
        public string[] Checksums;
    }
    public class FrameTime
    {
        public int Year = 0;
        public int Month = 0;
        public int Day = 0;
        public int Hour = 0;
        public int Minute = 0;
        public int Second = 0;
        public int Millisecond = 0;
        public bool DaylightSavingTime = false;

        public string DayOfWeek = "";
        public string MonthName = "";

        public int DayOfYear = 0;
        public int WeekOfYear = 0;

        public float DeltaTime = 0f;
    }
}
