using System;

public interface LogicComponentAction
{
    Action<int> onValueChange(LogicComponent comp);
}
