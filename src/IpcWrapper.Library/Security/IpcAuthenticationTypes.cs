namespace IpcWrapper.Security
{
    public class IpcAuthenticationTypes
    {
        public static IpcAuthenticationTypes Anonymous => new IpcAuthenticationTypes("Anonymous", "Anonymous identity");
        public static IpcAuthenticationTypes Unknown => new IpcAuthenticationTypes("Unknown", "Unknown how identity retrieved");

        public string Value { get; }

        public string Description { get; }

        protected IpcAuthenticationTypes(string authenticationType, string description)
        {
            Value = authenticationType;
            Description = description;
        }

        public static implicit operator string(IpcAuthenticationTypes auth)
        {
            return auth.Value;
        }
    }
}