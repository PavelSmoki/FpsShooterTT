using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace FpsShooter
{
    [UsedImplicitly]
    public class SaveSystem : IInitializable, IDisposable
    {
        public int WinCount { get; private set; }

        public int LoseCount { get; private set; }
        
        public void Initialize()
        {
            WinCount = PlayerPrefs.GetInt("WinCount", 0);
            LoseCount = PlayerPrefs.GetInt("LoseCount", 0);
        }

        public void CountWin() => WinCount++;

        public void CountLose() => LoseCount++;

        public void Dispose()
        {
            PlayerPrefs.SetInt("WinCount", WinCount);
            PlayerPrefs.SetInt("LoseCount", LoseCount);
        }
    }
}