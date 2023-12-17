using System;
using UnityEngine;

namespace FpsShooter.Guns
{
    [CreateAssetMenu(menuName = "Tools/Create Gun", fileName = "Gun")]
    public class Gun : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float ShootingSpeed { get; private set; }
        [field: SerializeField] public int MaxAmmo { get; private set; }
        [field: SerializeField] public Sprite GunSprite { get; private set; }
        
        public int AmmoLeft { get; set; }

        public void ResetAmmoLeft()
        {
            AmmoLeft = MaxAmmo;
        }
    }
}
