﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Collision
{
    public interface IBroadphase
    {
        OverlappingPairCache CollisionPairs { get; }

        void CalculateCollisionPairs();
        void SetProxyAABB(BroadphaseProxy proxy, ref AABB aabb);
        void RemoveProxy(BroadphaseProxy proxy);
        //void AABBAtPoint(Vector2 point);
        //void AABBsWithinArea(Rectangle area);
    }
}
