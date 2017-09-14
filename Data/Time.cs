
using MoonSharp.Interpreter;

namespace d4lilah.Data
{
    public class Time
    {
        [MoonSharpHidden]
        Game1 _game;

        [MoonSharpHidden]
        public Time(Game1 game)
        {
            _game = game;
            //Update();
        }
        [MoonSharpHidden]
        public void Update()
        {
            if(_game.Input.IsReplay)
            {
                Year = _game.Input.ReplayFrame.Time.Year;
                Month = _game.Input.ReplayFrame.Time.Month;
                Day = _game.Input.ReplayFrame.Time.Day;
                Hour = _game.Input.ReplayFrame.Time.Hour;
                Minute = _game.Input.ReplayFrame.Time.Minute;
                Second = _game.Input.ReplayFrame.Time.Second;
                Millisecond = _game.Input.ReplayFrame.Time.Millisecond;
                DaylightSavingTime = _game.Input.ReplayFrame.Time.DaylightSavingTime;
                DayOfWeek = _game.Input.ReplayFrame.Time.DayOfWeek;
                MonthName = _game.Input.ReplayFrame.Time.MonthName;
                DayOfYear = _game.Input.ReplayFrame.Time.DayOfYear;
                WeekOfYear = _game.Input.ReplayFrame.Time.WeekOfYear; 
            }
            else
            {
                Year = System.DateTime.Now.Year;
                Month = System.DateTime.Now.Month;
                Day = System.DateTime.Now.Day;
                Hour = System.DateTime.Now.Hour;
                Minute = System.DateTime.Now.Minute;
                Second = System.DateTime.Now.Second;
                Millisecond = System.DateTime.Now.Millisecond;
           
                DaylightSavingTime = System.DateTime.Now.IsDaylightSavingTime();

                DayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
                MonthName = System.DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);

                DayOfYear = System.DateTime.Now.DayOfYear;
                WeekOfYear = (int)System.Math.Ceiling((decimal)System.DateTime.Now.DayOfYear / 7);   
            }
            if(_game.GameTime != null)
            {
                DeltaTime = ((float)_game.GameTime.ElapsedGameTime.TotalMilliseconds / 1000f);
                if(_game.Levels.CurrentLevel != null)
                {
                    DeltaTime *= _game.Levels.CurrentLevel.Timescale;
                }
            }
    }

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
