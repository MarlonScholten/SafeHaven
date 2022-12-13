using UnityEngine;
using PathCreation;

/// <summary>
/// Author: Iris Giezen </para>
/// Modified by: N/A </para>
/// This script controls the placement and amount of prefabs that are placed on the chosen path.
/// In order to let it work this script should be added to the path creator object.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>Should be placed on the path creator object (together with the path creator script).</term>
///		    <term>Script</term>
///         <term>PlaceObjectsLine</term>
///		    <term>This script makes it possible that prefabs are placed on a created path with a specified distance.</term>
///	    </item>
/// </list>
[ExecuteInEditMode]
public class PlaceObjectsLine : MonoBehaviour
{
    /// <summary>
    /// In here should be the path creator object, which contains the created path. 
    /// </summary>
    [Tooltip("The path creator on which the prefabs should be placed.")]
    [SerializeField] private PathCreator _pathCreator;

    /// <summary>
    /// In here should be the prefab, which is used for creating the path. 
    /// </summary>
    [Tooltip("The prefabs that should be placed on the path.")]
    [SerializeField] private GameObject _prefabObject;
    
    /// <summary>
    /// In here should be the empty object, which is gonna contain the instantiated objects. 
    /// </summary>
    [Tooltip("The empty object in which the instantiated objects should be placed.")]
    [SerializeField] private GameObject _holderPrefabObject;

    /// <summary>
    /// This sets the distance between the instantiated objects.
    /// </summary>
    [Tooltip("The distance between the instantiated objects.")]
    [SerializeField] float spaceBetweenObjects = 3.0f;
    
    /// <summary>
    /// The minimal distance between the instantiated objects.
    /// </summary>
    private const float MinSpace = 0.1f;
    
    /// <summary>
    /// Bool for checking if the path creator is null.
    /// </summary>
    private bool _isPathCreatorNull;
    
    /// <summary>
    /// Bool for checking if the prefab object is null.
    /// </summary>
    private bool _isPrefabObjectNull;
    
    /// <summary>
    /// Bool for checking if the prefab holder is null.
    /// </summary>
    private bool _isHolderPrefabObjectNull;

    /// <summary>
    /// Sets the booleans to null.
    /// </summary>
    private void Start()
    {
        _isPathCreatorNull = _pathCreator == null;
        _isPrefabObjectNull = _prefabObject == null;
        _isHolderPrefabObjectNull = _holderPrefabObject == null;
    }

    /// <summary>
    /// Creates the path.
    /// </summary>
    private void Update()
    {
        if (_isPathCreatorNull) return;
        CreatePath();
    }

    /// <summary>
    /// Destroys the 'old' objects and instantiates new ones for the path.
    /// </summary>
    private void CreatePath()
    {
        if (_isPathCreatorNull || _isPrefabObjectNull || _isHolderPrefabObjectNull) return;
        DestroyObjects();
        InstantiateGameObjectsPath();
    }

    /// <summary>
    /// Instantiates objects for the path with a set distance between.
    /// </summary>
    private void InstantiateGameObjectsPath()
    {
        spaceBetweenObjects = Mathf.Max(MinSpace, spaceBetweenObjects);

        var distancePath = 0f;
        var path = _pathCreator.path;
        while (distancePath < path.length)
        {
            Instantiate(_prefabObject, path.GetPointAtDistance(distancePath), path.GetRotationAtDistance(distancePath),
                _holderPrefabObject.transform);
            distancePath += spaceBetweenObjects;
        }
    }

    /// <summary>
    /// Destroys objects.
    /// </summary>
    private void DestroyObjects()
    {
        var childCount = _holderPrefabObject.transform.childCount;
        for (var i = childCount - 1; 0 <= i; i--)
        {
            DestroyObject(i);
        }
    }

    /// <summary>
    /// Destroys a specified object.
    /// </summary>
    /// <param name="number">The number of the object that should be destroyed.</param>
    private void DestroyObject(int number)
    {
        var childObject = _holderPrefabObject.transform.GetChild(number).gameObject;
        DestroyImmediate(childObject, false);
    }
}