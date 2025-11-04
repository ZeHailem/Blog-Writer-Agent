using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using OpenAI.Chat;
using System.ClientModel;

namespace Video_Blog_Writer
{
    internal sealed class BlogExcutor() : ReflectingExecutor<BlogExcutor>("BlogExcutor"),
    IMessageHandler<string, string>
    {
        internal static async Task<string> WriteBlogAsync(string transcribedText, string title)
        {
            var userPrompt = $"As a blog post writer, create a one paragraph blog post based on the following transcribed text: {transcribedText} using title: {title}";

            ClientResult<ChatCompletion> result = await LLMClient.ChatClient.CompleteChatAsync(userPrompt);
            var assistantResponse = result.Value.Content.FirstOrDefault()?.Text;

            // Return the content of the assistant's message or an empty string if null
            return assistantResponse ?? string.Empty;
        }

        public async ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var blog = await WriteBlogAsync(message ,AgentState.TranscribedVideoText);
            return blog ?? string.Empty;
        }
    }
}

             