using System;

public interface IMessageReceiver<Type> where Type : Enum
{
    void ReceiveMessage(Type message);
}
