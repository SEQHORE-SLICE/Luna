using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Slice;
namespace SandBox
{
    public class SliceTest : MonoBehaviour
    {
        public Material cross;
        public LayerMask layer;
        public GameObject plane;
        public Quaternion rotation;
        
        
        private Vector3 boxColliderCenter => new Vector3(plane.transform.lossyScale.z * 10, plane.transform.lossyScale.x * 10, 10);
        private Vector3 w => plane.transform.position + Vector3.up * (plane.transform.lossyScale.x * 5);
        private Vector3 a => plane.transform.position + Vector3.down * (plane.transform.lossyScale.x * 5);
        private Vector3 s => plane.transform.position + Vector3.left * (plane.transform.lossyScale.z * 5);
        private Vector3 d => plane.transform.position + Vector3.right * (plane.transform.lossyScale.z * 5);
        private List<Collider> _colliders = new List<Collider>();
        private readonly Collider[] _collider2 = { };

        
        private void Start()
        {
            layer = LayerMask.GetMask("Sliceable");
        }
        private void Update()
        {


            if (!Input.GetMouseButtonDown(0)) return;
            Physics.OverlapBoxNonAlloc(transform.position, boxColliderCenter, _collider2, rotation, mask: layer);
            _colliders = _collider2.ToList();
            Cut(w, Vector3.down);
            Cut(a, Vector3.up);
            Cut(s, Vector3.right);
            Cut(d, Vector3.left);
        }

        private void Cut(Vector3 position, Vector3 direction)
        {
            int count = _colliders.Count;
            for (int index = 0; index < count; index++)
            {
                var c = _colliders[index];
                var hull = c.gameObject.Slice(position, direction);
                if (hull == null) continue;
                var upper = hull.CreateUpperHull(c.gameObject, cross);
                upper.AddComponent<Rigidbody>().useGravity = false;
                var meshCollider = upper.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                _colliders.Add(meshCollider);
                upper.layer = LayerMask.NameToLayer("Sliceable");
                _colliders.Remove(c);
                c.gameObject.SetActive(false);
            }
        }
        private void OnDrawGizmos()
        {
            //w
            Gizmos.DrawSphere(w, .1f);
            Gizmos.DrawLine(w, w + Vector3.down);
            //s
            Gizmos.DrawSphere(a, .1f);
            Gizmos.DrawLine(a, a + Vector3.up);
            //a
            Gizmos.DrawSphere(s, .1f);
            Gizmos.DrawLine(s, s + Vector3.right);
            //d
            Gizmos.DrawSphere(d, .1f);
            Gizmos.DrawLine(d, d + Vector3.left);
        }
        private void OnDrawGizmosSelected() { Gizmos.DrawCube(transform.position, boxColliderCenter); }
    }
}
