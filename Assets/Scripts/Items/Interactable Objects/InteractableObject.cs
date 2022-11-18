using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Interactable Object", menuName ="Interactable Object/Create New Interactable Object")]
public class InteractableObject : ScriptableObject
{
    [Tooltip("The ID of the interactable object.")]public int _id;
    [Tooltip("The name of the interactable object.")]public string _name;
    [Tooltip("The sort input needed to interact with the object")]public string _inputSort;
    [Tooltip("The name ")] public string _inputName;
}
