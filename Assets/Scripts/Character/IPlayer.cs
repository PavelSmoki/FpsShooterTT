using UniRx;
using UnityEngine;

namespace FpsShooter.Character
{
    public interface IPlayer
    {
        IReactiveProperty<int> CurrentHealth { get; }
        Transform GetCurrentTransform();
        Transform GetLookAtTransform();
    }
}