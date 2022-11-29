using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Author: Marlon Kerstens<br/>
/// Modified by: <br/>
/// Description: A script to create a custom inspector for the EnemyAiStateManager class.
/// This is made so the inspector gives the option to add one waypoint when the enemy is a guard and
/// it gives the option to add a patrol path when the enemy is nog a guard.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>none</term>
///		    <term>Script</term>
///         <term>None</term>
///		    <term>This script will be automatic recognized by Unity Editor.</term>
///	    </item>
/// </list>
#if UNITY_EDITOR // Only run when it is in editor mode. 
    [CustomEditor(typeof(EnemyAiStateManager))]
    public class EnemyAiStateManagerEditor : Editor
    {
        private bool _showWaypoints = true; // A bool to show or hide the waypoints in the inspector.

        /// <summary>
        /// A method to draw the inspector with custom input.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draw the default inspector.
            EnemyAiStateManager enemyAiStateManager = (EnemyAiStateManager)target;
            // Enemy is not a guard.
            if (!enemyAiStateManager.isGuard)
            {
                // A foldout to show or hide the waypoints.
                _showWaypoints = EditorGUILayout.Foldout(_showWaypoints, "Waypoints", true); 

                if (_showWaypoints)
                {
                    EditorGUI.indentLevel++;
                    List<Transform> wayPoints = enemyAiStateManager.wayPoints;
                    int size = Mathf.Max(1, EditorGUILayout.IntField("Size", wayPoints.Count));

                    while (size > wayPoints.Count)
                    {
                        wayPoints.Add(null);
                    }

                    while (size < wayPoints.Count)
                    {
                        wayPoints.RemoveAt(wayPoints.Count - 1);
                    }

                    for (int i = 0; i < wayPoints.Count; i++)
                    {
                        wayPoints[i] =
                            EditorGUILayout.ObjectField("Waypoint " + i, wayPoints[i], typeof(Transform),
                                true) as Transform;
                    }

                    EditorGUI.indentLevel--;
                }
            }
            else
            { 
                // Enemy is a guard.
               enemyAiStateManager.guardWaypoint =  EditorGUILayout.ObjectField("Guard Waypoint", enemyAiStateManager.guardWaypoint, typeof(Transform), true) as Transform;
            }
        }
    }
#endif

