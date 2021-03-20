using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeVR.AI
{
    public interface ICharacterDependent
    {
        CharacterDependencies Dependencies { get; set; }
        int InitOrder { get; }
        void Initialized();
    }
}
