using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace WcfWSChatForWeb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WSChatService : IWSChatService
    {
        public WSChatService()
        {
            Debug.WriteLine("ChatService instantiated");
        }
        public async Task SendMessageToServer(Message msg)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IWSChatCallback>();
            if (msg.IsEmpty || ((IChannel)callback).State != CommunicationState.Opened)
            {
                return;
            }

            byte[] body = msg.GetBody<byte[]>();
            string msgTextFromClient = Encoding.UTF8.GetString(body);

            string msgTextToClient = string.Format(
                "Got message {0} at {1}",
                msgTextFromClient,
                DateTime.Now.ToLongTimeString());
            var timer = new Timer();
            timer.Interval = 4000;
            timer.Elapsed += (o, e) => {
                callback.SendMessageToClient(
                    CreateMessage(msgTextToClient));
                timer.Stop();
            };
            timer.Start();
            
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private Message CreateMessage(string msgText)
        {
            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(msgText)));
            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty 
                { MessageType = WebSocketMessageType.Text };
            return msg;
        }
    }
}
