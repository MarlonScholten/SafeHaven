using UnityEngine;
 
/// <summary>
/// This script creates a proppertyAttribute for the tagSelector. This selector makes it possible to get a list of all the tags.
/// </summary>
public class TagSelectorAttribute : PropertyAttribute
{
    /// <summary>
    /// This bool will be set to true if the tags a default. This bool will be used for the TagSelectorPropertyDrawer.
    /// </summary>
    public bool UseDefaultTagFieldDrawer = false;
}