using System;
using System.Threading.Tasks;
using ConoHaDNS.ViewModel;
using Sharprompt;

namespace ConoHaDNS
{
    internal class Program
    {
        private static int _status;

        private static async Task Main(string[] args)
        {
            Api.GetToken();

            while (true)
            {
                switch (_status)
                {
                    case 0: _status = await DomainsControls.SelectDomain(); break;
                    case 1: _status = DomainsControls.SelectControl(); break;
                    case 2: _status = await DomainsControls.CreateDomain(); break;
                    case 3: _status = await DomainsControls.DeleteDomain(); break;
                    case 4: _status = RecordsControls.SelectType(); break;
                    case 5: _status = await RecordsControls.SelectRecord(); break;
                    case 6: _status = RecordsControls.SelectControl(); break;
                    case 7: _status = await RecordsControls.ViewRecord(); break;
                    case 8: _status = await RecordsControls.CreateRecord(); break;
                    case 9: _status = await RecordsControls.UpdateRecord(); break;
                    case 10: _status = await RecordsControls.DeleteRecord(); break;
                    case 99: _status = Exit(); break;
                }
            }
        }

        private static int Exit()
        {
            if (Prompt.Confirm("Do you want to exit ?")) Environment.Exit(0);
            return 0;
        }
    }
}
