namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public abstract class ProxiaField
    {
        public bool IsRequired { get; }

        public bool HasDefaultValue { get; }

        public abstract string Value { get; set; }

        protected ProxiaField(bool isRequired, bool hasDefaultValue)
        {
            IsRequired = isRequired;
            HasDefaultValue = hasDefaultValue;
        }

        public override string ToString() => Value;
    }
}
