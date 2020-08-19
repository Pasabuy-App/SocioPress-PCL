using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using SocioPress.Profile.Struck;

namespace SocioPress.Post
{
    public class CountAll
    {
        #region Fields
        /// <summary>
        /// Instance of Count All Post by User ID Class.
        /// </summary>
        private static CountAll instance;
        public static CountAll Instance
        {
            get
            {
                if (instance == null)
                    instance = new CountAll();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public CountAll()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void ByUser(string wp_id, string session_key, string user_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("user_id", user_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/post/user/count", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(result);

                bool success = token.status == "success" ? true : false;
                string data = token.status == "success" ? result : token.message;
                callback(success, data);
            }
            else
            {
                callback(false, "Network Error! Check your connection.");
            }
        }
        #endregion
    }
}
