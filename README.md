# Video to Blog Post Generator

This console application automates the process of creating a blog post from a YouTube video. It downloads the audio from a given YouTube URL, transcribes it, generates a title, and then writes a blog post based on the transcribed content.

## Features

- **YouTube Video Downloading**: Downloads the audio from any public YouTube video.
- **Audio Transcription**: Uses a local Whisper.net model to transcribe the audio into text.
- **AI-Powered Content Generation**: Leverages the OpenAI API (GPT-4) to generate a relevant blog title and a one-paragraph blog post.
- **Automated Workflow**: Utilizes the `Microsoft.Agents.AI.Workflows` framework to orchestrate the entire process seamlessly.

## Prerequisites

Before you can run this project, you need to have the following installed and configured:

1.  **.NET 9 SDK**: Ensure you have the .NET 9 SDK or a later version installed.
2.  **FFmpeg**: The application uses FFmpeg to process audio. You must [install FFmpeg](https://ffmpeg.org/download.html) and ensure it is available in your system's PATH.
3.  **OpenAI API Key**: The project requires an OpenAI API key to generate the title and blog content.
    -   Set your API key as an environment variable named `OPENAI_API_KEY`.
4.  **Whisper Model**: A pre-trained `Whisper.net` model is required for transcription.
    -   Download the `ggml-base.bin` model. A common source is the [Hugging Face repository](https://huggingface.co/ggerganov/whisper.cpp/blob/main/ggml-base.en.bin).
    -   Create a `model` folder in the root of the project directory.
    -   Place the downloaded `ggml-base.bin` file inside the `model` folder.

## Installation

1.  Clone the repository:
    ```sh
    git clone <repository-url>
    ```
2.  Navigate to the project directory:
    ```sh
    cd Video-Blog-Writer
    ```
3.  Restore the .NET dependencies:
    ```sh
    dotnet restore
    ```

## Usage

1.  Open the `Program.cs` file.
2.  Change the YouTube URL in the following line to the video you want to process:
    ```csharp
    workflow.ExcuteWorkFlow("https://www.youtube.com/watch?v=JT6YT93gRo8").GetAwaiter().GetResult();
    ```
3.  Run the application from your terminal:
    ```sh
    dotnet run
    ```

The application will then execute the workflow, and you will see the output of each step printed to the console, culminating in the final blog post.

## Workflow Overview

The application's logic is orchestrated in `BlogWriterWorkFlow.cs` and follows these steps:

1.  **VideoDownLoaderExcutor**: Downloads the audio from the specified YouTube URL.
2.  **VideoTranscriberExecutor**: Transcribes the downloaded audio file into text.
3.  **BlogTitleExcutor**: Sends the transcribed text to the OpenAI API to generate a suitable blog post title.
4.  **BlogExcutor**: Takes both the transcribed text and the generated title and prompts the OpenAI API to write a one-paragraph blog post.

## Key Dependencies

-   `Microsoft.Agents.AI.Workflows`
-   `YoutubeExplode`
-   `Whisper.net`
-   `OpenAI`
