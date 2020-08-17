using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest.Models
{
    class CourseQueryParams
    {
        public string CourseGuid { get; set; }
        public string CourseGuidForNonEquality { get; set; }

        public string CourseCode { get; set; }
        public string CourseCodeForContaining { get; set; }

        public string CourseTitle { get; set; }
        public string CourseTitleForContaining { get; set; }

        public string CourseStartDateForGreaterThanOrEqualTo { get; set; }
        public string CourseEndDateForLessThanOrEqualTo { get; set; }

        public string CourseStatus { get; set; }
        public bool ExcludeExpiredCourses { get; set; }

        public string OrganisationGuid { get; set; }
        public Nullable<bool> DelFlag { get; set; }
    }
}
