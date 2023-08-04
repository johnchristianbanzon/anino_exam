using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SlotView : MonoBehaviour
{
    [SerializeField]
    private Button _spinButton;
    [Inject]
    private ISlotManager _slotManager;
    [SerializeField]
    private ReelBehavior[] _reels;


    private void Start()
    {
        _spinButton.onClick.AddListener(OnClickSpin);
        _slotManager.SetView(this);

        Setup();
    }

    private void Setup()
    {
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].SetReelNumber(i);
        }
    }

    private void OnClickSpin()
    {

        Debug.Log("SlotView ");
        StartSpin();
    }

    public void StartSpin()
    {
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].Spin();
        }
    }
}
