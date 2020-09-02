using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocioPress.Model;
using System.Net.Http;

namespace SocioPress
{
    public class Reviews
    {
        #region Fields
        /// <summary>
        /// Instance of Reviews Class with insert and get method.
        /// </summary>
        private static Reviews instance;
        public static Reviews Instance
        {
            get
            {
                if (instance == null)
                    instance = new Reviews();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Reviews()
        {
            client = new HttpClient();
        }
        #endregion
        #region Insert Method
        public async void Insert(string wp_id, string session_key, string comment, string mover_id, string rating, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("msg", comment);
                dict.Add("rid", mover_id);
                dict.Add("rat", rating);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/reviews/insert", content);
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

        #region Get Reviews Method
        public async void Get(string wp_id, string session_key, string user_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                if (user_id != "") { dict.Add("uid", user_id); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/reviews/user/list", content);
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
