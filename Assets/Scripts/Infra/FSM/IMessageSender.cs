using System;

public interface IMessageSender<Type> where Type : Enum
{
    void AddReceiver(IMessageReceiver<Type> receiver);
    void RemoveReceiver(IMessageReceiver<Type> receiver);
}