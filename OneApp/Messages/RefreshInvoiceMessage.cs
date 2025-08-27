using CommunityToolkit.Mvvm.Messaging.Messages;

namespace OneApp.Messages;

public class RefreshInvoiceMessage : ValueChangedMessage<bool>
{
    public RefreshInvoiceMessage(bool value) : base(value)
    {
    }
}
