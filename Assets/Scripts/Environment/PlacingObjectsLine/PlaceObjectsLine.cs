using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
public class PlaceObjectsLine : MonoBehaviour
{
    public PathCreator pathCreator;

    public GameObject prefabObject;
    public GameObject holderPrefabObject;

    private bool _isPathCreatorNull;
    private bool _isPrefabObjectNull;
    private bool _isHolderPrefabObjectNull;

    public float spaceBetweenObjects = 3.0f;
    private const float MinSpace = 0.1f;

    private void Start()
    {
        _isPathCreatorNull = pathCreator == null;
        _isPrefabObjectNull = prefabObject == null;
        _isHolderPrefabObjectNull = holderPrefabObject == null;
    }

    private void Update()
    {
        if (_isPathCreatorNull) return;
        CreatePath();
    }

    private void CreatePath()
    {
        if (_isPathCreatorNull || _isPrefabObjectNull || _isHolderPrefabObjectNull) return;
        DestroyObjects();
        InstantiateGameObjectsPath();
    }

    private void InstantiateGameObjectsPath()
    {
        spaceBetweenObjects = Mathf.Max(MinSpace, spaceBetweenObjects);

        var distancePath = 0f;
        var path = pathCreator.path;
        while (distancePath < path.length)
        {
            Instantiate(prefabObject, path.GetPointAtDistance(distancePath), path.GetRotationAtDistance(distancePath),
                holderPrefabObject.transform);
            distancePath += spaceBetweenObjects;
        }
    }

    private void DestroyObjects()
    {
        var childCount = holderPrefabObject.transform.childCount;
        for (var i = childCount - 1; 0 <= i; i--)
        {
            DestroyObject(i);
        }
    }

    private void DestroyObject(int i)
    {
        var childObject = holderPrefabObject.transform.GetChild(i).gameObject;
        DestroyImmediate(childObject, false);
    }
}