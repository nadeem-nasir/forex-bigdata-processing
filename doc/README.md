# Title
Azure Durable Functions in the .NET Isolated Worker to compute the average exchange rate for each currency and year using MapReduce and Fan-in and Fan-out
# Introduction 
- Explore how to implement the MapReduce programming model on top of Azure Durable Functions in the .NET isolated worker and use Fan-in and Fan-out. 
- Forex exchange rate dataset as an example to demonstrate how to use MapReduce with Azure Durable Functions. 
- Goal is to compute the average exchange rate for each currency and year using MapReduce.
- The dataset contains the daily exchange rates of EUR against the US dollar. 
- Sample input data is in SampleData folder
# Preqrequisites
- Azure subscription
- Visual Studio 2022 or later 
- .NET 8.0
- Azure Functions Core Tools v4.x
- Azure development workload in Visual Studio
- Azure storage account
- Upload sample data to Azure Blob Storage in 'forex/exchangeratedata' container from SampleData folder in the repository
# Azure resources used in the project
- Azure Function App in .NET 8.0, isolated worker process  Linux OS, Durable Functions
- Azure hosting plan
- Azure Confugurations in Bicep template
- Azure Storage
- Azure blob storage
- Create a container using bicep template and blob service 
- Azure Storage account role assignment in Bicep template
# To run the application locally
- Clone the repository
- Open the solution in Visual Studio 2022 or later
- Rename template.local.settings.json to local.settings.json
- Update the connection string in local.settings.json or in appsettings.json
- Build the solutio and wait to restore packages
- api/ForexBigDataProcessingOrchestratorFunction_HttpStart?path=forex&subPath=exchangeratedata
- Bicep template deploy all the required resources and also set the required configurations. so it is easy to deploy the infrastructure and test the application
# Deploying Azure Function Apps using Azure DevOps
- Azure subscription
- Deploy infrastructure using Azure Bicep temnplate from infra folder
- Create a new project in Azure DevOps or GitHub
- Create a new service connection in Azure DevOps or GitHub
- Create variables group in Azure DevOps or GitHub
- Create a new pipeline in Azure DevOps or GitHub using the pipelines.yaml file from deploy folder
- Push the code to the repository
# Learning Outcomes of the Case Study 
- Understand the MapReduce programming model and its benefits for processing large-scale data sets in parallel and distributed manner.
- Understand the Azure Durable Functions and how they enable stateful and long-running workflows in a serverless environment.
- Understand the .NET isolated worker and how it allows you to run .NET functions in a separate process from the host runtime.
- Understand the Azure Functions Core Tools and how they help you to develop, test, and deploy Azure Functions locally and remotely.
- Understand the Azure storage account and how it provides scalable and secure cloud storage for various types of data.
- Understand the Azure DevOps and how it supports continuous integration and continuous delivery for Azure Functions.
- Understand the Azure Bicep and how it simplifies the creation and deployment of Azure resources using a declarative language.
- Understand the Azure Functions triggers and bindings and how they enable your functions to respond to events and interact with other services.
- Understand the Azure Functions input and output bindings and how they allow you to read and write data from various sources and destinations.
- Fan-in and Fan-out: This is a common pattern for orchestrating multiple function instances in parallel. The case study uses it to fan out the map tasks to multiple ProcessingMapper functions and fan in the results to a single ReducerActivity function.
- Http and timer trigger: These are two types of triggers that can invoke Azure Functions. The case study uses an Http trigger to start the workflow and a timer trigger to schedule it periodically.
- Integration with blob storage: This is a service that provides scalable and secure cloud storage for various types of data. The case study uses it to store the input and output files of the MapReduce process.
- .NET 8: This is the latest version of the .NET framework that supports cross-platform development and performance improvements. The case study uses it to leverage the new features such as global usings, record types, and pattern matching.
- Orchestrator: This is a special type of Azure Durable Function that coordinates the execution of other functions and maintains the state of the workflow. The case study uses it to orchestrate the MapReduce process and invoke the ProcessingFileActivity, ProcessingMapper, and ReducerActivity functions.
- ProcessingFileActivity: This is an activity function that reads the input file from blob storage and splits it into smaller chunks for the map tasks.
- ProcessingMapper: This is an activity function that performs the map phase of the MapReduce process.  It then groups the records and calculates the average exchange rates for each group.
# Various industry use cases of the case study
- Data analysis 
- Data mining
- Machine learning
- Data transformation
# Extend the Case Study: Todo For Learning 
- Deploy the infrastructure using Azure Bicep template
- Change the AzureWebJobsStorage in local.settings.json to use the Azure Storage connection string
- Upload sample data to Azure Blob Storage in 'forex/exchangeratedata' container from SampleData folder in the repository
- Run the application locally
- Create a pipeline in Azure DevOps using the pipelines.yaml file from deploy folder
- Deploy the Azure Function App using Azure DevOps
- Test the deployed Azure Function App in the Azure portal
- Currently it upload result to blob storage in forex-uploaded-data container. You can extend it and write a blob trigger function that listens to the forex-uploaded-data container that notify users that result is available.
- Write a time trigger function that runs one a day and delete the uploaded data from forex-uploaded-data container
- Write a http trigger function that uploads files from SampleData folder to 'forex/exchangeratedata' container.
	
