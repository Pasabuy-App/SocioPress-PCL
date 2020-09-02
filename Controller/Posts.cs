using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocioPress.Model;
using System.Net.Http;

namespace SocioPress
{
    public class Posts
    {
        #region Fields
        /// <summary>
        /// Instance of Post Class with insert, update, delete, getlink, sharelink and count post method.
        /// </summary>
        private static Posts instance;
        public static Posts Instance
        {
            get
            {
                if (instance == null)
                    instance = new Posts();
                return instance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Posts()
        {
            client = new HttpClient();
        }
        #endregion

        #region Insert Method
        public async void Insert(string wp_id, string session_key, string title, string contents, string type, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("title", title);
                dict.Add("content", contents);
                dict.Add("type", type);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/insert", content);
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

        #region Update Method
        public async void Update(string wp_id, string session_key, string title, string contents, string post_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("title", title);
                dict.Add("content", contents);
                dict.Add("post_id", post_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/update", content);
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

        #region Delete Method
        public async void Delete(string wp_id, string session_key, string post_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("post_id", post_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/delete", content);
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

        #region GetLink Method
        public async void GetLink(string wp_id, string session_key, string post_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("pid", post_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/share", content);
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

        #region ShareLink Method
        public async void ShareLink(string wp_id, string session_key, string title, string post_link, string type, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("title", title);
                dict.Add("post", post_link);
                dict.Add("type", type);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/share/insert", content);
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

        #region Count Method
        public async void Count(string wp_id, string session_key, string user_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("user_id", user_id);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/post/user/count", content);
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
