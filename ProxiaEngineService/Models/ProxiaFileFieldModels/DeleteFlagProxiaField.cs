using System;

namespace ProxiaEngineService.Models.ProxiaFileFieldModels
{
    public class DeleteFlagProxiaField : ProxiaField
    {
        public bool Flag { get; private set; }

        public override string Value
        {
            get
            {
                return Flag ? "x" : string.Empty;
            }
            set
            {
                switch (value)
                {
                    case "x":
                        Flag = true;
                        break;
                    case "":
                        Flag = false;
                        break;
                    default:
                        throw new ArgumentException("Invalid value, \"x\" or \"\" expected");
                }
            }
        }

        public DeleteFlagProxiaField()
            : base(false, false)
        { }

        public override string ToString()
        {
            return Flag ? "x" : string.Empty;
        }
    }
}
