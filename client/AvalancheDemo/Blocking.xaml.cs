using RestSharp;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace AvalancheDemo
{
    /// <summary>
    /// Blocking.xaml 的交互逻辑
    /// </summary>
    public partial class Blocking : Window
    {
        private Task<string> TS;
        private int sid;

        public Blocking(Task<string> ts, int sid)
        {
            InitializeComponent();
            this.Show();
            TS = ts;
            this.sid = sid;
            readHttp();
        }

        private async void readHttp()
        {
            string response = await TS;
            textBox.Text = "Text Recognizing on Cloud ...";
            Timer t = new Timer(5000) { Enabled = true };
            t.Elapsed += (object sender, ElapsedEventArgs e) => {
                this.Dispatcher.Invoke(() =>
               {
                   IRestResponse fetch = fetchResult();
                   if (fetch.StatusCode == System.Net.HttpStatusCode.OK)
                   {
                       DisplayResult displaywindow = new DisplayResult(fetch.Content);
                       displaywindow.Show();
                       this.Close();
                       t.Enabled = false;
                   }
               });
            };
        }
        private IRestResponse fetchResult()
        {
            var client = new RestClient("http://serverip/hackathon/api/v1/session/" + sid);
            var request = new RestRequest(Method.GET);
            return client.Execute(request);
        }
    }
}
