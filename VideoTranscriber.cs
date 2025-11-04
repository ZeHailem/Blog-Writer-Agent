using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using System.Diagnostics;
using Whisper.net;
namespace Video_Blog_Writer
{
    internal class VideoTranscriberExecutor() : ReflectingExecutor<VideoTranscriberExecutor>("VideoTranscriberExecutor"), IMessageHandler<string, string>
    {
        public static async Task<string?> TranscribeVideoAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("❌ File not found.");
                    return null;
                }
                // This model is dowloaded fom huggingface and saved in model folder.
                string modelPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\model\ggml-base.bin"));

                // ✅ Download or load Whisper model from local path
                using var whisperFactory = WhisperFactory.FromPath(modelPath);

                // ✅ Create processor (for audio/video)
                using var processor = whisperFactory.CreateBuilder()
                    .WithLanguage("en") // change language if needed
                    .Build();

                string wavFile = await ConvertToWavAsync(filePath);

                Console.WriteLine($"🎙️ Transcribing: {Path.GetFileName(filePath)}");

                string resultText = string.Empty;

                await foreach (var segment in processor.ProcessAsync(File.OpenRead(wavFile)))
                {
                    resultText += segment.Text + " ";
                }

                // ✅ Clean up file after transcription
                File.Delete(filePath);
                Console.WriteLine($"🗑️ Deleted: {filePath}");
                return resultText.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during transcription: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> ConvertToWavAsync(string inputPath)
        {
            string outputPath = Path.ChangeExtension(inputPath, ".wav");

            var psi = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-y -i \"{inputPath}\" -ar 16000 -ac 1 -c:a pcm_s16le \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi);
            await process.WaitForExitAsync();

            if (File.Exists(outputPath))
                return outputPath;

            throw new Exception("FFmpeg failed to convert the file to WAV.");
        }

        public async ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            var result = await TranscribeVideoAsync(message);
            return result ?? string.Empty;
        }

    }
}
