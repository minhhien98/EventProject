using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.HangoutsChat;
using Microsoft.Extensions.Logging;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace Service
{
    public class BackgroundTaskService : IBackGroundTaskService
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "EventWeb";
        static string SpreadsheetId = "1pxc1hAYsxDIVzWn959FUZs2m3fwxaUuGfX4SMRCCjQM";
        static SheetsService service;
        private readonly ILogger<BackgroundTaskService> _logger;
        public BackgroundTaskService(ILogger<BackgroundTaskService> logger)
        {
            _logger = logger;
        }
        public async Task CheckIn(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ConnectToGG();
                IList<IList<object>> List;
                IList<IList<object>> CheckInTicket;
                //check all CheckIn{i} Sheet   
                for (int CheckinPos = 1; CheckinPos <= 2; CheckinPos++)
                {
                    var requestCheckIn = service.Spreadsheets.Values.Get(SpreadsheetId, $"CheckIn{CheckinPos}!A2");
                    try
                    {
                        var responseCheckIn = requestCheckIn.Execute();
                        CheckInTicket = responseCheckIn.Values;
                    }
                    catch (Exception ex)
                    {
                        CheckInTicket = null;
                    }
                    if (CheckInTicket != null)
                    {
                        int pos = 2;
                        var request = service.Spreadsheets.Values.Get(SpreadsheetId, "TicketSheet!A2:E");
                        var response = request.Execute();
                        List = response.Values;
                        //split checkinTicket for qr string
                        var obj = CheckInTicket[0];
                        var split = obj[0].ToString().Split("\n", StringSplitOptions.None);
                        if (split.Length < 3)
                        {
                            //fake/invalid ticket
                            DeleteCheckInTicket(CheckinPos);
                            //var message = "Can't scan.";
                            //SendBotMessage(CheckinPos, message);
                            continue;
                        }
                        var qr = split[2];
                        bool isFake = true;
                        foreach (var item in List)
                        {
                            if (item.Count > 4)
                            {
                                if (item[2].ToString() == qr)
                                {
                                    if (item[4].ToString() == "FALSE")
                                    {
                                        UpdateTicket(pos);
                                        DeleteCheckInTicket(CheckinPos);
                                        //var message = item[0].ToString() + " " + item[1].ToString();
                                        //SendBotMessage(CheckinPos, message);
                                        isFake = false;
                                        break;
                                    }
                                    else
                                    {
                                        //scanned ticket
                                        DeleteCheckInTicket(CheckinPos);
                                        //var message = "\'text\': \'Scanned Ticket.\'";
                                        //var message = "Scanned Ticket.";
                                        //SendBotMessage(CheckinPos, message);
                                        isFake = false;
                                        break;
                                    }
                                }
                            }
                            pos++;
                        }
                        if (isFake)
                        {
                            //Fake Ticket: Invalid Qr...
                            DeleteCheckInTicket(CheckinPos);
                            //var message = "Fake Ticket.";
                            //SendBotMessage(CheckinPos, message);
                            continue;
                        }
                    }                   
                }
                await Task.Delay(1000 * 30);
            }
        }
        //Google Chat Room Webhook for CheckIn result
        /*private static string[] WebHookUrl =
        {
            "https://chat.googleapis.com/v1/spaces/AAAAgHoFATM/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=bP2aTmMjYkPhEqZveD69sH0OPvm5suaxbbrZpUPhfQo%3D", //Room 1
            "https://chat.googleapis.com/v1/spaces/AAAAQowsiN0/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=Na4OzikKG5g874vZtcf893Ys8EKFFpJQuoOUvUA8FsY%3D",
        };*/
        //send message to google chat room
        /*private void SendBotMessage(int CheckinPos, string message)
        {
        var url = WebHookUrl[CheckinPos -1];

            HttpClient client = new HttpClient();
            var values = new Dictionary<string, string>()
            {
                {"text",message }
            };
            var json = JsonConvert.SerializeObject(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = client.PostAsync(new Uri(url),content);

        }*/
        private void UpdateTicket(int pos)
        {
            var valuerange = new ValueRange();
            var oblist = new List<object>() { DateTime.Now, true };
            valuerange.Values = new List<IList<object>> { oblist };
            var update = service.Spreadsheets.Values.Update(valuerange, SpreadsheetId, $"TicketSheet!D{pos}:E");
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateresponse = update.Execute();
        }
        private void DeleteCheckInTicket(int CheckinPos)
        {
            var requestBody = new ClearValuesRequest();
            var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, SpreadsheetId, $"CheckIn{CheckinPos}!A2:E");
            var deleteResponse = deleteRequest.Execute();
        }
        private void ConnectToGG()
        {
            GoogleCredential credential;

            using (var stream =
                new FileStream("EventClient.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
            .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}



