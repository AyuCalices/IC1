namespace Features.Connection.UI
{
    public class ActivityButtonRotationElement : BaseButtonRotationElement
    {
        protected override void ActivateInternal(ButtonRotationManager buttonRotationManager)
        {
            gameObject.SetActive(true);
        }

        protected override void DeactivateInternal(ButtonRotationManager buttonRotationManager)
        {
            gameObject.SetActive(false);
        }
    }
}
