namespace Reflection.Randomness
{
    public class FromDistribution : Attribute
    {
        public readonly Type DistributionType;
        private readonly int[] values;
        
        public FromDistribution(Type type, params int[] values)
        {
            DistributionType = type;
            this.values = values;
        }

        public IReadOnlyCollection<int> Values => Array.AsReadOnly(values);
    }
}
