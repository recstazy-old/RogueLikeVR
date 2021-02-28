using RoguelikeVR.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public interface IWeaponReciever
    {
        void SetWeapon(Weapon weapon);
    }
}
