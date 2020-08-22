using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using SocioPress.Profile.Struck;

namespace SocioPress.Post
{
    public class GetLink
    {
        #region Fields
        /// <summary>
        /// Instance of Get Link for Share Post Class.
        /// </summary>
        private static GetLink instance;
        public static GetLink Instance
        {
            get
            {
                if (instance == null)
                    instance = new GetLink();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public GetLink()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Share(string wp_id, string session_key, string post_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("pid", post_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/sociopress/v1/post/share", content);
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
