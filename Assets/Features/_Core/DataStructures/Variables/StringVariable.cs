using UnityEngine;

namespace Features._Core.DataStructures.Variables
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
