using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace NPCs.Enemies.Birds
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private Transform restPoint;
        [SerializeField] private float speed = 5;
        private float _distanceTravelled;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(transform.position, restPoint.position) > 0.2f)
                {
                    _distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled);
                    transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravelled);
                }
        }
    }
}
