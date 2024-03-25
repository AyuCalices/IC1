using UnityEngine;
using VENTUS.DataStructures.Variables;

namespace Features.CharacterCard.Scripts
{
    [CreateAssetMenu]
    public class BookDataVariable : AbstractVariable<BookData>
    {
        protected override BookData SetStoredDefault()
        {
            return null;
        }
    }
}
