using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base;
using MeDsCore.Interactions.Base.Entities;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;

namespace MeDsCore.Interactions.ApplicationCommands;

public class ApplicationCommandSocket
{
    private readonly IMethodExecutor _executor;
    internal InteractionBase<InteractionApplicationCommandResponse> InteractionResponse { get; }
    public IReadOnlyCollection<ApplicationArgument> Args { get; }

    public ApplicationArgument? this[string name]
    {
        get
        {
            foreach (var arg in Args)
            {
                if (arg.Name == name)
                {
                    return arg;
                }
            }

            return null;
        }
    }

    internal ApplicationCommandSocket(IMethodExecutor executor, InteractionBase<InteractionApplicationCommandResponse> interactionResponse)
    {
        _executor = executor;
        InteractionResponse = interactionResponse;

        var args = new ApplicationArgument[interactionResponse.Data.Options.Length];
        
        for (var i = 0; i < args.Length; i++)
        {
            args[i] = new ApplicationArgument(interactionResponse.Data.Options[i], interactionResponse.Data.ResolvedData, executor);
        }

        Args = args;
    }

    public async Task ResponseMessageInteractionAsync(string content, bool tts = false)
    {
        var responseInteractionMethodInfo =
            InteractionsMethods.ConfigureResponseInteractionMethodInfo(InteractionResponse.Id,
                InteractionResponse.Token);
        var interactionResponseInst = new InteractionCreateResponse(InteractionCallbackType.ChannelMessageWithSource,
            new InteractionResponseMessage(content, tts));
        var jsonContent = new JsonContentBuilder(interactionResponseInst);
        var result = await _executor.ExecuteMethodAsync(responseInteractionMethodInfo, jsonContent);

        if (!result.IsSuccess) throw new DiscordException("Failed to response to the interaction");
    }
    
    private class InteractionResponseMessage 
    {
        public InteractionResponseMessage(string content, bool tts)
        {
            Tts = tts;
            Content = content;
        }

        [JsonPropertyName("tts")]
        public bool Tts { get; }
        [JsonPropertyName("content")]
        public string Content { get; }
    }

    private class InteractionCreateResponse
    {
        public InteractionCreateResponse(InteractionCallbackType type, object data)
        {
            Type = (int)type;
            Data = data;
        }

        [JsonPropertyName("type")]
        public int Type { get; }
        [JsonPropertyName("data")]
        public object Data { get; }
    }
}