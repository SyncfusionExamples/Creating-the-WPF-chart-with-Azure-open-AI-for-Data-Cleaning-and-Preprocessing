using Azure.AI.OpenAI;
using Azure;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DataCleaning_Preprocessing.Service
{
    internal class AzureOpenAIService
    {
        const string endpoint = "https://your_end_point.openai.azure.com";
        const string deploymentName = "GPT35Turbo";

        string key = "";

        public AzureOpenAIService(string key)
        {
            this.key = key;
        }

        public async Task<ObservableCollection<WebsiteTrafficData>> GetCleanedData(ObservableCollection<WebsiteTrafficData> rawData)
        {
            ObservableCollection<WebsiteTrafficData> collection = new ObservableCollection<WebsiteTrafficData>();

            var chatCompletionsOptions = new ChatCompletionsOptions
            {
                DeploymentName = deploymentName,
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };


            var prompt = $"Clean the following e-commerce website traffic data, resolve outliers and fill missing values:\n{string.Join("\n", rawData.Select(d => $"{d.DateTime:yyyy-MM-dd-HH-m-ss}: {d.Visitors}"))} and the output cleaned data should be in the yyyy-MM-dd-HH-m-ss:Value, not required explanations";
            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));
            try
            {
                var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
                var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
                return GetCleanedData(response.Value.Choices[0].Message.Content, collection);
            }
            catch (Exception ex)
            {
                return GetDummyData(collection);
            }
        }

        ObservableCollection<WebsiteTrafficData> GetCleanedData(string json, ObservableCollection<WebsiteTrafficData> collection)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new ObservableCollection<WebsiteTrafficData>();
            }

            var lines = json.Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    var date = DateTime.ParseExact(parts[0].Trim(), "yyyy-MM-dd-HH-m-ss", CultureInfo.InvariantCulture);
                    var high = double.Parse(parts[1].Trim());

                    collection.Add(new WebsiteTrafficData { DateTime = date, Visitors = high });
                }
            }

            return collection;
        }

        private ObservableCollection<WebsiteTrafficData> GetDummyData(ObservableCollection<WebsiteTrafficData> collection)
        {
            return new ObservableCollection<WebsiteTrafficData>() {
              new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 00, 00, 00), Visitors = 150 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 01, 00, 00), Visitors = 160 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 02, 00, 00), Visitors = 155 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 03, 00, 00), Visitors = 162 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 04, 00, 00), Visitors = 170 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 05, 00, 00), Visitors = 175 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 06, 00, 00), Visitors = 145 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 07, 00, 00), Visitors = 180 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 08, 00, 00), Visitors = 190 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 09, 00, 00), Visitors = 185 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 10, 00, 00), Visitors = 200 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 11, 00, 00), Visitors = 207 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 12, 00, 00), Visitors = 220 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 13, 00, 00), Visitors = 230 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 14, 00, 00), Visitors = 237 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 15, 00, 00), Visitors = 250 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 16, 00, 00), Visitors = 260 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 17, 00, 00), Visitors = 270 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 18, 00, 00), Visitors = 277 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 19, 00, 00), Visitors = 280 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 20, 00, 00), Visitors = 290 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 21, 00, 00), Visitors = 300 },
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 22, 00, 00), Visitors = 307 }, // Missing data
                new WebsiteTrafficData { DateTime = new DateTime(2024, 07, 01, 23, 00, 00), Visitors = 320 },
                };
        }
    }
}
