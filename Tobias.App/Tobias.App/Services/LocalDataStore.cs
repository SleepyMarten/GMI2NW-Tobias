using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tobias.App.Models;

namespace Tobias.App.Services
{
    public class LocalDataStore : IDataStore<Item>
    {
        #region Interface Properties
        public string ServerProtocol => "[none]";
        public string ServerName => "[none]";
        public string ServerPort => "[none]";
        public string ServerRootUrl => "[none]";
        public string ServerRootUrlOfItem => "[none]";
        #endregion

        readonly List<Item> m_items;

        public LocalDataStore()
        {
            m_items = new List<Item>()
            {
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Pelle",  LastName="Persson",  SocialSecurityNumber="18880101", BloodGroupRh="Rh+", BloodGroupAB0="A" },
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Kalle",  LastName="Karlsson", SocialSecurityNumber="18890102", BloodGroupRh="Rh-", BloodGroupAB0="B" },
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Olle",   LastName="Olsson",   SocialSecurityNumber="18900103", BloodGroupRh="Rh-", BloodGroupAB0="AB" },
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Krille", LastName="Persson",  SocialSecurityNumber="18910104", BloodGroupRh="Rh+", BloodGroupAB0="0" },
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Grålle", LastName="Häst",     SocialSecurityNumber="18920105", BloodGroupRh="Rh+", BloodGroupAB0="A" },
                new Item { Guid = Guid.NewGuid().ToString(),  FirstName="Skalle", LastName="Skelett",  SocialSecurityNumber="18930106", BloodGroupRh="Rh-", BloodGroupAB0="0" }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            m_items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = m_items.Where((Item arg) => arg.Guid == item.Guid).FirstOrDefault();
            m_items.Remove(oldItem);
            m_items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string guid)
        {
            var oldItem = m_items.Where((Item arg) => arg.Guid == guid).FirstOrDefault();
            m_items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string guid)
        {
            return await Task.FromResult(m_items.FirstOrDefault(s => s.Guid == guid));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(m_items);
        }
    }
}