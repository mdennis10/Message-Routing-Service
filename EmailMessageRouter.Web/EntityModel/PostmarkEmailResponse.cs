using System;

namespace EmailMessageRouter.Web.EntityModel
{
    public class PostmarkEmailResponse
    {
        public PostmarkEmailResponse(Guid requestId, int total)
        {
            RequestId = requestId;
            Total = total;
            Created = DateTime.Now;
        }
        public Guid RequestId { get; }
        public DateTime Created { get; }
        public int Total { get; }
    }
}