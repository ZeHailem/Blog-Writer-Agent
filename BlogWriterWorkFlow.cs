// See https://aka.ms/new-console-template for more information
using Microsoft.Agents.AI.Workflows;
using Video_Blog_Writer;

internal class BlogWriterWorkFlow
{
    static VideoDownLoaderExcutor videoDownLoaderExcutor = new();
    static VideoTranscriberExecutor videoTranscriberExecutor = new();
    static BlogTitleExcutor blogTitleExcutor = new();
    static BlogExcutor blogExcutor = new();

    public BlogWriterWorkFlow()
    {
        Workflow = new WorkflowBuilder(videoDownLoaderExcutor) // Pass the required 'start' parameter
            .AddEdge(videoDownLoaderExcutor, videoTranscriberExecutor)
            .AddEdge(videoTranscriberExecutor, blogTitleExcutor)
            .AddEdge(blogTitleExcutor, blogExcutor)       // Join to wait for both inputs
            .WithOutputFrom(blogExcutor)
            .Build();
    }

    public static Workflow Workflow { get; set; }


    public async Task ExcuteWorkFlow(string videoURL)
    {
        await using Run run = await InProcessExecution.RunAsync(Workflow, videoURL);

        foreach (WorkflowEvent evt in run.NewEvents)
        {
            switch (evt)
            {
                case ExecutorCompletedEvent executorComplete:
                    Console.WriteLine($"{executorComplete.ExecutorId}: {executorComplete.Data}");
                    break;
                case WorkflowOutputEvent workflowOutput:
                    Console.WriteLine($"Workflow '{workflowOutput.SourceId}' outputs: {workflowOutput.Data}");
                    break;
            }
        }
    }
}