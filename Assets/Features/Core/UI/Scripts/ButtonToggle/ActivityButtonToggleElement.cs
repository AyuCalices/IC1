using UnityEngine;

namespace Features.Connection.UI
{
    public class ActivityButtonToggleElement : BaseButtonToggleElement
    {
        protected override void ActivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            gameObject.SetActive(true);
        }

        protected override void DeactivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            gameObject.SetActive(false);
        }
    }
}
