using UnityEngine;
namespace Utilities.Slice.Core
{

    /**
	 * A Basic Structure which contains intersection information
	 * for Plane->Triangle Intersection Tests
	 * TO-DO -> This structure can be optimized to hold less data
	 * via an optional indices array. Could lead for a faster
	 * intersection test as well.
	 */
    public sealed class IntersectionResult
    {

        // general tag to check if this structure is valid
        private bool _isSuccess;

        // our intersection points/triangles
        private readonly Triangle[] _upperHull;
        private readonly Triangle[] _lowerHull;
        private readonly Vector3[] _intersectionPt;

        // our counters. We use raw arrays for performance reasons
        private int _upperHullCount;
        private int _lowerHullCount;
        private int _intersectionPtCount;

        public IntersectionResult()
        {
            _isSuccess = false;

            _upperHull = new Triangle[2];
            _lowerHull = new Triangle[2];
            _intersectionPt = new Vector3[2];

            _upperHullCount = 0;
            _lowerHullCount = 0;
            _intersectionPtCount = 0;
        }

        public Triangle[] upperHull => _upperHull;

        public Triangle[] lowerHull => _lowerHull;

        public Vector3[] intersectionPoints => _intersectionPt;

        public int upperHullCount => _upperHullCount;

        public int lowerHullCount => _lowerHullCount;

        public int intersectionPointCount => _intersectionPtCount;

        public bool isValid => _isSuccess;

        /**
		 * Used by the intersector, adds a new triangle to the
		 * upper hull section
		 */
        public IntersectionResult AddUpperHull(Triangle tri)
        {
            _upperHull[_upperHullCount++] = tri;

            _isSuccess = true;

            return this;
        }

        /**
		 * Used by the intersector, adds a new triangle to the
		 * lower gull section
		 */
        public IntersectionResult AddLowerHull(Triangle tri)
        {
            _lowerHull[_lowerHullCount++] = tri;

            _isSuccess = true;

            return this;
        }

        /**
		 * Used by the intersector, adds a new intersection point
		 * which is shared by both upper->lower hulls
		 */
        public void AddIntersectionPoint(Vector3 pt)
        {
            _intersectionPt[_intersectionPtCount++] = pt;
        }

        /**
		 * Clear the current state of this object 
		 */
        public void Clear()
        {
            _isSuccess = false;
            _upperHullCount = 0;
            _lowerHullCount = 0;
            _intersectionPtCount = 0;
        }

        /**
		 * Editor only DEBUG functionality. This should not be compiled in the final
		 * Version.
		 */
        public void OnDebugDraw()
        {
            OnDebugDraw(Color.white);
        }

        public void OnDebugDraw(Color drawColor)
        {
            #if UNITY_EDITOR

            if (!isValid)
            {
                return;
            }

            var prevColor = Gizmos.color;

            Gizmos.color = drawColor;

            // draw the intersection points
            for (int i = 0; i < intersectionPointCount; i++)
            {
                Gizmos.DrawSphere(intersectionPoints[i], 0.1f);
            }

            // draw the upper hull in RED
            for (int i = 0; i < upperHullCount; i++)
            {
                upperHull[i].OnDebugDraw(Color.red);
            }

            // draw the lower hull in BLUE
            for (int i = 0; i < lowerHullCount; i++)
            {
                lowerHull[i].OnDebugDraw(Color.blue);
            }

            Gizmos.color = prevColor;

            #endif
        }
    }
}
