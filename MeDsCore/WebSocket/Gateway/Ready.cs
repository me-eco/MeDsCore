using MeDsCore.WebSocket.Gateway.Entities;

namespace MeDsCore.WebSocket.Gateway;

public class Ready
{
    public Ready(ReadyEntity readyEntity)
    {
        SessionId = readyEntity.SessionId;
        Application = new Application(readyEntity.Application);
    }
    
    public string SessionId { get; }
    public Application Application { get; }
}

public class Application
{
    public Application(ApplicationEntity applicationEntity)
    {
        Id = ulong.Parse(applicationEntity.Id);
    }
    
    public ulong Id { get; }
}