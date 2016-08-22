using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.IO;
using Bot_Application1.Models;
using System.Web;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        const string luiskey = "your luis key";
        const string luisappid = "your luis app id";
        string luisurl = "https://api.projectoxford.ai/luis/v1/application?id=" + luisappid + "&subscription-key=" + luiskey;

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                string answer = string.Empty;

                #region Send to LUIS

                WebRequest request = WebRequest.Create(luisurl + "&q=" + HttpUtility.UrlEncode(activity.Text));
                HttpWebResponse luisres = (HttpWebResponse)request.GetResponse();
                Stream datastream = luisres.GetResponseStream();
                StreamReader reader = new StreamReader(datastream);
                string resjson = reader.ReadToEnd();

                LuisResult luisresdata = JsonConvert.DeserializeObject<LuisResult>(resjson);


                /*
                 * 由LUIS分析回來的結果
                 * 會依分數高低排序，通常我們取第一個就是分數最高的
                 * 做為使用者意圖分類的判定，並撰寫相關後續要回應的邏輯
                 */
                if (luisresdata.intents.Count > 0)
                {
                    var intentresult = luisresdata.intents[0];

                    switch (intentresult.intent)
                    {
                        case "詢問課程活動":
                            answer = "您好，9月份課程:版控的概念與實務(http://www.accupass.com/go/vcconpra)";
                            break;
                        case "None":
                            answer = "不明白您的意思";
                            break;
                    }
                }

                reader.Close();
                datastream.Close();
                luisres.Close();

                #endregion


                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //// calculate something for us to return
                //int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                Activity reply = activity.CreateReply(answer);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}