using System.Collections;

namespace Features.Core.UI.Scripts.TransitionView
{
    public interface ICustomTransitionView
    {
        public void Enable();
        public IEnumerator EnableCoroutine();
        public void Disable(DeactivationType deactivationType);
        public IEnumerator DisableCoroutine(DeactivationType deactivationType);
    }
}
