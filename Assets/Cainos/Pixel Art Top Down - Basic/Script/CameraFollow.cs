using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    // Let the camera follow the target and deactivate/reactivate objects
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;
        public float reactivateDistance = 20.0f; // Distance at which objects should reactivate

        private Vector3 offset;
        private Vector3 targetPos;
        private Camera mainCamera;

        private List<GameObject> deactivatedObjects = new List<GameObject>();

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
            mainCamera = Camera.main; // Get the main camera
        }

        private void Update()
        {
            if (target == null) return;

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);

            // Reactivate objects when they are close enough to enter the camera's view
            ReactivateObjects();

            // Deactivate objects outside the camera's view
            DeactivateObjects();
        }

        private void ReactivateObjects()
        {
            for (int i = deactivatedObjects.Count - 1; i >= 0; i--)
            {
                GameObject obj = deactivatedObjects[i];
                if (Vector3.Distance(obj.transform.position, transform.position) <= reactivateDistance)
                {
                    obj.SetActive(true);
                    deactivatedObjects.RemoveAt(i);
                }
            }
        }

        private void DeactivateObjects()
        {
            if (mainCamera == null) return;

            // Get the camera's frustum
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

            // Find all objects with a Collider2D component in the scene
            Collider2D[] allColliders = FindObjectsOfType<Collider2D>();

            foreach (Collider2D collider in allColliders)
            {
                if (!collider.gameObject.activeSelf)
                    continue;

                // Check if the collider bounds are outside the camera's frustum
                if (!GeometryUtility.TestPlanesAABB(frustumPlanes, collider.bounds) && collider.gameObject.layer == 10)
                {
                    // Deactivate the object if it's outside the view
                    collider.gameObject.SetActive(false);
                    deactivatedObjects.Add(collider.gameObject);
                }
            }
        }
    }
}
