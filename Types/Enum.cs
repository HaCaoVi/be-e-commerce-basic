using System.ComponentModel;

namespace e_commerce_basic.Types
{
    public enum EGenderType
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Male")]
        Male = 1,
        [Description("Female")]
        Female = 2,
        [Description("Other")]
        Other = 3
    }

    public enum EDiscount
    {
        [Description("%")]
        Percent = 0,
        [Description("VND")]
        Vnd = 1
    }

    public enum EAccountType
    {
        [Description("Local")]
        Local = 0,
        [Description("Google")]
        Google = 1
    }
}