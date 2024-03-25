namespace Features.Connection.UI
{
    public class BoolButtonRotationElement : BaseButtonRotationElement
    {
        public bool IsActive { get; private set; }
        
        protected override void ActivateInternal(ButtonRotationManager buttonRotationManager)
        {
            IsActive = true;
        }

        protected override void DeactivateInternal(ButtonRotationManager buttonRotationManager)
        {
            IsActive = false;
        }
    }
}
