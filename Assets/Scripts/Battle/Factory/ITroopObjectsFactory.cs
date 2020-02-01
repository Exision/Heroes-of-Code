using System.Collections.Generic;

public interface ITroopObjectsFactory
{
    TroopObject[] Create(List<Troop> troops);
}
