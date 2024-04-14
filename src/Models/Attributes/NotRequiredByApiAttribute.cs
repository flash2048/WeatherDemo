namespace Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class NotRequiredByApiAttribute : Attribute
    {
    }
}
