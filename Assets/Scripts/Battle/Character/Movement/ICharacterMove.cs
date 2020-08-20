using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.Character
{
    public interface ICharacterMove
    {
        void Move(CharacterAction characterAction);
    }
}

