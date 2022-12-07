using UnityEngine;
 
/// <summary>
/// This script creates a proppertyAttribute for the tagSelector. This selector makes it possible to get a list of all the tags.
/// </summary>
public class TagSelectorAttribute : PropertyAttribute
{
    public bool UseDefaultTagFieldDrawer = false;
}