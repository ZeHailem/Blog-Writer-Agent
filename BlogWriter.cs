using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Linq;
using System.Threading.Tasks;

namespace Video_Blog_Writer
{
    internal sealed class BlogExcutor() : ReflectingExecutor<BlogExcutor>("BlogExcutor"),
    IMessageHandler<Tuple<string, string>, string>
    {
        internal static async Task<string> WriteBlogAsync(string transcribedText, string title)
        {
            var userPrompt = $"As a blog post writer, create a one paragraph blog post based on the following transcribed text: {transcribedText} using title: {title}";

            ClientResult<ChatCompletion> result = await LLMClient.ChatClient.CompleteChatAsync(userPrompt);
            var assistantResponse = result.Value.Content.FirstOrDefault()?.Text;

            // Return the content of the assistant's message or an empty string if null
            return assistantResponse ?? string.Empty;
        }

        public async ValueTask<string> HandleAsync(Tuple<string, string> message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            // The framework provides the outputs from parent nodes as a tuple.
            // Item1 will be from the first edge added (videoTranscriberExecutor).
            // Item2 will be from the second edge added (blogTitleExcutor).
            var transcribedText = message.Item1;
            var title = message.Item2;

            // Ensure the retrieved values are not null

            var blog = await WriteBlogAsync(transcribedText, title);
            return blog ?? string.Empty;
        }
    }
}

             