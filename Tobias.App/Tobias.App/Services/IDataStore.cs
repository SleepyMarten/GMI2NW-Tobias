using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tobias.App.Services
{
    public interface IDataStore<T>
    {
        #region Interface Properties
        #region Server Information
        //string ServerProtocol => "http";
        string ServerProtocol { get; }
        string ServerName { get; }
        //string ServerPort => "44357";
        string ServerPort { get;  }
        string ServerRootUrl { get; }
        string ServerRootUrlOfItem { get; }
        #endregion
        #endregion

        #region Interface CRUD Methods
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        #endregion
    }
}
