using UnityEngine;
using System;
using System.Collections.Generic;

public static class MathUtils
 {
	public static bool ObjectInCone(Transform origin, Vector3 dir, float arc, float maxRange, CharacterController collider, bool accountForColliderRadius = false)
    {
        float outDistance = 0;

		bool inCone = GetDistanceInCone(origin, dir, arc, maxRange, collider, accountForColliderRadius, ref outDistance);

		if (inCone && outDistance < maxRange)
		{
			return true;
		}
		return false;
    }

	public static bool GetDistanceInCone(Transform origin, Vector3 dir, float arc, float maxRange, CharacterController collider, bool accountForColliderRadius, ref float outDistance)
    {
        outDistance = 0;

		Vector3 pos = collider.transform.position;
		var heading = (pos - origin.position).normalized;
		var dot = Vector3.Dot (origin.forward, heading);
		var acos = Mathf.Acos(dot);
		var angle = acos * 180 / Mathf.PI;

		if (angle <= arc || System.Single.IsNaN(angle))
		{
			outDistance = Vector3.Distance (origin.position, collider.transform.position);
			if (accountForColliderRadius)
			{
				outDistance -= collider.radius;
			}
			return true;
		}

		return false;
    }

    public static bool TestCollisionLineSegment(Vector2 inStart, Vector2 inEnd, CharacterController collider)
    {
        /*
        E is the starting point of the ray,
        L is the end point of the ray,
        C is the center of sphere you're testing against
        r is the radius of that sphere
        Compute:
        d = L - E ( Direction vector of ray, from start to end )
        f = E - C ( Vector from center sphere to ray start )
        */
        Vector2 d = inEnd - inStart;
        Vector2 f = inStart - (/*inCollider.offset +*/ (Vector2)collider.transform.position);
        float r = collider.radius;

        float a = Vector2.Dot(d, d);
        float b = 2 * Vector2.Dot(f, d);
        float c = Vector2.Dot(f, f) - r * r;

        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            // no intersection
        }
        else
        {
            // ray didn't totally miss sphere,
            // so there is a solution to
            // the equation.

            discriminant = Mathf.Sqrt(discriminant);

            // either solution may be on or off the ray so need to test both
            // t1 is always the smaller value, because BOTH discriminant and
            // a are nonnegative.
            float t1 = (-b - discriminant) / (2 * a);
            float t2 = (-b + discriminant) / (2 * a);

            if (t1 >= 0 && t1 <= 1)
            {
                // t1 is the intersection, and it's closer than t2
                // (since t1 uses -b - discriminant)
                // Impale, Poke
                return true;
            }

            // here t1 didn't intersect so we are either started
            // inside the sphere or completely past it
            if (t2 >= 0 && t2 <= 1)
            {
                // ExitWound
                return true;
            }

            // no intn: FallShort, Past, CompletelyInside
            return false;
        }
        return false;
    }

    public static double DistanceFromPointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        // given a line based on two points, and a point away from the line,
        // find the perpendicular distance from the point to the line.
        // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
        // for explanation and defination.        

        return Math.Abs((lineEnd.x - lineStart.x) * (lineStart.z - point.z) - (lineStart.x - point.x) * (lineEnd.z - lineStart.z)) /
                Math.Sqrt(Math.Pow(lineEnd.x - lineStart.x, 2) + Math.Pow(lineEnd.z - lineStart.z, 2));
    }
		
}
