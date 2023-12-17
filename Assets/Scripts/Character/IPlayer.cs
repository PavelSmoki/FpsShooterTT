using System;
using UniRx;
using UnityEngine;

namespace FpsShooter.Character
{
    public interface IPlayer
    {
        IReactiveProperty<int> CurrentHealth { get; }
        public Action<int> OnShoot { get; set; }
        Transform GetCurrentTransform();
        Transform GetLookAtTransform();
        Inventory GetInventory();
    }
}