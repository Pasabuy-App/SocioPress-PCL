using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using SocioPress.Profile.Struck;

namespace SocioPress.Post
{
    public class NewPost
    {
        #region Fields
        /// <summary>
        /// Instance of Insert New Post Class.
        /// </summary>
        private static NewPost instance;
        public static NewPost Instance
        {
            get
            {
                if (instance == null)
                    instance = new NewPost();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public NewPost()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Insert(string wp_id, string session_key, string title, string contents, string type,  Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("title", title);
            dict.Add("content", contents);
            dict.Add("type", type);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/post/insert", content);
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