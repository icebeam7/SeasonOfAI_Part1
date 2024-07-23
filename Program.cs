#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Converting null literal or possible null value to non-nullable type.

using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using OpenAI.Images;

string key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
string endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_ENDPOINT");
string model = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_DEPLOYMENT");

AzureOpenAIClient azureClient = new(
    new Uri(endpoint),
    new AzureKeyCredential(key));

Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.White;   
Console.WriteLine("Example 1 - Chat Completions");

ChatClient chatClient = azureClient.GetChatClient(model);

ChatCompletionOptions options = new ChatCompletionOptions()
{
    Temperature = 0.7f,
    MaxTokens = 50
};

List<ChatMessage> messages = [
    new SystemChatMessage("You are a helpful assistant that talks like a pirate."),
    new UserChatMessage("Hi, can you help me?"),
    new AssistantChatMessage("Arrr! Of course, me hearty! What can I do for ye?"),
    new UserChatMessage("What's the best way to train a parrot?"),
];

ChatCompletion completion = chatClient.CompleteChat(messages, options);

Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

Console.ReadLine();
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Green;

Console.WriteLine("\nExample 2 - Image Generation");

ImageClient imageClient = azureClient.GetImageClient("Dalle3");
ImageGenerationOptions igOptions = new ImageGenerationOptions()
{
    Quality = GeneratedImageQuality.Standard,
    ResponseFormat = GeneratedImageFormat.Uri,
    Size = GeneratedImageSize.W1024xH1024,
    Style = GeneratedImageStyle.Vivid,
};
string imagePrompt = "An image of a couple sharing an umbrella on a quaint park bench amidst falling rain.";

GeneratedImage image = imageClient.GenerateImage(imagePrompt, igOptions);
Console.WriteLine(image.ImageUri);

Console.ReadLine();
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\nExample 3 - Prompt Engineering");

Console.WriteLine("Zero-shot learning:");
List<ChatMessage> messagesZ = [
    new UserChatMessage("Good morning! => "),
];

ChatCompletion completionZ = chatClient.CompleteChat(messagesZ);
Console.WriteLine($"{completionZ.Role}: {completionZ.Content[0].Text}");

Console.WriteLine("One-shot learning:");
List<ChatMessage> messagesO = [
    new UserChatMessage("Good morning! => "),
    new AssistantChatMessage("¡Buenos días!"),
    new UserChatMessage("Hello world! => "),
];

ChatCompletion completionO = chatClient.CompleteChat(messagesO);
Console.WriteLine($"{completionO.Role}: {completionO.Content[0].Text}");

Console.WriteLine("Few-shot learning:");
List<ChatMessage> messagesF = [
    new UserChatMessage("Good morning! => "),
    new AssistantChatMessage("¡Buenos días!"),
    new UserChatMessage("Hello world! => "),
    new AssistantChatMessage("¡Hola mundo!"),
    new UserChatMessage("This is awesome => "),
    new AssistantChatMessage("Esto es sorprendente"),
    new UserChatMessage("Yesterday, I watched a movie => "),
    new AssistantChatMessage("Ayer vi una película"),
    new UserChatMessage("What's up? => "),
];

ChatCompletion completionF = chatClient.CompleteChat(messagesF);
Console.WriteLine($"{completionF.Role}: {completionF.Content[0].Text}");

Console.ForegroundColor = ConsoleColor.White;
