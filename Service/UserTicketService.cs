using DomainModel.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Repository.Interface;
using Service.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Service
{
    public class UserTicketService : IUserTicketService
    {
        private readonly IUserTicketRepository _utRepository;
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "EventWeb";
        static string SpreadsheetId = "1pxc1hAYsxDIVzWn959FUZs2m3fwxaUuGfX4SMRCCjQM";
        static SheetsService service;
        public UserTicketService(IUserTicketRepository utRepository)
        {
            _utRepository = utRepository;
        }
        public void AddUserTicket(UserTicket userTicket)
        {            
            _utRepository.Add(userTicket);
            ConnectToSheet();
            TicketToSheet(userTicket);
        }

        public void DeleteTicket(UserTicket userTicket)
        {
            _utRepository.Delete(userTicket);
        }

        public void EditUserTicket(UserTicket userTicket)
        {
            _utRepository.Edit(userTicket);
        }

        public UserTicket GetUserTicketById(int id)
        {
            return _utRepository.GetUserTicketById(id);
        }

        public IEnumerable<UserTicket> GetUserTicketList()
        {
            return _utRepository.UserTicketList();
        }

        public IEnumerable<UserTicket> GetUserTicketListByType(int id)
        {
            return _utRepository.UserTicketListByType(id);
        }        
        private void TicketToSheet(UserTicket userTicket)
        {
            string sheet = "TicketSheet";
            var range = $"{sheet}!A:E";
            var valueRange = new ValueRange();
            var fullname = userTicket.User.LastName + " " + userTicket.User.FirstName;
            // Data for another Student...
            var oblist = new List<object>() { userTicket.Ticket.TicketName, fullname, userTicket.QRCode, userTicket.ScanDate, userTicket.IsScanned };
            valueRange.Values = new List<IList<object>> { oblist };
            // Append the above record...
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }
        private void ConnectToSheet()
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
