using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Orders a given list of <see cref="GameObject"/> by distance, and returns the closest one relative to the origin pos.
    /// </summary>
    /// <param name="gameObjects">The list of <see cref="GameObject"/> to pull from.</param>
    /// <param name="originPos">The relative origin pos the function will use to compare it to.</param>
    /// <returns>The closest <see cref="GameObject"/> relative to the origin pos.</returns>
    /// <exception cref="NullReferenceException">Returns a null reference if gameobjects is null/empty.</exception>
    public static GameObject GetClosestGameObject(this IEnumerable<GameObject> gameObjects, Vector3 originPos)
    {
        GameObject closest = gameObjects?.OrderBy(x => Vector3.Distance(originPos, x.transform.position)).FirstOrDefault();

        if (closest == null)
            throw new NullReferenceException("Error, gameObjects can not be null.");

        return closest;
    }
}
