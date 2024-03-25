namespace Features.Connection.UI
{
    public class BoolButtonRotationElement : BaseButtonRotationElement
    {
        public bool IsActive { get; private set; }
        
        protected override void ActivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            IsActive = true;
        }

        protected override void DeactivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            IsActive = false;
        }
    }
}
