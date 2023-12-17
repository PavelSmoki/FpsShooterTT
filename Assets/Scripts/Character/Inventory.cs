using System;
using System.Collections.Generic;
using FpsShooter.Guns;
using UnityEngine;

namespace FpsShooter.Character
{
    [Serializable]
    public class Inventory 
    {
        [field: SerializeField] public List<Gun> Guns { get; private set; }
    }
}