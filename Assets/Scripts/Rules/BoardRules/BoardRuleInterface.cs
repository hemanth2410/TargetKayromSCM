using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IBoardRule
{
    public abstract bool ExecuteRule(Vector3 strikerPosition, Vector3 strikerForceDirection, CoinType faction, LayerMask layermask);
}
