using Core.Utilities.EmailHandler.Models;

namespace Core.Utilities.EmailHandler.Abstract;

public interface IEmailService
{
    void SendMessage(Message message);
}
