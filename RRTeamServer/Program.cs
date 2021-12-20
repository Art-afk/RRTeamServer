using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RRTeamServer
{
    class RRTeamServer
    {
        public const string API_KEY = "AIzaSyAtXutAkd2BiVo6EMUUp5bWRvPomUwDXsc";

        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    Console.WriteLine("Введите текст который хотите отправить или нажмите кнопку ESC:");
                    var a = Console.ReadLine();
                    sendermessage(a);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

           
        }
       static void sendermessage(string MESSAGE) 
        {

            
            var jGcmData = new JObject();
            var jData = new JObject();

            jData.Add("message", MESSAGE);
            jGcmData.Add("to", "/topics/global");
            jGcmData.Add("data", jData);

            var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + API_KEY);

                    Task.WaitAll(client.PostAsync(url,
                        new StringContent(jGcmData.ToString(), Encoding.Default, "application/json"))
                            .ContinueWith(response =>
                            {
                                Console.WriteLine(response);
                                Console.WriteLine("Message sent: check the client device notification tray.");
                            }));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to send GCM message:");
                Console.Error.WriteLine(e.StackTrace);
            }
        
        }
    }
}