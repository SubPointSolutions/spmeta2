namespace SPMeta2.Attributes.Capabilities
{
    public class WebTokenCapabilityAttribute : TokenCapabilityAttribute
    {
        public WebTokenCapabilityAttribute()
        {
            Token = "~site";
        }
    }
}
