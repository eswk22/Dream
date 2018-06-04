using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAL.ElasticRepository
{
    public interface IElasticRepository<T> where T : class
    {
        #region "index"
        /// <summary>
        /// Index the searchDoc synchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>        
        /// <returns></returns>
        string Index(T document) ;

        /// <summary>
        /// Index the searchDoc asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>        
        /// <returns></returns>
        void IndexAsync(T document) ;

        /// <summary>
        /// Batch Indexing the searchDocs synchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents"></param>        
        /// <returns></returns>
        bool Index(IList<T> documents) ;

        /// <summary>
        /// Batch Indexing the searchDocs asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents"></param>        
        /// <returns></returns>
        void IndexAsync(IList<T> documents) ;

        #endregion

        #region "Get"
        /// <summary>
        /// Load a searchDoc by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>        
        /// <param name="documentType"></param>
        /// <returns></returns>
        T GetById(string id, string documentType = "") ;

        /// <summary>
        /// This is a MultiGet based on the multiple IDs passed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>        
        /// <param name="documentType"></param>
        /// <returns></returns>
        IList<T> GetMany(IEnumerable<string> ids, string documentType = "") ;

        /// <summary>
        /// This is lucene term query. Searching for a term on all fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="skip">-1 here means take all</param>
        /// <param name="take"></param>
        /// <returns></returns>
        IList<T> Search(string term, int skip = 0, int take = -1) ;

        #endregion

        #region "Update"

        /// <summary>
        /// Update a searchDoc
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        bool Update(T document) ;

        #endregion

        #region "Delete"

        bool Delete(string id) ;

        void DeleteAsync(string id) ;

        //bool Delete(IList<string> ids);

        //void DeleteAsync(IList<string> ids) ;

        #endregion

        #region "Index Level ops"

        bool CreateIndex(string index, IIndexState settings);

        bool DeleteIndex(string index = "");

        #endregion
    }
}

