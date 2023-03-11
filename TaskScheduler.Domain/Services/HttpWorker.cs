using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler.Domain.Services.Interfaces;

namespace TaskScheduler.Domain.Services
{
    public class HttpWorker : IWorker
    {
        private ILogger logger;
        private IRepository service;

        public HttpWorker(ILogger _logger, IRepository _service)
        {
            logger = _logger;
            service = _service;
        } 


        public Task Do(TASK task)
        {
            return Task.Run(() => {

                using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
                {
                    try
                    {
                        HttpResponseMessage response = client.GetAsync(task.JOB).Result;
                        response.EnsureSuccessStatusCode();
                        string responseBody = response.Content.ReadAsStringAsync().Result;
                        // Above three lines can be replaced with new helper method below
                        // string responseBody = await client.GetStringAsync(uri);
                        dynamic obj = JsonConvert.DeserializeObject(responseBody);
                        if (obj.message == "ОК")
                        {
                            logger.Info(task.NAME + "|" + task.JOB + " Выполнено!");
                        }
                        else {
                            logger.Error(task.NAME + "|" + task.JOB + " " + obj.message);
                        }
                       

                    }
                    catch (Exception ex)
                    {

                        logger.Error(task.NAME + "|" + task.JOB +" "+ex.Message);
                    }
                    
                }
                
            });
        }
    }
}
