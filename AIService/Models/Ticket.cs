using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.DynamicData;

namespace AIService.Models
{
    [MetadataType(typeof(Ticket_Metadata))]
    public partial class Ticket
    {
        public bool IsValid
        {
            get { return (GetRuleViolations().Count() == 0); }
        }

        public IEnumerable<RuleViolation> GetRuleViolations()
        {
            if (String.IsNullOrEmpty(ticket_description))
                yield return new RuleViolation("Ticket description required", "ticket_description");

            if (String.IsNullOrEmpty(ticket_summary))
                yield return new RuleViolation("Ticket summary required", "ticket_summary");

            yield break;
        }

        partial void OnValidate(ChangeAction action)
        {
            if (!IsValid)
                throw new ApplicationException("Rule violations prevent saving");
        }
    }

    [TableName("Ticket")]
    public class Ticket_Metadata
    {
        [DisplayName("Created")]
        public object ticket_created { get; set; }

        [DisplayName("Description")]
        public object ticket_description { get; set; }

        [DisplayName("Summary")]
        public object ticket_summary { get; set; }

        [DisplayName("Updated")]
        public object ticket_updated { get; set; }

        [DisplayName("Due Date")]
        public object ticket_duedate { get; set; }

        [DisplayName("Asset")]
        public object asset_id { get; set; }

        [DisplayName("Category")]
        public object category_id { get; set; }

        [DisplayName("Status")]
        public object status_id { get; set; }

        [DisplayName("Assigned Technician")]
        public object technician_id { get; set; }

        [DisplayName("Client")]
        public object customer_id { get; set; }

        [ScaffoldColumn(false)]
        public object ticket_mlreviewed { get; set; }

        [ScaffoldColumn(false)]
        public object IsValid { get; set; }
    }

    public class RuleViolation
    {

        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }

        public RuleViolation(string errorMessage, string propertyName)
        {
            ErrorMessage = errorMessage;
            PropertyName = propertyName;
        }
    }

    
}