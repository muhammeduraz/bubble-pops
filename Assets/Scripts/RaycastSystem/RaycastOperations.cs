using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.RaycastSystem
{
    public class RaycastOperations
    {
        #region Variables

        private Camera _mainCamera;

        #endregion Variables

        #region Functions

        public RaycastOperations(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

        public T GetObjectOfTypeWithinAll<T>(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity);

            for (int i = 0; i < raycastHitArray.Length; i++)
            {
                requiredObject = raycastHitArray[i].collider.GetComponent<T>();

                if (requiredObject != null)
                {
                    break;
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeWithinAllNonAlloc<T>(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = null;
            Physics.RaycastNonAlloc(ray, raycastHitArray, Mathf.Infinity);

            if (raycastHitArray.Length > 0)
            {
                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    requiredObject = raycastHitArray[i].collider.GetComponent<T>();

                    if (requiredObject != null)
                    {
                        break;
                    }
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeWithinAll<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                Collider collider = null;

                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    collider = raycastHitArray[i].collider;

                    if (collider)
                    {
                        requiredObject = collider.GetComponent<T>();
                        break;
                    }
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeWithinAllNonAlloc<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;
            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = null;
            Physics.RaycastNonAlloc(ray, raycastHitArray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                Collider collider = null;

                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    collider = raycastHitArray[i].collider;

                    if (collider)
                    {
                        requiredObject = collider.GetComponent<T>();
                        break;
                    }
                }
            }

            return requiredObject;
        }

        public T GetObjectOfType<T>(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponent<T>();
                }
            }

            return requiredObject;
        }

        public T GetObjectOfType<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layer))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponent<T>();
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeInChildren<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layer))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponentInChildren<T>();
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeInParent<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layer))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponentInParent<T>();
                }
            }

            return requiredObject;
        }

        public List<T> GetObjectsOfType<T>(int layer, Vector3 screenPosition)
        {
            Ray ray = new Ray();
            List<T> requiredObjectList = new List<T>();

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                Collider collider = null;

                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    collider = raycastHitArray[i].collider;

                    if (collider)
                    {
                        requiredObjectList.Add(collider.GetComponent<T>());
                    }
                }
            }

            return requiredObjectList;
        }

        public T GetObjectOfTypeFromTransform<T>(int layer, Vector3 offset, Transform transform, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default(T);
            List<T> objectList = new List<T>();

            ray = new Ray(transform.position + offset, direction);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                Collider collider = null;

                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    collider = raycastHitArray[i].collider;

                    if (collider)
                    {
                        requiredObject = collider.GetComponent<T>();
                        break;

                    }
                }
            }

            return requiredObject;
        }

        public List<T> GetObjectsOfTypeFromTransform<T>(int layer, Vector3 offset, Transform transform, Vector3 direction)
        {
            Ray ray = new Ray();
            List<T> requiredObjectList = new List<T>();

            ray = new Ray(transform.position + offset, direction);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                Collider collider = null;

                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    collider = raycastHitArray[i].collider;

                    if (collider)
                    {
                        requiredObjectList.Add(collider.GetComponent<T>());
                    }
                }
            }

            return requiredObjectList;
        }

        public T GetObjectOfTypeByRaycastingWithDirection<T>(GameObject gameObject, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = new Ray(gameObject.transform.position, direction);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity);

            if (raycastHitArray != null && raycastHitArray.Length > 0)
            {
                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    if (raycastHitArray[i].collider.GetComponent<T>() != null)
                    {
                        requiredObject = raycastHitArray[i].collider.GetComponent<T>();
                        break;
                    }
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeByRaycastingWithDirection<T>(int layer, GameObject gameObject, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = new Ray(gameObject.transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layer))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponent<T>();
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeByRaycastingWithDirectionParent<T>(GameObject gameObject, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = new Ray(gameObject.transform.position, direction);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity))
            {
                if (raycastHit.collider)
                {
                    requiredObject = raycastHit.collider.GetComponentInParent<T>();
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeByRaycastingWithDirectionWithinAll<T>(int layer, GameObject gameObject, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = new Ray(gameObject.transform.position, direction);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity, layer);

            if (raycastHitArray.Length > 0)
            {
                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    if (raycastHitArray[i].collider)
                    {
                        requiredObject = raycastHitArray[i].collider.GetComponent<T>();

                        if (requiredObject != null)
                        {
                            return requiredObject;
                        }
                    }
                }
            }

            return requiredObject;
        }

        public T GetObjectOfTypeByRaycastingWithDirectionWithinAll<T>(GameObject gameObject, Vector3 direction)
        {
            Ray ray = new Ray();
            T requiredObject = default;

            ray = new Ray(gameObject.transform.position, direction);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity);

            if (raycastHitArray.Length > 0)
            {
                for (int i = 0; i < raycastHitArray.Length; i++)
                {
                    if (raycastHitArray[i].collider)
                    {
                        requiredObject = raycastHitArray[i].collider.GetComponent<T>();

                        if (requiredObject != null)
                        {
                            return requiredObject;
                        }
                    }
                }
            }

            return requiredObject;
        }

        public Vector3 GetHitPoint(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            Vector3 hitPoint = Vector3.zero;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            bool didHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

            if (didHit)
            {
                hitPoint = hit.point;
            }

            return hitPoint;
        }

        public Vector3 GetHitPoint<T>(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            Vector3 hitPoint = Vector3.zero;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity);

            for (int i = 0; i < raycastHitArray.Length; i++)
            {
                if (raycastHitArray[i].collider.GetComponent<T>() != null && raycastHitArray[i].collider)
                {
                    hitPoint = raycastHitArray[i].point;
                }
            }

            return hitPoint;
        }

        public Vector3 GetHitPointByDirectionFromPosition(Vector3 center, Vector3 direction)
        {
            Ray ray = new Ray();
            Vector3 hitPoint = Vector3.zero;

            ray = new Ray(center, direction);

            bool didHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

            if (didHit)
            {
                hitPoint = hit.point;
            }

            return hitPoint;
        }

        public Vector3 GetSurfaceNormal(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            Vector3 normal = Vector3.zero;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            bool didHit = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

            if (didHit)
            {
                normal = hit.normal;
            }

            return normal;
        }

        public Vector3 GetSurfaceNormal<T>(Vector3 screenPosition)
        {
            Ray ray = new Ray();
            Vector3 normal = Vector3.zero;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, Mathf.Infinity);

            for (int i = 0; i < raycastHitArray.Length; i++)
            {
                if (raycastHitArray[i].collider.GetComponent<T>() != null && raycastHitArray[i].collider)
                {
                    normal = raycastHitArray[i].normal;
                }
            }

            return normal;
        }

        public Vector3 GetRaycastPlanePoint(Vector3 screenPosition, Vector3 inNormal, Vector3 inPosition)
        {
            Ray ray = new Ray();
            Plane plane = new Plane(inNormal, inPosition);
            Vector3 hitPoint = Vector3.zero;

            ray = _mainCamera.ScreenPointToRay(screenPosition);

            bool didHit = plane.Raycast(ray, out float distance);

            if (didHit)
            {
                hitPoint = ray.GetPoint(distance);
            }

            return hitPoint;
        }

        #endregion Functions
    }
}