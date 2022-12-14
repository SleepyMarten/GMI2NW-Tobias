using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tobias.App.Models;

namespace Tobias.App.Services
{
    public class ServerDb : IDataStore<Item>
    {
        #region Interface Properties
        #region Server Information

        //public string ServerProtocol => "https";
        public string ServerProtocol => "http";
        public string ServerName => "localhost";
        //public string ServerPort => "44357";
        public string ServerPort => "38660";
        public string TypeOfItem => "Donor";
        public string ServerRootUrl => $"{ServerProtocol}://{ServerName}:{ServerPort}/";
        public string ServerRootUrlOfItem => $"{ServerRootUrl}{TypeOfItem}";
        #endregion
        #endregion

        List<Item> m_items = new List<Item>();

        #region Constructor(s)
        public ServerDb()
        {

        }
        #endregion

        #region Interface CRUD operations
        public async Task<bool> AddItemAsync(Item item)
        {
            //TODO: should update on server, currently only locally

            m_items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            //TODO: should update on server, currently only locally

            var oldItem = m_items.Where((Item arg) => arg.Guid == item.Guid).FirstOrDefault();
            m_items.Remove(oldItem);
            m_items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string guid)
        {
            //TODO: should update on server, currently only locally

            var oldItem = m_items.Where((Item arg) => arg.Guid == guid).FirstOrDefault();
            m_items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string guid)
        {
            FetchItemsFromServer();
            return await Task.FromResult(m_items.FirstOrDefault(s => s.Guid == guid));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            FetchItemsFromServer();
            return await Task.FromResult(m_items);
        }

        #region Internal CRUD Methods
        private void FetchItemsFromServer()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(ServerRootUrlOfItem);
            httpWebRequest.Method = "GET";

            WebResponse response = httpWebRequest.GetResponse();

            string responseString;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            m_items = JsonConvert.DeserializeObject<List<Item>>(responseString);
        }
        #endregion
        #endregion
    }
}