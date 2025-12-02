using System.Collections.Generic;
using UnityEngine;

namespace soulercoasterLite.scripts.pathGenerators {
    [RequireComponent(typeof(LineRenderer))]
    public class HelixLine : MonoBehaviour {
        [Header("Generates a helix from origin to destination.")] 
        public Transform origin;
        public Transform destination;
        [Tooltip("Radius at the origin")]
        public float startRadius = 10;
        [Tooltip("Radius at the destination")]
        public float endRadius = 10;

        [Tooltip("Number of points to generate")]
        public float resolution=32;

        [Tooltip("Number of rotations in the helix")]
        public float rotations = 1;

        public bool liveUpdate = true;

        public void OnValidate() {
            this.setupLineRenderer();
            if (liveUpdate) {
                generate();
            }
        }

        [ContextMenu("generate")]
        public void generate() {
            var generalDirection = Vector3.Normalize(destination.position - origin.position);
            var currentPointOnLine = origin.position;
            var length = Vector3.Distance(origin.position, destination.position);

            var i = 0;
            var points = new List<Vector3>();
            var perpendicular = Vector3.Normalize(Vector3.Cross(generalDirection, Vector3.up));
            if (perpendicular == Vector3.zero) {
                perpendicular = Vector3.Normalize(Vector3.Cross(generalDirection, Vector3.forward));
            }

            var stepSize = length / resolution;
            var helixFrequency = (360 / resolution)*rotations;

            while (Vector3.Distance(currentPointOnLine, destination.position) > stepSize) {
                var circleVector = Quaternion.AngleAxis(i * helixFrequency , generalDirection) * perpendicular;
                var radius = startRadius + (endRadius - startRadius) * ((length-Vector3.Distance(currentPointOnLine, destination.position)) / length);
                var nextPoint = currentPointOnLine + (generalDirection + circleVector * radius);
                points.Add(nextPoint);
                i++;
                currentPointOnLine += generalDirection * stepSize;
                if (i > 1000) {
                    break;
                }
            }

            GetComponent<LineRenderer>().positionCount = points.Count;
            GetComponent<LineRenderer>().SetPositions(points.ToArray());
        }
    }
}