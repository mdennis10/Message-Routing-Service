namespace EmailMessageRouter.Data.EntityModel
{
    /// <summary>
    /// Account entity store account information which I am
    /// assuming consist of FROM emails and if it facilitate
    /// single or bulk emails. [Note] This entity is just a
    /// mock so it does not follow proper data normalization
    /// principles. 
    /// </summary>
    public class Account
    {
        public long AccountId { get; set; }
        public int SupportedMessageType { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}