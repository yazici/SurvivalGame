using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Utilities;
using Assets.Scripts.GameObjects.GameModel;
using System;
using UnityEngine.UI;

namespace Pamux.Lib.Managers
{
    public class TimeManager : Singleton<TimeManager>
    {
        public Text TimeField;

        public long TotalHours => SimulationActualRunningTimeInGameHours;
        public long TotalDays => TotalHours % (long) LengthOfGameDayInGameHours;
        public long DayOfYear => TotalDays % (long) LengthOfGameYearInGameDays;
        public long Now => (long) (TotalHours % LengthOfGameDayInGameHours);

        public Seasons Season => DayOfYearToSeasonMap[DayOfYear];
        public TimeOfDay TimeOfDay => NowToTimeOfDayMap[Now];

        private DateTime SimulationStartTime = DateTime.UtcNow;
        private TimeSpan SimulationTotalPauseAmount = TimeSpan.Zero;
        private bool SimulationIsRunning = false;
        private DateTime SimulationPauseStartTime;

        private TimeSpan SimulationRunningTime => DateTime.UtcNow - SimulationStartTime;
        private TimeSpan SimulationActualRunningTime => SimulationRunningTime - SimulationTotalPauseAmount;

        private long SimulationActualRunningTimeInGameHours => (long)(SimulationActualRunningTime.TotalMinutes / LengthOfGameHourInRealWorldMinutes);

        public const float LengthOfGameDayInGameHours = 24f;
        public const float LengthOfGameYearInGameDays = 100f;
        public const float LengthOfGameDayInRealWorldMinutes = 15f;
        public const float LengthOfGameHourInRealWorldMinutes = LengthOfGameDayInRealWorldMinutes / LengthOfGameDayInGameHours;

        private Seasons[] DayOfYearToSeasonMap = new Seasons[(long)LengthOfGameYearInGameDays];
        private TimeOfDay[] NowToTimeOfDayMap = new TimeOfDay[(long)LengthOfGameDayInGameHours];

        public int FirstDayOfSpring = 1;
        public int FirstDayOfSummer = 30;
        public int FirstDayOfFall = 50;
        public int FirstDayOfWinter = 80;

        public int DawnStartTime = 6;
        public int MorningStartTime = 8;
        public int NoonStartTime = 13;
        public int DuskStartTime = 19;
        public int NightStartTime = 21;

        public void Start()
        {
            for (int i = 0; i < LengthOfGameYearInGameDays; ++i)
            {
                if (i < FirstDayOfSummer)
                {
                    DayOfYearToSeasonMap[i] = Seasons.Spring;
                }
                else if (i < FirstDayOfFall)
                {
                    DayOfYearToSeasonMap[i] = Seasons.Summer;
                }
                else if (i < FirstDayOfWinter)
                {
                    DayOfYearToSeasonMap[i] = Seasons.Fall;
                }
                else
                {
                    DayOfYearToSeasonMap[i] = Seasons.Winter;
                }
            }

            for (int i = 0; i < LengthOfGameDayInGameHours; ++i)
            {
                if (i < DawnStartTime)
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Night;
                }
                else if (i < MorningStartTime)
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Dawn;
                }
                else if (i < NoonStartTime)
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Morning;
                }
                else if (i < DuskStartTime)
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Noon;
                }
                else if (i < NightStartTime)
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Dusk;
                }
                else
                {
                    NowToTimeOfDayMap[i] = TimeOfDay.Night;
                }
            }
        
            SimulationStartTime = DateTime.UtcNow;
            SimulationTotalPauseAmount = TimeSpan.Zero;
            SimulationIsRunning = true;
        }

        public void Pause()
        {
            SimulationPauseStartTime = DateTime.UtcNow;
            SimulationIsRunning = false;
        }

        public void Resume()
        {
            SimulationTotalPauseAmount += (DateTime.UtcNow - SimulationPauseStartTime);
            SimulationIsRunning = true;
        }

        void Update()
        {
            if (TimeField != null)
            {
                //TimeField.text = (LengthOfGameDayInRealWorldMinutes / LengthOfGameDayInGameHours).ToString();
                //TimeField.text = SimulationActualRunningTime.ToString();
                TimeField.text = SimulationIsRunning ? Now.ToString() : "PAUSED";
            }
        }
    }
}