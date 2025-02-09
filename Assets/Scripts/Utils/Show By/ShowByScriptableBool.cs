public class ShowByScriptableBool : ShowByScriptableValue<ScriptableBool>
{
    protected override bool ShouldShow =>_value.Value;
}
