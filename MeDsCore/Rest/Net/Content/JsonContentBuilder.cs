using System.Net.Http.Json;

namespace MeDsCore.Rest.Net.Content;

internal class JsonContentBuilder : IContentBuilder
{
    private readonly object _target;

    public JsonContentBuilder(object target)
    {
        _target = target;
    }
    
    public void CreateContent(HttpRequestMessage requestMessage)
    {
        var jsonContent = JsonContent.Create(_target, _target.GetType());
        
        requestMessage.Content = jsonContent;
    }
}