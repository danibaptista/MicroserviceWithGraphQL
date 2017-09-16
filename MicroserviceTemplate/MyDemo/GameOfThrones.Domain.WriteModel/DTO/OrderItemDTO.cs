using System.Runtime.Serialization;

namespace MicroserviceArchitecture.GameOfThrones.Domain.WriteModel
{
    [DataContract]
    public class OrderItemDTO
    {
        [DataMember]
        public decimal Discount { get; private set; }

        [DataMember]
        public string PictureUrl { get; private set; }

        [DataMember]
        public int ProductId { get; private set; }

        [DataMember]
        public string ProductName { get; private set; }

        [DataMember]
        public decimal UnitPrice { get; private set; }

        [DataMember]
        public int Units { get; private set; }

        public OrderItemDTO(decimal discount, string pictureUrl, int productId, string productName, decimal unitPrice, int units)
        {
            Discount = discount;
            PictureUrl = pictureUrl;
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Units = units;
        }
    }
}