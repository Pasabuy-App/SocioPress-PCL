﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocioPress.Model;
using System.Net.Http;

namespace SocioPress
{
    public class Feeds
    {
        #region Fields
        /// <summary>
        /// Instance of Feeds Class with home and profile method.
        /// </summary>
        private static Feeds instance;
        public static Feeds Instance
        {
            get
            {
                if (instance == null)
                    instance = new Feeds();
                return instance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Feeds()
        {
            client = new HttpClient();
        }
        #endregion

        #region Home Method
        public async void Home(string wp_id, string session_key, string last_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                if (last_id != "") { dict.Add("lid", last_id); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/feeds/home", content);
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

        #region Profile Method
        public async void Profile(string wp_id, string session_key, string last_id, string user_id, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                if (last_id != "") { dict.Add("lid", last_id); }
                if (user_id != "") { dict.Add("user_id", user_id); }
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(SPHost.Instance.BaseDomain + "/sociopress/v1/feeds/profile", content);
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