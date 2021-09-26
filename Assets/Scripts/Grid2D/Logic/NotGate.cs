public class NotGate : LogicGate
{
    public void UpdateOutgoingPlugValues(GridComponentPlug[] incomingPlugs, GridComponentPlug[] outgoingPlugs)
    {
        if (incomingPlugs[0].GetValue() == 0)
        {
            outgoingPlugs[0].SetValue(1);
        }
        else
        {
            outgoingPlugs[0].SetValue(0);
        }
    }
}
