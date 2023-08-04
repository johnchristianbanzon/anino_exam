using System;

public class SlotManager : ISlotManager
{
    private SlotView _view;

    public void SetView(SlotView slotView)
    {
        _view = slotView;
    }

    public void StartSpin()
    {
        _view.StartSpin();
    }
}