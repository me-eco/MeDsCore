namespace MeDsCore.Rest.Net.Content;

/// <summary>
/// Конфигурирует HTTP контент для конкретного запроса
/// </summary>
internal interface IContentBuilder
{
    /// <summary>
    /// Конфигурирует HTTP контент для конкретного запроса
    /// </summary>
    /// <param name="requestMessage"></param>
    void CreateContent(HttpRequestMessage requestMessage);

    public static IContentBuilder EmptyContent => new NoContentBuilder();
    public static IContentBuilder CreateJsonContentBuilder(object target) => new JsonContentBuilder(target);
}