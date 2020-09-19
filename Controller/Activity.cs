using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocioPress.Model;
using System.Net.Http;

namespace SocioPress
{
    public class Activity
    {
        #region Fields
        /// <summary>
        /// Instance of Activity Class with insert, list and open method.
        /// </summary>
        private static Activity instance;
        public static Activity Instance
        {
            get
            {
                if (instance == null)
                    instance = new Activity();
                return instance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Activity()
        {
            client = new HttpClient();
        }
        #endregion

        #region Insert Method
        public async void Insert(string wp_id, string session_key, string stid, string title, string info, string icon, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                if (stid != "") { dict.Add("stid", stid); }
                dict.Add("title", title);
                dict.Add("info", info);
                dict.Add("icon", icon);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/activity/insert", content);
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

        #region List Method
        public async void List(string wp_id, string session_key, string stid, string icon, string open, string lid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                if (stid != "") { dict.Add("stid", stid); }
                if (icon != "") { dict.Add("icon", icon); }
                if (open != "") { dict.Add("open", open); }
                if (lid != "") { dict.Add("lid", lid); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/activity/list/all", content);
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

        #region Open Method
        public async void Open(string wp_id, string session_key, string atid, string stid, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("atid", atid);
                if (stid != "") { dict.Add("stid", stid); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/activity/select", content);
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

        #region MarkAllRead Method
        public async void MarkAll(string wp_id, string session_key, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/activity/markall/read", content);
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
