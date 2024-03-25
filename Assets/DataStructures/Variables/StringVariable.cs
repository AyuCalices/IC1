using UnityEngine;
using VENTUS.DataStructures.Variables;

namespace DataStructures.Variables
{
    [CreateAssetMenu]
    public class StringVariable : AbstractVariable<string>
    {
        protected override string SetStoredDefault()
        {
            return string.IsNullOrEmpty(storedValue) ? string.Empty : storedValue;
        }
    }
}
