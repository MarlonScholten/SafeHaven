using System;
using System.Collections;
using System.Collections.Generic;
using InteractableItemsSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    [Tooltip("Number of points on the line.")][SerializeField] private int _numPoints = 50;
    [Tooltip("Distance between points on the line.")][SerializeField] private float _timeBetweenPoints = 0.1f;
    [Tooltip("The physics layers that will cause the line to stop being drawn.")][SerializeField] private LayerMask _collidableLayers;
    
    private ThrowableItemController _throwableItemController;
    [SerializeField] private LineRenderer _lineRenderer;
    public List<Vector3> points;
    [SerializeField] private Camera _cam;
    
    
    private Inventory _inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        _inventory = GetComponent<Inventory>();
        _throwableItemController = GetComponent<ThrowableItemController>();
        InputBehaviour.Instance.OnThrowEvent += DrawProjectionLine;
    }

    private void Update()
    {
        if(!_inventory.HasItemInInventory) return;
        
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = Mathf.CeilToInt(_numPoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _throwableItemController.ThrowDirection.position;
        Vector3 startVelocity = _throwableItemController.ThrowForce * _cam.transform.forward /
                                _inventory.ItemInInventoryObj.GetComponent<Rigidbody>().mass;
        int i = 0;
        _lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _numPoints; time += _timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            
            _lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = _lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit,
                    (point - lastPosition).magnitude, _collidableLayers))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }
        /*_lineRenderer.positionCount = (int)_numPoints;
       points = new List<Vector3>();
        Vector3 startingPosition = transform.parent.position + _throwableItemController.ThrowDirection.position;
        Vector3 startingVelocity = _throwableItemController.ThrowDirection.up * _throwableItemController.ThrowForce;

        for (float t = 0; t < _numPoints; t += _timeBetweenPoints)
        {
            Vector3 newPoint =  + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 0.5f,_collidableLayers).Length > 0)
            {
                _lineRenderer.positionCount = points.Count;
                break;
            }
        }
        _lineRenderer.SetPositions(points.ToArray());*/
    }

    private void DrawProjectionLine()
    {
        /*if(!_inventory.HasItemInInventory) return;
        
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = Mathf.CeilToInt(_numPoints / _timeBetweenPoints) + 1;
        Vector3 startPosition = _throwableItemController.ThrowDirection.position;
        Vector3 startVelocity = _throwableItemController.ThrowForce * _cam.transform.forward /
                                _inventory.ItemInInventoryObj.GetComponent<Rigidbody>().mass;
        int i = 0;
        _lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < _numPoints; time += _timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            
            _lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = _lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit,
                    (point - lastPosition).magnitude, _collidableLayers))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }*/
    }
}
