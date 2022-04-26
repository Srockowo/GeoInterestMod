using System;
using UnityEngine;
using System.Threading.Tasks;

namespace GeoInterestMod
{
    public enum TaskState { Stopped, Running, Paused }

    public class InterestTask : MonoBehaviour
    {
        public static double InterestRate = 1.05;
        public static TimeSpan InterestInterval = TimeSpan.FromMinutes(1);

        public static bool ShouldDisplayTimer = true;

        private static TaskState currentState = TaskState.Stopped;

        private static double nextActionTime = 0;
        private static double timeElapsedRunning = 0;

        public static TaskState GetState => currentState;

        public void Update()
        {
            if (currentState == TaskState.Stopped) return;

            timeElapsedRunning += Time.deltaTime;

            if (currentState == TaskState.Paused) return;

            if (ShouldDisplayTimer) InterestTimer.UpdateText(InterestInterval.TotalSeconds - timeElapsedRunning);

            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + InterestInterval.TotalSeconds;

                int currentGeo = PlayerData.instance.geo;
                int geoToAdd = Mathf.CeilToInt((float) (currentGeo * InterestRate - currentGeo));

                HeroController.instance.AddGeo(geoToAdd);

                timeElapsedRunning = 0;
            }
        }

        public static void StartTask()
        {
            if (currentState == TaskState.Running) return;

            nextActionTime = Time.time + InterestInterval.TotalSeconds;

            if (ShouldDisplayTimer) InterestTimer.SetActive(true);

            currentState = TaskState.Running;
        }

        public static async void StartTaskWithDelay(TimeSpan delay)
        {
            await Task.Delay(delay);

            StartTask();
        }

        public static void StopTask()
        {
            if (currentState == TaskState.Stopped) return;

            InterestTimer.SetActive(false);

            currentState = TaskState.Stopped;
        }

        public static void PauseTask()
        {
            if (currentState == TaskState.Paused) return;

            currentState = TaskState.Paused;
        }

        public static void ResumeTask()
        {
            if (currentState != TaskState.Paused) return;

            nextActionTime = Time.time + (InterestInterval.TotalSeconds - timeElapsedRunning);

            currentState = TaskState.Running;
        }

        /// <summary>
        /// Method <c>Edit</c> will reset the task, starting it again with the new given values
        /// for InterestRate and InterestInterval respectively.
        /// </summary>
        public static void Edit(double newRate, TimeSpan newTimeSpan)
        {
            StopTask();

            timeElapsedRunning = 0;

            InterestInterval = newTimeSpan;
            InterestRate = newRate;

            if (HeroController.instance == null) return;

            StartTask();
        }

        public static void Edit(double newRate) => Edit(newRate, InterestInterval);
        public static void Edit(TimeSpan newTimeSpan) => Edit(InterestRate, newTimeSpan);
    }
}