using Achievement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAchievement : ScriptableObject, IAchieve
{
    [Header("Content")]
    [TextArea]
    public string information;


    public void Achieve()
    {
        throw new System.NotImplementedException();
    }

    public void Register()
    {
        throw new System.NotImplementedException();
    }
}
