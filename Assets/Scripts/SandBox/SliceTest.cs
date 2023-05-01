using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.Slice;

namespace SandBox
{
    public class SliceTest : MonoBehaviour
    {
        public Material cross;
        public LayerMask layer;
        public GameObject plane;
        public BoxCollider boxCollider;

        private Vector3 w => plane.transform.position + Vector3.up * (plane.transform.lossyScale.x * 5);
        private Vector3 a => plane.transform.position + Vector3.down * (plane.transform.lossyScale.x * 5);
        private Vector3 s => plane.transform.position + Vector3.left * (plane.transform.lossyScale.z * 5);
        private Vector3 d => plane.transform.position + Vector3.right * (plane.transform.lossyScale.z * 5);

        public Collider[] result = { };
        public List<Collider> result2 = new List<Collider>();
        public Collider[] resultW = { };
        public Collider[] resultA = { };
        public Collider[] resultS = { };
        public Collider[] resultD = { };



        public GameObject[] planes = { };



        private void Start()
        {
            layer = LayerMask.GetMask("Sliceable");
        }

        // Update is called once per frame
        private void Update()
        {
            /*           plane.transform.position =
                           Explorer.TryGetService<CameraService>().mainCamera.ScreenToWorldPoint(
                               new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 10.95f));
           
           
           
           
                       Physics.OverlapBoxNonAlloc(w, new Vector3(d.x - a.x, .1f, 10), resultW, Quaternion.identity, layer);
                       Physics.OverlapBoxNonAlloc(a, new Vector3(d.x - a.x, .1f, 10), resultA, Quaternion.identity, layer);
                       Physics.OverlapBoxNonAlloc(s, new Vector3(.1f, w.y - s.y, 10), resultS, Quaternion.identity, layer);
                       Physics.OverlapBoxNonAlloc(d, new Vector3(.1f, w.y - s.y, 10), resultD, Quaternion.identity, layer);
           
           
           
           
     



            //检测鼠标横向移动
            float mx = Input.GetAxis("Mouse X");
            transform.Rotate(0, 0, mx * 2);      */
            if (!Input.GetMouseButtonDown(0)) return;


            foreach (var pla in planes)
            {
                Cut(result, pla.transform);
            }


            //盒子射线检测
            // var colliders = Physics.OverlapBox(boxCollider.transform.position, boxCollider.size, transform.rotation, layer);


        }



        private void Cut(Collider[] colliders, Transform trans)
        {

            result2.Clear();
            //将每一个检测到的进行切割
            foreach (var c in colliders)
            {
                //添加切割面的材质
                //切割并返回表皮
                var hull = c.gameObject.Slice(trans.position, trans.up);
                if (hull != null)
                {
                    var lower = hull.CreateLowerHull(c.gameObject, cross);
                    var upper = hull.CreateUpperHull(c.gameObject, cross);
                    GameObject[] objs = { lower, upper };
                    foreach (var o in objs)
                    {
                        o.AddComponent<Rigidbody>().useGravity = false;
                        var meshCollider = o.AddComponent<MeshCollider>();
                        meshCollider.convex = true;
                        result2.Add(meshCollider);
                        o.layer = LayerMask.NameToLayer("Sliceable");
                    }
                    Destroy(c.gameObject);
                }
                else
                {
                    result2.Add(c);
                }
            }


            result = result2.ToArray();
        }



        private void OnDrawGizmos()
        { /*
            //w
            Gizmos.DrawSphere(w, .1f);
            Gizmos.DrawCube(w, new Vector3(d.x - a.x, .1f, 10));
            //s
            Gizmos.DrawSphere(a, .1f);
            //a
            Gizmos.DrawSphere(s, .1f);
            //d
            Gizmos.DrawSphere(d, .1f);*/
        }
    }
}
