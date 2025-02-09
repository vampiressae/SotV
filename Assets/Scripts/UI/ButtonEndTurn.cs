public class ButtonEndTurn : UIButtonBase
{
    protected override void Click()
    {
        FightController.Instance.EndTurn();
    }
}
