using System.Runtime.Serialization;

namespace Ecom.Backend.Core.Entities.Orders
{
    public enum OrderStatus
    {
      [EnumMember(Value = "Pending")]
      Pending,
      [EnumMember(Value = "Payment Received")]
      PaymentReceived,
      [EnumMember(Value = "Payment Failed")]
      PaymentFailed


    }
}