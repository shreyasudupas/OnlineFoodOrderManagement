// Populate values from your OpenAI deployment
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using System.Text;
using Inventory.Mongo.Persistance;
using MenuManagement.AI.Core.Functions;
using MongoDb.Shared.Persistance;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable SKEXP0010,SKEXP0070
//var builder = WebApplication.CreateBuilder(args);

var env = "Development";
var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                    .AddJsonFile($"appsettings.{env}.json", optional: true)
                                    .AddEnvironmentVariables()
                                    .Build();

//var modelId = "mistral";
//var endpoint = new Uri("http://localhost:11434");


// Create a kernel with Azure OpenAI chat completion
var kernelBuilder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(modelId: "phi4",
        endpoint: new Uri("http://localhost:1234/v1/"),
        apiKey: "lm-studio");

    //.AddOllamaChatCompletion(modelId, endpoint);

// Add enterprise components
kernelBuilder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Error));
kernelBuilder.Services.AddInventoryMongoInfratructure(configuration);
kernelBuilder.Services.AddSharedMongoServices(configuration);

kernelBuilder.Plugins.AddFromType<VendorMenuPlugin>(nameof(VendorMenuPlugin));

// Build the kernel
Kernel kernel = kernelBuilder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

var prompt = @"You are a Menu Management Assistant, designed to help users navigate registered restaurants. 
    Use the VendorMenuPlugin for restaurant-related tasks, including:
    1. Retrieving the list of restaurants
    2. Displaying menus based on restaurant ID
    3. Adding menu items to the cart

    Rules:
    1. Only add menu items that are available within the selected restaurant.
    2. If a user tries to add unavailable items, provide a warning.
    3. Do not display sensitive information (e.g., restaurant ID, menu ID, user ID).
    4. Keep responses short and precise.
    5. Do not answer any questions unrelated to menu management.";

//OllamaPromptExecutionSettings ollamaPromptExecutionSettings = new()
//{
//    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
//};

OpenAIPromptExecutionSettings settings = new()
{
    Temperature = 0.1,
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

ChatHistory chatHistory = [];
chatHistory.AddSystemMessage(prompt);

while (true)
{
    Console.Write("You: ");
    var userMessage = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(userMessage))
    {
        break;
    }
    chatHistory.AddUserMessage(userMessage);

    //var response = await kernel.InvokePromptAsync(userMessage, new KernelArguments(settings));
    var response = chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory,
        executionSettings: settings,
        kernel: kernel);

    StringBuilder responseBuilder = new();
    Console.Write("Bot: ");

    await foreach(var chunck in response)
    {
        Console.Write(chunck.Content);
        responseBuilder.AppendLine(chunck.Content);
    }

    chatHistory.AddAssistantMessage(responseBuilder.ToString());

    Console.WriteLine("\n");

}