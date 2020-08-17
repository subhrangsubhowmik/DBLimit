using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest.Models
{
    public abstract class DocumentBase
    {
        protected DocumentBase()
        {
            this.DocType = this.GetType().Name;
        }


        public string Id { get; set; }

        public string TenantId { get; set; }

        public string OrganizationGuid { get; set; }


        public string DocType { get; }

        public bool DelFlag { get; set; }


        public string ModifiedBy { get; set; }


        public string ModifiedByName { get; set; }


        public string ModifiedOn { get; set; }


        public string ETag { get; set; }
    }
}
