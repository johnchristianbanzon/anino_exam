using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class ReelBehavior : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private SymbolBehavior[] _symbolBehaviors;
    [SerializeField]
    private SimpleScrollSnap _simpleScrollSnap;
    private bool _startedSpin;
    private int _reelNumber = 0;

    public void SetReelNumber(int number)
    {
        _reelNumber = number;
        for (int i = 0; i < _symbolBehaviors.Length; i++)
        {
            var symbol = UnityEngine.Random.Range(0, 6);
            _symbolBehaviors[i].SetSymbol(symbol);
        }
    }

    public void Spin()
    {
        StartCoroutine("StopReel");
        _startedSpin = true;
    }

    private IEnumerator StopReel()
    {
        yield return new WaitForSeconds(3 + (0.45f * _reelNumber));
        _startedSpin = false;
        _simpleScrollSnap.Velocity = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        if (_startedSpin==false)
        {
            return;
        }
        _simpleScrollSnap.Velocity = new Vector2(0, -5000f);
        /*        if (_simpleScrollSnap.Velocity.y >= -5000)
                {
                    _simpleScrollSnap.Velocity -= new Vector2(0, 10f);
                }*/

    }
}