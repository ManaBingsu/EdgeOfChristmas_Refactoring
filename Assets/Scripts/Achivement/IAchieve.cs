using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Achievement
{
    public interface IAchieve
    {
        void Register();
        void Achieve();
    }
}
