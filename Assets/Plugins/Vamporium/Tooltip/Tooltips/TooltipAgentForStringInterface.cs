public interface ITooltipString
{
    string Title { get; }
    string Description { get; }
    bool Divider { get; }
}

public class TooltipAgentForStringInterface : TooltipAgent<TooltipForString>
{
    private ITooltipString _iTooltip;

    public override bool IsEmpty => string.IsNullOrEmpty(_iTooltip.Title) && string.IsNullOrEmpty(_iTooltip.Description);

    private void Awake() => _iTooltip = GetComponent<ITooltipString>();

    protected override void Init(TooltipForString tooltip)
    {
        if (_iTooltip == null) tooltip.Init("<color=red>Tooltip interface not found!</color>");
        else tooltip.Init(_iTooltip.Title, _iTooltip.Description, _iTooltip.Divider);
    }
}
