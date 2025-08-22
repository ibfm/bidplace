using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BidPlace.ClientApp.Messages;

public class ProductCountChangedMessage(int count) : ValueChangedMessage<int>(count);
