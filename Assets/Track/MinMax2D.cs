using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Utility
{
    public class MinMax2D
    {

        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }

        public MinMax2D()
        {
            Min = Vector2.one * float.MaxValue;
            Max = Vector2.one * float.MinValue;
        }

        public void AddValue(Vector2 v)
        {
            Min = new Vector2(Mathf.Min(Min.x, v.x), Mathf.Min(Min.y, v.y));
            Max = new Vector2(Mathf.Max(Max.x, v.x), Mathf.Max(Max.y, v.y));
        }
    }
}
