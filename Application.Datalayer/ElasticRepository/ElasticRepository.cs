using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAL.ElasticRepository
{
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    {
      
        public string indexName;
        private readonly IElasticCore _elasticCore;

        public ElasticClient Client
        {
            get
            {
                return _elasticCore.Client;
            }
        }
        public ElasticRepository(IElasticCore elasticCore)
        {
            _elasticCore = elasticCore;
        }

        public IBulkResponse BulkInsert(IEnumerable<T> entities)
        {
            var request = new BulkDescriptor();

            foreach (var entity in entities)
            {
                request
                    .Index<T>(op => op
                        .Id(Guid.NewGuid().ToString())
                        .Index(indexName)
                        .Document(entity));
            }

            return Client.Bulk(request);
        }

        //public ICreateIndexResponse CreateIndex()
        //{
        //    var response = Client.IndexExists(IndexName);
        //    if (response.Exists)
        //    {
        //        return null;
        //    }
        //    return Client.CreateIndex(IndexName, index =>
        //        index.Mappings(ms =>
        //            ms.Map<TEntity>(x => x.AutoMap())));
        //}



        #region "SearchDoc ops"

        protected readonly int takeCount = 256;

        //public virtual bool Index(T document)
        //{
        //    var response = Client.Index<T>(document, c => c.OpType(Elasticsearch.Net.OpType.Create).Index(indexName));

        //    return response.Created;
        //}

        public virtual string Index(T document)
        {
            var response = Client.Index<T>(document, c => c.OpType(Elasticsearch.Net.OpType.Create).Index(indexName));

            return response.Id;
        }

        public virtual void IndexAsync(T document)
        {
            Client.IndexAsync<T>(document, c => c.OpType(Elasticsearch.Net.OpType.Create).Index(indexName));
        }

        public virtual bool Index(IList<T> documents)
        {
            var response = Client.IndexMany<T>(documents, indexName);

            return !response.Errors;
        }

        public virtual void IndexAsync(IList<T> documents)
        {
            Client.IndexManyAsync<T>(documents, indexName);
        }

        public virtual T GetById(string id, string documentType = null)
        {
            T response;
            DocumentPath<T> path = new DocumentPath<T>(id).Index(indexName);
            response = Client.Source<T>(path);
            return response;
        }

        public virtual IList<T> GetMany(IEnumerable<string> ids, string documentType = null)
        {
            IList<T> response;

            response = Client.SourceMany<T>(ids, indexName, documentType).ToList();

            return response;
        }

        /// <summary>
        /// Searching on all fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public virtual IList<T> Search(string term, int skip = 0, int take = -1)
        {
            if (take == -1) take = takeCount;

            var results = Client.Search<T>(s => s
                .Index(indexName)
                .From(skip)
                .Take(take)
                .Query(q => q.QueryString(qs => qs.Query(term))
                ));

            return results.Documents.ToList();
        }

        public virtual bool Update(T document)
        {
            var response = Client.Update<T>(
                new DocumentPath<T>(document), c => c.Doc(document).Index(indexName));

            return response.ServerError == null;
        }

        public virtual void UpdateAsync(T document)
        {
            Client.UpdateAsync<T>(
                new DocumentPath<T>(document), c => c.Doc(document).Index(indexName));
        }

        public virtual bool Update(IList<T> documents)
        {
            var descriptor = new BulkDescriptor().Index(indexName);
            foreach (var doc in documents)
            {
                descriptor.Update<T>(op => op
                    .IdFrom(doc)
                    .Doc(doc)
                );
            }

            var result = Client.Bulk(d => descriptor);

            return !result.Errors;
        }

        public virtual bool Delete(string id)
        {
            var response = Client.Delete<T>(id, d => d.Index(indexName));

            return response.Found;
        }

        public virtual void DeleteAsync(string id)
        {
            Client.DeleteAsync<T>(id, d => d.Index(indexName));
        }

        //public virtual bool Delete(IList<string> ids)
        //{
        //    var documents = new List<T>();

        //    foreach (var id in ids) documents.Add(new T
        //    {
        //        Id = id
        //    });

        //    var response = Client.DeleteMany<T>(documents, indexName.Value);

        //    return !response.Errors;
        //}

        //public virtual void DeleteAsync(IList<string> ids)
        //{
        //    var documents = new List<T>();

        //    foreach (var id in ids) documents.Add(new T { Id = id });

        //    Client.DeleteManyAsync<T>(documents, indexName.Value);
        //}

        #endregion

        #region "Index level ops"
        public virtual bool CreateIndex(string index, IIndexState settings)
        {
            var response = Client.CreateIndex(index, i => i.InitializeUsing(settings));

            return response.Acknowledged;
        }

        public virtual bool DeleteIndex(string index = "")
        {
            IIndicesResponse response = null;
            if (Client.IndexExists(index).Exists)
                response = Client.DeleteIndex(index);

            return response.Acknowledged;
        }

        #endregion
    }
}
