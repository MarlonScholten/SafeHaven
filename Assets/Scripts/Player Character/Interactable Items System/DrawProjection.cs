using System;
using System.Collections;
using System.Collections.Generic;
using InteractableItemsSystem;
using UnityEngine;
using BreakableObjects;

namespace InteractableItemsSystem
{
    /// <summary>
    /// Author: Jasper Driessen <br/>
    /// Modified by:  <br/>
    /// Description: Draws a line of how and where the item will be thrown.
    /// </summary>
    /// <list type="table">
    ///	    <listheader>
    ///         <term>On what GameObject</term>
    ///         <term>Type</term>
    ///         <term>Name of type</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term>Player GameObject</term>
    ///         <term>Prefab</term>
    ///         <term>Player Interact System</term>
    ///         <term>Used for controlling the interaction with items of the player.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Component</term>
    ///         <term>Line Renderer</term>
    ///         <term>Used for drawing the line of where you are throwing.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>SerializedField</term>
    ///         <term>Collidable layers</term>
    ///         <term>All the layers the item could hit.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Script</term>
    ///         <term>Inventory</term>
    ///         <term>Inventory for the player where the item the player is holding will be stored.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Script</term>
    ///         <term>PlayerItemInteraction</term>
    ///         <term>Used for interacting with objects and items.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Script</term>
    ///         <term>ThrowableItemController</term>
    ///         <term>Used for controlling the throwing of items.</term>
    ///     </item>
    ///     <item>
    ///         <term>Player Interact System</term>
    ///         <term>Component</term>
    ///         <term>Line Renderer</term>
    ///         <term>Used for creating the visual of the line where the item will land when throwing.</term>
    ///     </item>
    /// </list>
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(ThrowableItemController))]
    [RequireComponent(typeof(PlayerItemInteraction))]
    public class DrawProjection : MonoBehaviour
    {
        [Tooltip("Number of points on the line.")] [SerializeField] [Range(10, 100)]
        private int _numPoints = 50;

        [Tooltip("Distance between points on the line.")] [SerializeField] [Range(0.01f, 0.25f)]
        private float _timeBetweenPoints = 0.1f;

        [Tooltip("The physics layers that will cause the line to stop being drawn.")] [SerializeField]
        private LayerMask _collidableLayers;

        //Changed in other scripts.
        /// <summary>
        /// Determines if a line should be drawn or not.
        /// </summary>
        [NonSerialized] public bool DrawLine;

        private Renderer _renderer;
        
        private LineRenderer _lineRenderer;
        private Inventory _inventory;
        private ThrowableItemController _throwableItemController;
        private PlayerItemInteraction _playerItemInteraction;

        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _inventory = GetComponent<Inventory>();
            _throwableItemController = GetComponent<ThrowableItemController>();
            _playerItemInteraction = GetComponent<PlayerItemInteraction>();
            InputBehaviour.Instance.OnThrowEvent += StartDrawingLine;
        }

        private void StartDrawingLine()
        {
            DrawLine = true;
            _playerItemInteraction.PlayerController.DisableMovement();
        }

        private void Update()
        {
            if(_renderer != null) _renderer.material.DisableKeyword("_EMISSION");
            if (DrawLine) DrawProjectionLine();
        }

        private void DrawProjectionLine()
        {
            
            if (!_inventory.HasItemInInventory) return;
            
            
            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = Mathf.CeilToInt(_numPoints / _timeBetweenPoints) + 1;
            Vector3 startPosition = _playerItemInteraction.ItemHolder.transform.position;
            Vector3 startVelocity = _throwableItemController.ThrowForce *
                                    _throwableItemController.Cam.transform.forward /
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
                    if (hit.transform.GetComponent<BreakObject>() != null)
                    {
                        HighlightMaterial(hit);
                    }
                    _lineRenderer.SetPosition(i, hit.point);
                    _lineRenderer.positionCount = i + 1;
                    return;
                }
            }
        }

        private void HighlightMaterial(RaycastHit hit)
        {
            _renderer = hit.transform.GetComponent<Renderer>();
            if (_renderer == null) return;
            
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", _playerItemInteraction.HighlightColor);
        }
    }
}
