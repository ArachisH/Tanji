using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HCatalogOffer
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public bool IsRentable { get; set; }

        public int CreditCost { get; set; }
        public int OtherCurrencyCost { get; set; }
        public int OtherCurrencyType { get; set; }

        public bool CanGift { get; set; }
        public HCatalogProduct[] Products { get; set; }

        public int ClubLevel { get; set; }
        public bool IsPet { get; set; }
        public bool AllowBundle { get; set; }

        public string PreviewImage { get; set; }

        public HCatalogOffer(HMessage packet)
        {
            Id = packet.ReadInteger();
            DisplayName = packet.ReadString();
            IsRentable = packet.ReadBoolean();

            CreditCost = packet.ReadInteger();
            OtherCurrencyCost = packet.ReadInteger();
            OtherCurrencyType = packet.ReadInteger();
            CanGift = packet.ReadBoolean();

            Products = new HCatalogProduct[packet.ReadInteger()];
            for (int i = 0; i < Products.Length; i++)
            {
                Products[i] = new HCatalogProduct(packet);
            }

            ClubLevel = packet.ReadInteger();
            IsPet = packet.ReadBoolean();
            AllowBundle = packet.ReadBoolean();

            PreviewImage = packet.ReadString();
        }
    }
}