/// <summary>
/// Author: Thijs Orsel, Jelco van der Straaten and Iris Giezen </para>
/// Modified by: N/A </para>
/// Contains the different actions the player can choose from of the pinging system to let the Brother AI perform.
/// </summary>
/// <list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///     <item>
///         <term>None</term>
///		    <term>Script</term>
///         <term>PingType</term>
///		    <term>This is needed to pass the information from the pinging system to the Brother AI (integration).</term>
///	    </item>
/// </list>
/// 
/// <summary>
/// Enum which contains the different actions that can be performed.
/// </summary>
/// <returns>The chosen action by the player.</returns>
public enum PingType
{
    Move,
    Hide,
    Interact,
}