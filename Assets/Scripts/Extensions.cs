using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class Extensions
{
    #region IENumerable

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

    #endregion

    #region NavmeshAgent

    /// <summary>
    /// Checks and returns if the <see cref="NavMeshAgent"/> has absolutely and undeniably reached their destination.
    /// </summary>
    /// <param name="agent">The <see cref="NavMeshAgent"/> to check against.</param>
    /// <returns>True if the destination is reached, and false if it hasn't yet.</returns>
    public static bool DestinationReached(this NavMeshAgent agent)
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance &&
               !agent.hasPath && agent.velocity.sqrMagnitude == 0f;
    }

    #endregion

    /// <summary>
    /// Sets the bounds of the capsuleCollider, it is used for adjusting the collider of the brother when entering stealth.
    /// </summary>
    /// <param name="height">Determines the Height of the capsuleCollider.</param>
    /// /// <param name="centerBoundX">Determines the X position of the center of the capsuleCollider.</param>
    /// /// <param name="centerBoundY">Determines the Y position of the center of the capsuleCollider.</param>
    /// /// <param name="centerBoundZ">Determines the Z position of the center of the capsuleCollider.</param>
    public static void SetCapsuleCollider(this CapsuleCollider collider, float height, float centerBoundX, float centerBoundY, float centerBoundZ)
    {
        collider.center = new Vector3(centerBoundX, centerBoundY, centerBoundZ);
        collider.height = height;
    }

    public static float Map(this float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}
