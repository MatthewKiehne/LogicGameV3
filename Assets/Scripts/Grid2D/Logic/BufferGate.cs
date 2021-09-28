public class BufferGate : LogicGate
{
    public void UpdateOutgoingPlugValues(GridComponentPlug[] incomingPlugs, GridComponentPlug[] outgoingPlugs)
    {
        outgoingPlugs[0].SetValue(incomingPlugs[0].GetValue());
    }
}
