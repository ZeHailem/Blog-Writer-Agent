using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Reflection;
using System;
using System.IO;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace Video_Blog_Writer
{
    internal sealed class VideoDownLoaderExcutor() : ReflectingExecutor<VideoDownLoaderExcutor>("VideoDownLoaderExcutor"),
    IMessageHandler<string, string>
    {

        // Downloaded video will be saved to this pat.
        public static string VideoFilePath =>  Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
        public static async Task<string?> DownloadAsync(string url, bool onlyAudio, string outputDir)
        {
            try
            {
                // ✅ Ensure output directory exists
                Directory.CreateDirectory(VideoFilePath);

                var youtube = new YoutubeClient();
                var video = await youtube.Videos.GetAsync(url);
                var title = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

                // ✅ Select best stream
                IStreamInfo? streamInfo = onlyAudio
                    ? streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate()
                    : streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                if (streamInfo == null)
                {
                    Console.WriteLine("❌ No stream found.");
                    return null;
                }

                // ✅ Build file path
                var fileExtension = onlyAudio ? "mp3" : streamInfo.Container.Name;
                var outputPath = Path.Combine(outputDir, $"{title}.{fileExtension}");

                Console.WriteLine($"⬇️ Downloading: {video.Title}");

                // ✅ Download file
                if (onlyAudio)
                {
                    // Convert to MP3 (requires FFmpeg in PATH)
                    await youtube.Videos.DownloadAsync(url, outputPath);
                }
                else
                {
                    await youtube.Videos.Streams.DownloadAsync(streamInfo, outputPath);
                }

                Console.WriteLine($"✅ Download complete: {outputPath}");

                return outputPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error downloading video: {ex.Message}");
                return null;
            }
        }

        public ValueTask<string> HandleAsync(string message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(DownloadAsync(message, true, VideoFilePath).Result ?? string.Empty);
        }
    }


}
