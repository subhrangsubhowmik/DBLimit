using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest.Models
{
    public class Course : DocumentBase
    {
        public string CourseGuid { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string CourseStartDate { get; set; }
        public string CourseEndDate { get; set; }
        public string CourseStatus { get; set; }
        //public bool IsCourseValidated { get; set; } //Added by gautam on 31-Dec-19
        public bool BlobContentDeleted { get; set; } //Added by Gautam on 5-Dec-2019
        public bool IsCourseIncomplete { get; set; }  //Here it means if end-date already passed

        public List<OrgDeptCoursePublishedTo> OrgDeptsCoursePublishedTo { get; set; } = new List<OrgDeptCoursePublishedTo>();
        public IList<CourseLanguageInfo> CourseSupportiveLanguages { get; set; } //Added by Gautam on 13-Nov-2019

        //public IList<NotToEnrolThoughPublished> UserIdsPublishedButNotToEnroll { get; set; } 
    }

    public class OrgDeptCoursePublishedTo
    {
        public string OrganizationGuid { get; set; }
        public string DepartmentGuid { get; set; }
    }

    public class CourseLanguageInfo
    {
        public string LanguageId { get; set; }
    }
}
