using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace IdentityServiceApiTester
{
    class Program
    {
        // Load base URL from appsettings.json
        private static readonly string AppSettingsPath = "appsettings.json";
        private static readonly string BaseUrl = LoadBaseUrl();

        static async Task Main(string[] args)
        {
            Console.WriteLine("IdentityServiceApiTester - .NET 8 Console App");
            Console.WriteLine($"Base URL: {BaseUrl}");
            Console.WriteLine("\nReplace sample data with real test values before running against production!");

            using var client = new HttpClient();

            // 1. Login (expecting 2FA required)
            var loginData = new
            {
                username = "sampleuser",
                password = "samplepassword",
                roleId = "sampleRoleId"
            };
            await SendRequest(client, "/login", HttpMethod.Post, loginData);

            // 2. Send SMS code
            var smsCodeData = new
            {
                userId = "sampleUserId"
            };
            await SendRequest(client, "/sendSMScode", HttpMethod.Post, smsCodeData);

            // 3. Verify SMS code
            var verifyCodeData = new
            {
                userId = "sampleUserId",
                code = "123456"
            };
            await SendRequest(client, "/verifyCode", HttpMethod.Post, verifyCodeData);

            // 4. Forgot password
            var forgotPasswordData = new
            {
                username = "sampleuser"
            };
            await SendRequest(client, "/forgotPassword", HttpMethod.Post, forgotPasswordData);

            // 5. Set new password
            var setNewPasswordData = new
            {
                userId = "sampleUserId",
                token = "sampleResetToken",
                newPassword = "newSamplePassword"
            };
            await SendRequest(client, "/setNewPassword", HttpMethod.Put, setNewPasswordData);
        }

        private static async Task SendRequest(HttpClient client, string endpoint, HttpMethod method, object data)
        {
            var url = BaseUrl.TrimEnd('/') + endpoint;
            Console.WriteLine($"\nRequest: {method} {url}");
            Console.WriteLine($"Payload: {JsonConvert.SerializeObject(data, Formatting.Indented)}");

            HttpResponseMessage response;
            if (method == HttpMethod.Post)
            {
                response = await client.PostAsJsonAsync(url, data);
            }
            else if (method == HttpMethod.Put)
            {
                response = await client.PutAsJsonAsync(url, data);
            }
            else
            {
                throw new NotSupportedException("Only POST and PUT are supported in this tester.");
            }

            Console.WriteLine($"Response Status: {(int)response.StatusCode} {response.StatusCode}");
            var responseBody = await response.Content.ReadAsStringAsync();
            PrintJson(responseBody);
        }

        private static void PrintJson(string json)
        {
            try
            {
                var parsed = JToken.Parse(json);
                Console.WriteLine("Response Body:");
                Console.WriteLine(JsonConvert.SerializeObject(parsed, Formatting.Indented));
            }
            catch
            {
                Console.WriteLine("Response Body:");
                Console.WriteLine(json);
            }
        }

        private static string LoadBaseUrl()
        {
            try
            {
                var json = File.ReadAllText(AppSettingsPath);
                var obj = JObject.Parse(json);
                return obj["BaseUrl"]?.ToString() ?? "https://your-identity-server-url/api/v1/account";
            }
            catch
            {
                return "https://your-identity-server-url/api/v1/account";
            }
        }
    }
}
