using AzureFuncs.ForexBigDataProcessingFunctions.Functions.Activities;

namespace AzureFuncs.ForexBigDataProcessingFunctions.Functions.Orchestrators;
public static class ForexBigDataProcessingOrchestrator
{
    /// <summary>
    /// This function is the entry point for the orchestrator function
    /// It is responsible for calling the activities in the correct order and passing the required data between them
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>

    [Function(nameof(ForexBigDataProcessingOrchestratorFunctionBeginMapReduce))]
    public static async Task<bool> ForexBigDataProcessingOrchestratorFunctionBeginMapReduce(
    [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(ForexBigDataProcessingOrchestratorFunctionBeginMapReduce));
        try
        {
            logger.LogInformation("Starting ForexBigDataProcessingOrchestratorFunctionBeginMapReduce");

            var input = context.GetInput<ForexContainerModel>() ?? throw new ArgumentNullException(@"required parameter 'container' and 'subContainer'  not found", nameof(ForexContainerModel));

            var container = input.ContainerName ?? throw new ArgumentNullException(@"required parameter 'container'", nameof(ForexContainerModel.SubContainerName));

            var subContainer = input.SubContainerName ?? throw new ArgumentNullException(@"required parameter 'container'", nameof(ForexContainerModel.SubContainerName)); ;

            LogInformation(context, logger, "getting files names", "searching files");

            var fileNames = await context.CallActivityAsync<List<string>>(nameof(ForexBigDataProcessingFileActivity.GetFileListAsync), input);

            LogInformation(context, logger, "file(s) found", "Creating mappers");

            var readFileTask = new List<Task<ForexExchangeRateModel>>();

            foreach (var fileName in fileNames)
            {
                var forexContainerBlobModel = new ForexContainerBlobModel(container, fileName);

                readFileTask.Add(context.CallActivityAsync<ForexExchangeRateModel>(nameof(ForexBigDataProcessingFileActivity.ReadFileAsync), forexContainerBlobModel));
            }

            var readFileResults = await Task.WhenAll(readFileTask);

            var yearlyAverageExchangeRateTask = new List<Task<ForexYearlyAverageExchangeRate>>();

            foreach (var forexExchangeRateModel in readFileResults)
            {
                yearlyAverageExchangeRateTask.Add(context.CallActivityAsync<ForexYearlyAverageExchangeRate>(nameof(ForexBigDataProcessingMapperActivity.YearlyAverageExchangeRateMapperAsync), forexExchangeRateModel));
            }

            var yearlyAverageExchangeRateResults = await Task.WhenAll(yearlyAverageExchangeRateTask);

            var reducerResult = await context.CallActivityAsync<string>(nameof(ForexBigDataProcessingReducerActivity.Reducer), yearlyAverageExchangeRateResults.ToList());

            var finalResult = await context.CallActivityAsync<bool>(nameof(ForexBigDataProcessingFileActivity.WriteToBlob), reducerResult);

            return finalResult;

            static void LogInformation(TaskOrchestrationContext context, ILogger logger, string message, string status)
            {
                if (!context.IsReplaying)
                {
                    logger.LogInformation(message);

                    context.SetCustomStatus(new { status });
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ForexBigDataProcessingOrchestratorFunctionBeginMapReduce");
            throw;
        }
    }
}
