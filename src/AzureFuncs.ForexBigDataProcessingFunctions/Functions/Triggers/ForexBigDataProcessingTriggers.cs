using System.Net;

namespace AzureFuncs.ForexBigDataProcessingFunctions.Functions.Orchestrators;
public static class ForexBigDataProcessingTriggers
{

    /// <summary>
    /// This function is the entry point for the orchestrator function
    /// This funcation can be triggered by an HTTP request using the query string parameters as shown in the example below
    /// http://localhost:7287/api/ForexBigDataProcessingOrchestratorFunction_HttpStart?path=forex&subPath=exchangeratedata
    /// Change the port number to the one used by the local Azure Functions runtime in your environment
    /// Container and subContainer are created in azure bicep file. check the bicep file for more details
    /// </summary>
    /// <param name="req"></param>
    /// <param name="client"></param>
    /// <param name="executionContext"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [Function(nameof(ForexBigDataProcessingOrchestratorFunction_HttpStart))]
    public static async Task<HttpResponseData> ForexBigDataProcessingOrchestratorFunction_HttpStart(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
    [DurableClient] DurableTaskClient client,
    FunctionContext executionContext)
    {
        // Use descriptive name for the instance id
        string orchestrationId = string.Empty;
        ILogger logger = executionContext.GetLogger(nameof(ForexBigDataProcessingOrchestratorFunction_HttpStart));
        // try-finally block to ensure the instance is purged regardless of the outcome
        try
        {
            // Use the query parameters dictionary to access the values
            var container = req.Query["path"] ?? throw new ArgumentNullException(@"required query string parameter 'path' not found", "path"); ;
            var subContainer = req.Query["subPath"] ?? throw new ArgumentNullException(@"required query string parameter 'subPath' not found", "subPath");

            // Function input comes from the request content.
            orchestrationId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(ForexBigDataProcessingOrchestrator.ForexBigDataProcessingOrchestratorFunctionBeginMapReduce), new ForexContainerModel(container, subContainer));

            logger.LogInformation("Started orchestration with ID = '{orchestrationId}'.", orchestrationId);

            // Return the check status response without purging the instance
            return client.CreateCheckStatusResponse(req, orchestrationId);
        }
        catch (Exception ex)
        {
            // Log the exception details
            logger.LogError(ex, "An error occurred while starting the orchestration.");

            // Return a more informative error message
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
        finally
        {
            // Purge the instance in the finally block
            await client.PurgeInstanceAsync(orchestrationId);
        }
    }

    /// <summary>
    /// This function is the entry point for the orchestrator function
    /// Every 5 minutes, the function is triggered by a timer
    /// Container and subContainer are created in azure bicep file. check the bicep file for more details
    /// </summary>
    /// <param name="timerInfo"></param>
    /// <param name="client"></param>
    /// <param name="executionContext"></param>
    /// <returns></returns>
    [Function(nameof(ForexBigDataProcessingOrchestratorFunction_TimerStart))]
    [FixedDelayRetry(5, "00:00:10")]
    public static async Task<string> ForexBigDataProcessingOrchestratorFunction_TimerStart(
    [TimerTrigger("0 */5 * * * *", RunOnStartup = true),] TimerInfo timerInfo,
    [DurableClient] DurableTaskClient client,
    FunctionContext executionContext)
    {
        // more descriptive name for the instance id
        string orchestrationId = string.Empty;
        ILogger logger = executionContext.GetLogger(nameof(ForexBigDataProcessingOrchestratorFunction_TimerStart));
        // try-finally block to ensure the instance is purged regardless of the outcome
        try
        {
            //  constants for the container and subContainer values
            const string container = ForexContainerNames.ForexExchangeContainerName;
            const string subContainer = ForexContainerNames.ForexExchangeSubContainerName;
            // Function input comes from the request content.
            orchestrationId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(ForexBigDataProcessingOrchestrator.ForexBigDataProcessingOrchestratorFunctionBeginMapReduce), new ForexContainerModel(container, subContainer));

            logger.LogInformation("Started orchestration with ID = '{orchestrationId}'.", orchestrationId);

            // Return the instance id without purging the instance
            return orchestrationId;
        }
        catch (Exception ex)
        {
            // Log the exception details
            logger.LogError(ex, "An error occurred while starting the orchestration.");

            // Rethrow the exception to trigger the retry policy
            throw;
        }
        finally
        {
            // Purge the instance in the finally block
            await client.PurgeInstanceAsync(orchestrationId);
        }
    }
}
