using CosmosDBThrottlingTest.Models;
using ELearning.Infra.DocumentDb;
using ELearning.Web.Common;
using ELearning.Web.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest.Repositories
{
    public abstract class DocumentRepositoryBase<TDocument> where TDocument : DocumentBase
    {
        protected readonly Lazy<IDocCollection<TDocument>> Db;
        private string TenantId { get; set; }

        public DocumentRepositoryBase(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            this.TenantId = tenantId.Trim();

            //CosmosDb tenantDocumentDbConnectionInfo = TenantManager.GetCosmosDbAccount(this.TenantId);

            this.Db = new Lazy<IDocCollection<TDocument>>(() => DocDb
                .Connect("https://iknownow-nosql.documents.azure.com:443/", "xso34L5lJck8CjARPPnliqUwpOCOJEerTiZLuSy6Hsx6FDy8JvziE0kPBKVZq0W4WYcfYEnxgCxIlxD5p6q32Q==",
                    "iknownow", "uat-iknownow")
                .OfKind<TDocument>());
        }

        public virtual List<TDocument> GetDocuments(string sql, Dictionary<string, object> parameters, string documentAlias = "c")
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));
            if (parameters.IsNullOrEmpty()) throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(documentAlias)) throw new ArgumentNullException(nameof(documentAlias));

            if (string.IsNullOrWhiteSpace(this.TenantId)) throw new Exception("TenantId is null or white-space.");

            //if (string.IsNullOrWhiteSpace(sql)) return new List<TDocument>();
            //if (parameters.IsNullOrEmpty()) return new List<TDocument>();

            sql = sql.Trim();

            //sql += string.Format(" AND UPPER(RTRIM(LTRIM({0}.TenantId ?? ''))) = @tenantId", (documentAlias ?? "c").Trim());
            //parameters.Add("@tenantId", this.TenantId.ToUpper(CultureInfo.InvariantCulture));

            return (this.Db.Value.Select(sql, parameters) ?? new List<TDocument>()).Where(d => (d != null)).ToList();
        }

        public virtual List<TDocument> GetDocumentsWithOffset(string sql, Dictionary<string, object> parameters, string skip, string take, string documentAlias = "c")
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));
            if (parameters.IsNullOrEmpty()) throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(documentAlias)) throw new ArgumentNullException(nameof(documentAlias));

            if (string.IsNullOrWhiteSpace(this.TenantId)) throw new Exception("TenantId is null or white-space.");

            //if (string.IsNullOrWhiteSpace(sql)) return new List<TDocument>();
            //if (parameters.IsNullOrEmpty()) return new List<TDocument>();

            sql = sql.Trim();

            sql += string.Format(" AND UPPER(RTRIM(LTRIM({0}.TenantId ?? ''))) = @tenantId", (documentAlias ?? "c").Trim());
            parameters.Add("@tenantId", this.TenantId.ToUpper(CultureInfo.InvariantCulture));

            sql += " ORDER BY c.FirstName";
            sql += " OFFSET " + skip;
            sql += " LIMIT " + take;

            return (this.Db.Value.Select(sql, parameters) ?? new List<TDocument>()).Where(d => (d != null)).ToList();
        }

        //public virtual int GetDocumentCount(string sql, Dictionary<string, object> parameters, string documentAlias = "c")
        //{

        //    if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));
        //    if (parameters.IsNullOrEmpty()) throw new ArgumentNullException(nameof(parameters));
        //    if (string.IsNullOrWhiteSpace(documentAlias)) throw new ArgumentNullException(nameof(documentAlias));

        //    if (string.IsNullOrWhiteSpace(this.TenantId)) throw new Exception("TenantId is null or white-space.");

        //    sql = sql.Trim();

        //    sql += string.Format(" AND UPPER(RTRIM(LTRIM({0}.TenantId ?? ''))) = @tenantId", (documentAlias ?? "c").Trim());
        //    parameters.Add("@tenantId", this.TenantId.ToUpper(CultureInfo.InvariantCulture));

        //    /*The below part is required to get the record count directly*/
        //    sql = "SELECT Value COUNT(1)  FROM c WHERE c.DocType= 'Course' and c.TenantId='MOL'";

        //    DocumentClient client = new DocumentClient(new Uri(TenantManager.GetCosmosDbAccount(this.TenantId).UrlValue), TenantManager.GetCosmosDbAccount(this.TenantId).KeyValue, new ConnectionPolicy { EnableEndpointDiscovery = false });
        //    var list = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(TenantManager.GetCosmosDbAccount(this.TenantId).DatabaseValue, TenantManager.GetCosmosDbAccount(this.TenantId).CollectionIdValue).ToString(), new SqlQuerySpec(sql), new FeedOptions { MaxItemCount = -1 }).AsEnumerable().FirstOrDefault();
        //    return Convert.ToInt32(list);
        //}

        public HttpStatusCode CreateDocument(TDocument document, out string etag)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            //if (document == null)
            //{
            //    etag = null;
            //    return default(HttpStatusCode);
            //}

            if (string.IsNullOrWhiteSpace(this.TenantId)) throw new Exception("TenantId is null or white-space.");

            document.TenantId = this.TenantId.Trim();
            document.DelFlag = false;
            document.ModifiedOn = BusinessUtilities.GetCurrentUtcDateTimeAsString();

            return this.Db.Value.Create(document, out etag);
        }
        public ResponseModel CreateDocument(TDocument document, out HttpStatusCode httpStatusCode, out string etag)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var responseModel = new ResponseModel();

         
            httpStatusCode = this.CreateDocument(document, out etag);

            if (HttpStatusCode.Created.Equals(httpStatusCode))
            {
                responseModel.SuccessMessage = string.Format("{0} created successfully.", typeof(TDocument).Name);
                return responseModel;
            }
            else
            {
                responseModel.ErrorMessage = string.Format("{0} could not be created.", typeof(TDocument).Name);
                return responseModel;
            }
        }

        public HttpStatusCode UpdateDocument(TDocument document, out string etag)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            //if (document == null)
            //{
            //    etag = null;
            //    return default(HttpStatusCode);
            //}

            document.ModifiedOn = BusinessUtilities.GetCurrentUtcDateTimeAsString();

            return this.Db.Value.Put(document, document.ETag, out etag);
        }
        public ResponseModel UpdateDocument(TDocument document, out HttpStatusCode httpStatusCode, out string etag)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var responseModel = new ResponseModel();


            httpStatusCode = this.UpdateDocument(document, out etag);

            if (HttpStatusCode.OK.Equals(httpStatusCode))
            {
                responseModel.SuccessMessage = string.Format("{0} updated successfully.", typeof(TDocument).Name);
                return responseModel;
            }
            else if (HttpStatusCode.Created.Equals(httpStatusCode))
            {
                responseModel.ErrorMessage = string.Format("{0} created unexpectedly with the ETag value '{1}'.", typeof(TDocument).Name, etag);
                return responseModel;
            }
            else if (HttpStatusCode.Conflict.Equals(httpStatusCode))
            {
                responseModel.ErrorMessage = string.Format("{0} could not be updated. Conflict occurred. ETag: '{1}'.", typeof(TDocument).Name, etag);
                return responseModel;
            }
            else
            {
                responseModel.ErrorMessage = string.Format("{0} could not be updated.", typeof(TDocument).Name);
                return responseModel;
            }
        }

        public ResponseModel DeleteDocument(TDocument document, out HttpStatusCode httpStatusCode, out string etag)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var responseModel = new ResponseModel();

            //if (document == null)
            //{
            //    httpStatusCode = default(HttpStatusCode);
            //    etag = null;

            //    responseModel.ErrorMessage = string.Format("Input {0} is null.", typeof(TDocument).Name);
            //    return responseModel;
            //}

            document.DelFlag = true;
            httpStatusCode = this.UpdateDocument(document, out etag);

            if (HttpStatusCode.OK.Equals(httpStatusCode))
            {
                responseModel.SuccessMessage = string.Format("{0} deleted successfully.", typeof(TDocument).Name);
                return responseModel;
            }
            else if (HttpStatusCode.Created.Equals(httpStatusCode))
            {
                responseModel.ErrorMessage = string.Format("{0} created unexpectedly with the ETag value '{1}'.", typeof(TDocument).Name, etag);
                return responseModel;
            }
            else if (HttpStatusCode.Conflict.Equals(httpStatusCode))
            {
                responseModel.ErrorMessage = string.Format("{0} could not be deleted. Conflict occurred. ETag: '{1}'.", typeof(TDocument).Name, etag);
                return responseModel;
            }
            else
            {
                responseModel.ErrorMessage = string.Format("{0} could not be deleted.", typeof(TDocument).Name);
                return responseModel;
            }
        }
    }
}
