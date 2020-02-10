using System;
using System.ComponentModel.DataAnnotations;

namespace EmailMessageRouter.Web.EntityModel
{
    public class PostmarkEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
}
