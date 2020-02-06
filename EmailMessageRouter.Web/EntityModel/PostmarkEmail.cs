using System;
using System.ComponentModel.DataAnnotations;

namespace EmailMessageRouter.Web.EntityModel
{
    public class PostmarkEmail
    {
        [Required(ErrorMessage = "From property is required")]
        public string From { get; set; }
        
        [Required(ErrorMessage = "To property is required")]
        public string To { get; set; }
        
        [Required(ErrorMessage = "Subject property is required")]
        public string Subject { get; set; }
        
        [Required(ErrorMessage = "HtmlBody property is required")]
        public string HtmlBody { get; set; }
    }
}