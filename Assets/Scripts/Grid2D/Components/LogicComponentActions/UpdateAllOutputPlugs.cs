using System;
public class UpdateAllOutputPlugs : LogicComponentAction
{
    public Action<int> onValueChange(LogicComponent comp)
    {
        return (int value) =>
        {
            foreach (GridComponentPlug plug in comp.GetOutgoingPlugs())
            {
                plug.SetValue(value);
            }
        };
    }
}
