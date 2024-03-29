using DataStructures.Variables;
using UnityEngine;

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
