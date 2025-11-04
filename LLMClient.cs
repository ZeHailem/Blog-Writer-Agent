using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Blog_Writer
{
    internal class LLMClient
    {
        public static ChatClient ChatClient = new(model: "gpt-4", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }
}
