using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Video_Blog_Writer
{
    internal class AgentState
    {
        public string VideoFilePath { get; set; } = string.Empty;
        public string BlogTitle { get; set; } = string.Empty;
        public string TranscribedVideoText { get; set; } = string.Empty;

        public string BlogContent { get; set; } = string.Empty;


        // Ensure Messages is initialized to avoid null reference issues
        public List<ChatMessage> Messages { get; set; } = [];
    }
}
