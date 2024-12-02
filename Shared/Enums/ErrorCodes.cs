using System.ComponentModel;

namespace Shared.Enums
{
    public enum ErrorCodes
    {
        [Description("The file doesn't exist.")]
        FileMissing,
        [Description("Target amount cannot be <= 0")]
        TargetAmountError,
        [Description("There are no order books available")]
        NoOrderBooks,
        [Description("Insufficient amount available to fulfill the target amount.")]
        InsufficientAmount
    }
}
