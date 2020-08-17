using CosmosDBThrottlingTest.Models;
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
    class CourseRepository : DocumentRepositoryBase<Course>
    {
        public CourseRepository(string tenantId) : base(tenantId)
        {

        }

        public List<Course> GetCourses(CourseQueryParams courseQueryParams = null)
        {
            string sql = "SELECT * FROM c where c.DocType = @docType";
            var parameters = new Dictionary<string, object>() { { "@docType", nameof(Course) } };

            if (courseQueryParams != null)
            {
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseGuid))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.CourseGuid ?? ''))) = @courseGuid";
                    parameters.Add("@courseGuid", courseQueryParams.CourseGuid.Trim().ToUpper(CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseGuidForNonEquality))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.CourseGuid ?? ''))) != @courseGuidForNonEquality";
                    parameters.Add("@courseGuidForNonEquality", courseQueryParams.CourseGuidForNonEquality.Trim().ToUpper(CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseCode))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.CourseCode ?? ''))) = @courseCode";
                    parameters.Add("@courseCode", courseQueryParams.CourseCode.Trim().ToUpper(CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseCodeForContaining))
                {
                    sql += " AND CONTAINS(UPPER(RTRIM(LTRIM(c.CourseCode ?? ''))), @courseCodeForContaining)";
                    parameters.Add("@courseCodeForContaining", courseQueryParams.CourseCodeForContaining.Trim().ToUpper(CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseTitle))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.CourseTitle ?? ''))) = @courseTitle";
                    parameters.Add("@courseTitle", courseQueryParams.CourseTitle.Trim().ToUpper(CultureInfo.InvariantCulture));
                }
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseTitleForContaining))
                {
                    sql += " AND CONTAINS(UPPER(RTRIM(LTRIM(c.CourseTitle ?? ''))), @courseTitleForContaining)";
                    parameters.Add("@courseTitleForContaining", courseQueryParams.CourseTitleForContaining.Trim().ToUpper(CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseStartDateForGreaterThanOrEqualTo))
                {
                    sql += " AND c.CourseStartDate >= @courseStartDate";
                    parameters.Add("@courseStartDate", courseQueryParams.CourseStartDateForGreaterThanOrEqualTo.Trim());
                }
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseEndDateForLessThanOrEqualTo))
                {
                    sql += " AND c.CourseEndDate <= @courseEndDate";
                    parameters.Add("@courseEndDate", courseQueryParams.CourseEndDateForLessThanOrEqualTo.Trim());
                }
                if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseStatus))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.CourseStatus ?? ''))) = @courseStatus";
                    parameters.Add("@courseStatus", courseQueryParams.CourseStatus.Trim().ToUpper(CultureInfo.InvariantCulture));
                }
                if (courseQueryParams.ExcludeExpiredCourses)
                {
                    sql += " AND c.CourseEndDate >= @currentDate";
                    //parameters.Add("@currentDate", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    parameters.Add("@currentDate", BusinessUtilities.GetCurrentUtcDateAsString());
                }

                if (!string.IsNullOrWhiteSpace(courseQueryParams.OrganisationGuid))
                {
                    sql += " AND UPPER(RTRIM(LTRIM(c.OrganizationGuid ?? ''))) = @OrganizationGuid";
                    parameters.Add("@OrganizationGuid", courseQueryParams.OrganisationGuid.Trim().ToUpper(CultureInfo.InvariantCulture));
                }

                if (courseQueryParams.DelFlag.HasValue)
                {
                    sql += " AND c.DelFlag = @delFlag AND c.TenantId='MOL'";
                    parameters.Add("@delFlag", courseQueryParams.DelFlag);
                }
            }

            List<Course> courses = base.GetDocuments(sql, parameters).OrderBy(c => c.CourseCode).ToList();
            return courses;
        }


        //public int GetCoursesCount(CourseQueryParams courseQueryParams = null)
        //{
        //    string sql = "SELECT COUNT(c.id) TotalCounter FROM c WHERE c.DocType = @docType";
        //    var parameters = new Dictionary<string, object>() { { "@docType", nameof(Course) } };

        //    if (courseQueryParams != null)
        //    {
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseGuid))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.CourseGuid ?? ''))) = @courseGuid";
        //            parameters.Add("@courseGuid", courseQueryParams.CourseGuid.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseGuidForNonEquality))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.CourseGuid ?? ''))) != @courseGuidForNonEquality";
        //            parameters.Add("@courseGuidForNonEquality", courseQueryParams.CourseGuidForNonEquality.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }

        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseCode))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.CourseCode ?? ''))) = @courseCode";
        //            parameters.Add("@courseCode", courseQueryParams.CourseCode.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseCodeForContaining))
        //        {
        //            sql += " AND CONTAINS(UPPER(RTRIM(LTRIM(c.CourseCode ?? ''))), @courseCodeForContaining)";
        //            parameters.Add("@courseCodeForContaining", courseQueryParams.CourseCodeForContaining.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }

        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseTitle))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.CourseTitle ?? ''))) = @courseTitle";
        //            parameters.Add("@courseTitle", courseQueryParams.CourseTitle.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseTitleForContaining))
        //        {
        //            sql += " AND CONTAINS(UPPER(RTRIM(LTRIM(c.CourseTitle ?? ''))), @courseTitleForContaining)";
        //            parameters.Add("@courseTitleForContaining", courseQueryParams.CourseTitleForContaining.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }

        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseStartDateForGreaterThanOrEqualTo))
        //        {
        //            sql += " AND c.CourseStartDate >= @courseStartDate";
        //            parameters.Add("@courseStartDate", courseQueryParams.CourseStartDateForGreaterThanOrEqualTo.Trim());
        //        }
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseEndDateForLessThanOrEqualTo))
        //        {
        //            sql += " AND c.CourseEndDate <= @courseEndDate";
        //            parameters.Add("@courseEndDate", courseQueryParams.CourseEndDateForLessThanOrEqualTo.Trim());
        //        }
        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.CourseStatus))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.CourseStatus ?? ''))) = @courseStatus";
        //            parameters.Add("@courseStatus", courseQueryParams.CourseStatus.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }
        //        if (courseQueryParams.ExcludeExpiredCourses)
        //        {
        //            sql += " AND c.CourseEndDate >= @currentDate";
        //            //parameters.Add("@currentDate", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        //            parameters.Add("@currentDate", BusinessUtilities.GetCurrentUtcDateAsString());
        //        }

        //        if (!string.IsNullOrWhiteSpace(courseQueryParams.OrganisationGuid))
        //        {
        //            sql += " AND UPPER(RTRIM(LTRIM(c.OrganizationGuid ?? ''))) = @OrganizationGuid";
        //            parameters.Add("@OrganizationGuid", courseQueryParams.OrganisationGuid.Trim().ToUpper(CultureInfo.InvariantCulture));
        //        }

        //        if (courseQueryParams.DelFlag.HasValue)
        //        {
        //            sql += " AND c.DelFlag = @delFlag";
        //            parameters.Add("@delFlag", courseQueryParams.DelFlag);
        //        }
        //    }

        //    return base.GetDocumentCount(sql, parameters);
        //}

        public ResponseModel CreateCourse(Course course)
        {
            var responseModel = new ResponseModel();

            if (course == null)
            {
                responseModel.ErrorMessage = "Input Course is null.";
                return responseModel;
            }

            // Check whether any Course already exists with the same Course Code
            List<Course> courses = this.GetCourses(new CourseQueryParams() { CourseCode = course.CourseCode, DelFlag = false });

            if (!courses.IsNullOrEmpty())
            {
                responseModel.ErrorMessage = "Course Code already exists.";
                return responseModel;
            }

            responseModel = base.CreateDocument(course, out HttpStatusCode httpStatusCode, out string etag);
            return responseModel;
        }

        public ResponseModel UpdateCourse(Course course)
        {
            var responseModel = new ResponseModel();

            if (course == null)
            {
                responseModel.ErrorMessage = "Input Course is null.";
                return responseModel;
            }

            // Check whether any other Course already exists with the same Course Code
            List<Course> courses = this.GetCourses(new CourseQueryParams() { CourseGuidForNonEquality = course.CourseGuid, CourseCode = course.CourseCode, DelFlag = false });

            if (!courses.IsNullOrEmpty())
            {
                responseModel.ErrorMessage = "Course Code already exists.";
                return responseModel;
            }

            courses = this.GetCourses(new CourseQueryParams() { CourseGuid = course.CourseGuid });

            if (courses.IsNullOrEmpty())
            {
                responseModel.ErrorMessage = "Course not found.";
                return responseModel;
            }

            Course courseToSave = courses.First();

            courseToSave.CourseCode = course.CourseCode;
            courseToSave.CourseTitle = course.CourseTitle;
            courseToSave.CourseDescription = course.CourseDescription;
            courseToSave.CourseStartDate = course.CourseStartDate;
            courseToSave.CourseEndDate = course.CourseEndDate;
            courseToSave.CourseStatus = course.CourseStatus;
            courseToSave.OrgDeptsCoursePublishedTo = course.OrgDeptsCoursePublishedTo;
            courseToSave.CourseSupportiveLanguages = course.CourseSupportiveLanguages;
            //courseToSave.IsCourseValidated = course.IsCourseValidated;
            courseToSave.ModifiedBy = course.ModifiedBy;
            courseToSave.ModifiedByName = course.ModifiedByName;

            responseModel = base.UpdateDocument(courseToSave, out HttpStatusCode httpStatusCode, out string etag);
            return responseModel;
        }

        public ResponseModel DeleteCourse(Course course)
        {
            var responseModel = new ResponseModel();

            if (course == null)
            {
                responseModel.ErrorMessage = "Input Course is null.";
                return responseModel;
            }

            if (string.IsNullOrWhiteSpace(course.CourseGuid))
            {
                responseModel.ErrorMessage = "Course Guid not found.";
                return responseModel;
            }

            List<Course> courses = this.GetCourses(new CourseQueryParams() { CourseGuid = course.CourseGuid });
            courses = courses.FindAll(c => (c != null));

            if (courses.IsNullOrEmpty())
            {
                responseModel.ErrorMessage = "Course not found.";
                return responseModel;
            }

            Course courseToBeDeleted = courses.First();

            courseToBeDeleted.ModifiedBy = course.ModifiedBy;
            courseToBeDeleted.ModifiedByName = course.ModifiedByName;

            //responseModel = base.DeleteDocument(courses.First(), out HttpStatusCode httpStatusCode, out string etag);
            responseModel = base.DeleteDocument(courseToBeDeleted, out HttpStatusCode httpStatusCode, out string etag);
            return responseModel;
        }
    }
}
