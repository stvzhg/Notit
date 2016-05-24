using System.Windows;
using AgoraRtcEngineControlLib;
using System.IO;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace AvalancheDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private AgoraRtcEngineControl m_engine = null;
        private StreamWriter log;

        public string UserName { get; set; }

        private bool m_joined = false;

        private List<uint> userList = new List<uint>();
        private List<PictureBox> hostList = new List<PictureBox>();

        private uint myUID = 0;

        public MainWindow()
        {
            UserName = "Guest";
            InitializeComponent();
            setupRtcEngine();
            log = new StreamWriter(Environment.GetEnvironmentVariable("TEMP") + "\\" + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + "_MainWindow.log");
            hostList.Add(pictureBox11);
            hostList.Add(pictureBox12);
            hostList.Add(pictureBox21);
            hostList.Add(pictureBox22);
        }

        private void setupRtcEngine()
        {
            m_engine = new AgoraRtcEngineControl();
            m_engine.initialize("Vendor Key");

            m_engine.enableVideo();
            m_engine.setupLocalVideo(videoLocal.Handle, RenderMode.RenderMode_Hidden);

            m_engine.onJoinChannelSuccess += new _IAgoraRtcEngineControlEvents_onJoinChannelSuccessEventHandler(this.onJoinChannelSuccess);
            m_engine.onRejoinChannelSuccess += new _IAgoraRtcEngineControlEvents_onRejoinChannelSuccessEventHandler(this.onRejoinChannelSuccess);
            m_engine.onApiCallExecuted += new _IAgoraRtcEngineControlEvents_onApiCallExecutedEventHandler(this.onApiCallExecuted);
            m_engine.onLeaveChannel += new _IAgoraRtcEngineControlEvents_onLeaveChannelEventHandler(this.onLeaveChannel);
            m_engine.onUserJoined += new _IAgoraRtcEngineControlEvents_onUserJoinedEventHandler(this.onUserJoined);
            m_engine.onUserOffline += new _IAgoraRtcEngineControlEvents_onUserOfflineEventHandler(this.onUserOffline);
        }

        private void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            string channel = "", user = "";
            JoinChannelWindow subWindow = new JoinChannelWindow((C, U) => { channel = C; user = U; });
            subWindow.ShowDialog();
            if (channel == "" && user == "")
                return;
            if (m_engine.joinChannel("", channel + "_" + user, "", 0) == 0)
            {
                m_joined = true;
                SwitchAudioRecording();
            }
        }

        private void onJoinChannelSuccess(string channel, uint uid, int elapsed)
        {
            log.WriteLine(string.Format("joined channel '{0}' uid {1} elapsed {2}\n", channel, uid, elapsed));
            this.Dispatcher.Invoke(() =>
            {
                myUID = uid;
                channelUserListBox.Items.Add(myUID);
            });
        }

        private void onRejoinChannelSuccess(String channel, uint uid, int elapsed)
        {
            log.WriteLine(string.Format("rejoined channel '{0}' uid {1} elapsed {2}\n", channel, uid, elapsed));
        }

        private void onLeaveChannel(uint duration, uint txBytes, uint rxBytes)
        {
            log.WriteLine(string.Format("left channel: duration {0} seconds tx/rx bytes {1}/{2}\n", duration, txBytes, rxBytes));
        }

        private void onApiCallExecuted(String api, int result)
        {
            log.WriteLine(string.Format("onApiCallExecuted: '{0}' returns {1}\n", api, result));
        }

        private void onUserJoined(uint uid, int elapsed)
        {
            this.Dispatcher.Invoke(()=>
            {
                int currentUserCount = userList.Count;
                userList.Add(uid);
                channelUserListBox.Items.Add(uid);
                m_engine.setupRemoteVideo(uid, hostList[currentUserCount].Handle, RenderMode.RenderMode_Hidden);
            });
        }

        private void onUserOffline(uint uid, UserOfflineReason reason)
        {
            this.Dispatcher.Invoke(() =>
            {
                int offlineUser = userList.FindIndex((x) => x == uid);
                m_engine.setupRemoteVideo(uid, new IntPtr(0), RenderMode.RenderMode_Hidden);
            });
        }

        private void onFirstRemoteVideoDecoded(uint uid, int width, int height, int elapsed)
        {
            this.Dispatcher.Invoke(()=>
            {
                m_engine.setupRemoteVideo(uid, hostList[0].Handle, RenderMode.RenderMode_Hidden);
            });
        }

        private void runOnUIThread(MethodInvoker method)
        {
            this.Dispatcher.Invoke(method);
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        bool isRecording = false;
        int recordTimeStamp = 0;

        public void SwitchAudioRecording()
        {
            if (!isRecording)
            {
                record("open new Type waveaudio Alias recsound", "", 0, 0);
                record("record recsound", "", 0, 0);
                isRecording = true;
            }
            else
            {
                recordTimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string s = Environment.GetEnvironmentVariable("TEMP");
                string filePath = s + "\\" + recordTimeStamp + ".wav";
                record("save recsound " + filePath, "", 0, 0);
                record("close recsound", "", 0, 0);
                isRecording = false;

                string sid = m_engine.getCallId();
                UploadAudio(filePath, myUID, sid == null ? 15515 : sid.GetHashCode());
            }
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_joined)
            {
                m_joined = false;
                m_engine.leaveChannel();
                foreach (var box in hostList)
                    box.Image = null;
                videoLocal.Image = null;
                SwitchAudioRecording();
            }
        }

        private void CheckBox_Checked(object sender, EventArgs e)
        {
            if (((System.Windows.Controls.CheckBox)sender).IsChecked == true)
            {
                m_engine.enableVideo();
                m_engine.startPreview();
                videoCell.Visibility = Visibility.Visible;
                SwitchAudioRecording();
            }
            else if (((System.Windows.Controls.CheckBox)sender).IsChecked == false)
            {
                m_engine.disableVideo();
                m_engine.stopPreview();
                videoCell.Visibility = Visibility.Collapsed;
                SwitchAudioRecording();
            }
        }

        private Task<HttpWebRequest> HttpInternalWrite(string url, string file, string paramName, string contentType,
            NameValueCollection nvc)
        {
            return Task.Run(() =>
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = CredentialCache.DefaultCredentials;

                Stream rs = wr.GetRequestStream();
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, paramName, file, contentType);
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                return wr;
            });
        }
        public async Task<string> HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            //log.Debug(string.Format("Uploading {0} to {1}", file, url));
            HttpWebRequest wr = await HttpInternalWrite(url, file, paramName, contentType, nvc);

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd());
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
        }

        private void UploadAudio(string filePath, uint uid, int sid)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("uid", uid.ToString());
            nvc.Add("mid", sid.ToString());
            Task<string> response = HttpUploadFile("http://serverip/hackathon/api/v1/audio",
                 filePath, "file", "sound/wav", nvc);
            Blocking bl = new Blocking(response, sid);
        }

    }
}
