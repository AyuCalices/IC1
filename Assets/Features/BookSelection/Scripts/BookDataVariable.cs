using Features._Core.DataStructures.Variables;
using UnityEngine;

namespace Features.BookSelection.Scripts
{
    [CreateAssetMenu]
    public class BookDataVariable : AbstractVariable<BookData>
    {
        protected override BookData SetStoredDefault()
        {
            return null;
        }

        private void OnDisable()
        {
            Restore();
        }
    }
}
