using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest.Models
{
    public class ResponseModel
    {
        public string InfoMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public bool HasErrorMessage { get { return !string.IsNullOrWhiteSpace(this.ErrorMessage); } }


        public virtual void AppendInfoMessage(string infoMessage)
        {
            infoMessage = (infoMessage ?? string.Empty).Trim();

            this.InfoMessage = $"{(this.InfoMessage ?? string.Empty).Trim()} {infoMessage}";
            this.InfoMessage = this.InfoMessage.Trim();
        }
        public virtual void AppendSuccessMessage(string successMessage)
        {
            successMessage = (successMessage ?? string.Empty).Trim();

            this.SuccessMessage = $"{(this.SuccessMessage ?? string.Empty).Trim()} {successMessage}";
            this.SuccessMessage = this.SuccessMessage.Trim();
        }
        public virtual void AppendErrorMessage(string errorMessage)
        {
            errorMessage = (errorMessage ?? string.Empty).Trim();

            this.ErrorMessage = $"{(this.ErrorMessage ?? string.Empty).Trim()} {errorMessage}";
            this.ErrorMessage = this.ErrorMessage.Trim();
        }

        public virtual void PrependInfoMessage(string infoMessage)
        {
            infoMessage = (infoMessage ?? string.Empty).Trim();

            this.InfoMessage = $"{infoMessage} {(this.InfoMessage ?? string.Empty).Trim()}";
            this.InfoMessage = this.InfoMessage.Trim();
        }
        public virtual void PrependSuccessMessage(string successMessage)
        {
            successMessage = (successMessage ?? string.Empty).Trim();

            this.SuccessMessage = $"{successMessage} {(this.SuccessMessage ?? string.Empty).Trim()}";
            this.SuccessMessage = this.SuccessMessage.Trim();
        }
        public virtual void PrependErrorMessage(string errorMessage)
        {
            errorMessage = (errorMessage ?? string.Empty).Trim();

            this.ErrorMessage = $"{errorMessage} {(this.ErrorMessage ?? string.Empty).Trim()}";
            this.ErrorMessage = this.ErrorMessage.Trim();
        }
    }
}
