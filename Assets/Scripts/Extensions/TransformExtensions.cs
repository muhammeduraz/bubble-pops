using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class TransformExtensions
    {
        #region Functions

        public static Transform FindChildRecursively(this Transform aParent, string aName)
        {
            if (aParent.name == aName)
            {
                return aParent;
            }
            Transform result = aParent.Find(aName);
            if (result != null)
            {
                return result;
            }

            foreach (Transform child in aParent)
            {
                result = child.FindChildRecursively(aName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public static void SetUIParent(this RectTransform transform, RectTransform parent, bool fillParent = false, bool fillAnchors = false)
        {
            transform.SetParent(parent);
            transform.localScale = Vector3.one;
            transform.anchoredPosition3D = transform.anchoredPosition3D.WithZ(0f);

            if (fillParent || fillAnchors)
            {
                if (fillParent)
                {
                    transform.anchorMin = Vector2.zero;
                    transform.anchorMax = Vector2.one;
                    transform.pivot = Vector2.one * 0.5f;
                }
                transform.offsetMin = Vector2.zero;
                transform.offsetMax = Vector2.zero;
            }
        }

        public static string FindPathToChild(this Transform transform, Transform child)
        {
            Stack<string> path = new Stack<string>(10);

            while (child != null)
            {
                if (child == transform)
                {
                    return string.Join("/", path.ToArray());
                }

                path.Push(child.name);
                child = child.parent;
            }

            return null;
        }

        public static void DestroyChildren(this Transform transform)
        {
            if (transform == null) return;
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                if (child != null)
                {
                    Object.Destroy(child.gameObject);
                }
            }
        }

        public static void DestroyChildrenImmediate(this Transform transform)
        {
            if (transform == null) return;
            while (transform.childCount > 0)
            {
                Object.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }

        public static void ForceRefreshLayout(this RectTransform transform)
        {
            var controllers = transform.GetComponentsInChildren<UnityEngine.UI.ILayoutController>();
            foreach (var controller in controllers)
            {
                if ((controller as UnityEngine.MonoBehaviour).enabled)
                {
                    controller.SetLayoutHorizontal();
                    controller.SetLayoutVertical();
                }
            }

            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
            Canvas.ForceUpdateCanvases();

            // Force world coordinate update
            var canvas = transform.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                var rootCanvas = canvas.rootCanvas;
                var cam = rootCanvas.worldCamera;
                rootCanvas.worldCamera = null;
                rootCanvas.worldCamera = cam;
            }
        }

        #endregion Functions
    }
}