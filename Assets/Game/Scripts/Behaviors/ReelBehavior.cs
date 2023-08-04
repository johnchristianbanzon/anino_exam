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
    private SimpleScrollSnap _simpleScrollSnap;
    private bool _startedSpin;
    private int _reelNumber = 0;

    public void SetReelNumber(int number)
    {
        _reelNumber = number;
    }

    public void Spin()
    {
        //Debug.Log("Random.Range(-2500, - 5000) * Vector2.up :" + Random.Range(-2500, -5000) * Vector2.up);
        //_simpleScrollSnap.Velocity = (-3000 * Vector2.up);
        //_simpleScrollSnap.GoToPanel(1500);
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