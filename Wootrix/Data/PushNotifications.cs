using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace WootrixV2.Data
{
    public class PushNotifications
    {
        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");

        //The ServerKey under for the WootrixV2Apps app (https://console.firebase.google.com/u/2/project/wootrixv2apps/settings/cloudmessaging/android:com.ta.wootrix)
        private static string ServerKey = "AAAAzoKc4Sk:APA91bF61qVegrEJ8vffmza9xev6Jtmc-X5xcaDieAHiUxldzEJiLuARH2-97mrHoX_1I9xzGZLSdbkqXhPEwlmLGmkyxiwrTFEwpbqKsQ2mCBOQCRrjq09LFpp_g9AUVqwZg2D7ENGb";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        public static async Task<bool> SendPushNotification(string[] deviceTokens, string title, string body, object data)
        {
            bool sent = false;

            if (deviceTokens.Count() > 0)
            {
                //Object creation

                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = title,
                        text = body
                    },
                    data = data,
                    registration_ids = deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                /*
                 ------ JSON STRUCTURE ------
                 {
                    notification: {
                                    title: "",
                                    text: ""
                                    },
                    data: {
                            action: "Play",
                            playerId: 5
                            },
                    registration_ids = ["id1", "id2"]
                 }
                 ------ JSON STRUCTURE ------
                 */

                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);

                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                    sent = sent && result.IsSuccessStatusCode;
                }
            }

            return sent;
        }

    }

    public class Message
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; }
        public object data { get; set; }
    }
    public class Notification
    {
        public string title { get; set; }
        public string text { get; set; }
    }
}


public static class PushNotificationLogic
{
    

}