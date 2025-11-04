using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using OpenAI.Chat;
using System.ClientModel;

namespace Video_Blog_Writer
{
    internal class BlogTitleExcutor() : ReflectingExecutor<BlogTitleExcutor>("BlogTitleExcutor"),
        IMessageHandler<string, string>
    {
        internal static async Task<string> GetTitleAsync(string transcribedText)
        {
            var prompt = $"If you are a blog post writer, what would be the title of this text: {transcribedText}";

            
            // Send the message and get the response
            ClientResult<ChatCompletion> result = await LLMClient.ChatClient.CompleteChatAsync(prompt);

            var assistantResponse = result.Value.Content.FirstOrDefault()?.Text;


            // Return the content of the assistant's message or an empty string if null
            return assistantResponse ?? string.Empty;
        }

        public async ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var title = await GetTitleAsync(message);
            return title ?? string.Empty;
        }

    }
}
