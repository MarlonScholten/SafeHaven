using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutroTrigger : MonoBehaviour, ITrigger
{
    [SerializeField]
    private bool _playerWithinRange;

    [SerializeField]
    private bool _brotherWithinRange;

    [SerializeField]
    private bool _gameFinished => _playerWithinRange && _brotherWithinRange;

    private void UpdateWithinRange(GameObject instigator, bool enter)
    {
        _playerWithinRange = instigator.CompareTag("Player") ? enter : _playerWithinRange;
        _brotherWithinRange = instigator.CompareTag("Brother") ? enter : _brotherWithinRange;

        if (enter && _gameFinished)
            FindObjectOfType<IntroOutroManager>().StartOutro();
    }

    /// <remarks>Param not optional.</remarks>
    /// <inheritdoc />
    public void TriggerEnter(GameObject instigator) => UpdateWithinRange(instigator, true);

    /// <remarks>Param not optional.</remarks>
    /// <inheritdoc />
    public void TriggerExit(GameObject instigator) => UpdateWithinRange(instigator, false);
}
