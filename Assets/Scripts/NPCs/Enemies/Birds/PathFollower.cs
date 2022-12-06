using System.Collections.Generic;
using PathCreation;
using UnityEngine;

namespace NPCs.Enemies.Birds
{
    public class PathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator pathToFollow;
        [SerializeField] private float speed = 5;
        [SerializeField] private int startPointInPercentage = 0;
        private float _distanceTravelled;
        
        // Start is called before the first frame update
        void Start()
        {
           SetStartPoint();
        }

        // Update is called once per frame
        void Update()
        {
            TravelPath();
        }

        private void SetStartPoint()
        {
            var pathLength = pathToFollow.path.length;
            _distanceTravelled = pathLength * startPointInPercentage / 100;
        }
        private void TravelPath()
        {
            _distanceTravelled += speed * Time.deltaTime;
            transform.position = pathToFollow.path.GetPointAtDistance(_distanceTravelled);
            transform.rotation = pathToFollow.path.GetRotationAtDistance(_distanceTravelled);
        }
    }
}
