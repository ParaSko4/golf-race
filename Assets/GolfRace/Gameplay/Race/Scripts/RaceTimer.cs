using System;
using UniRx;
using UnityEngine;

namespace GolfRace.Gameplay.Race
{
    public class RaceTimer : MonoBehaviour
    {
        public event Action<float> TimeChange;
        public event Action Timeout;

        [SerializeField] private float raceTime;
        [SerializeField] private float minTimeWhenShowMilliseconds;

        private IDisposable timer;
        private float currentTime;
        private float stoppedTime;

        public float CurrentTime { get => currentTime; }

        public void StartTimer()
        {
            ResetTimer();

            timer = Observable.Interval(TimeSpan.FromMilliseconds(100)).Subscribe(time =>
            {
                float correctTime = raceTime + stoppedTime - (float)time / 10;

                ChangeTime(minTimeWhenShowMilliseconds >= correctTime ? correctTime : Mathf.Floor(correctTime));

                if (correctTime <= 0f)
                {
                    Timeout?.Invoke();
                    StopTimer();
                }
            });
        }

        public void ResetTimer()
        {
            stoppedTime = 0f;

            StopTimer();
            ChangeTime(0f);
        }

        public void StopTimer()
        {
            stoppedTime = raceTime - currentTime;
            timer?.Dispose();
        }

        private void ChangeTime(float time)
        {
            currentTime = time;

            TimeChange?.Invoke(currentTime);
        }

        private void OnDestroy()
        {
            StopTimer();
        }
    }
}
