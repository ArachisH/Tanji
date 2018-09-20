using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HCatalogProduct
    {
        public HProductType Type { get; set; }
        public int ClassId { get; set; }

        public string ExtraData { get; set; }
        public int ProductCount { get; set; }

        public bool IsLimited { get; set; }
        public int LimitedTotal { get; set; }
        public int LimitedRemaining { get; set; }

        public HCatalogProduct(HMessage packet)
        {
            Type = (HProductType)packet.ReadString()[0];
            switch (Type)
            {
                case HProductType.Badge:
                {
                    ExtraData = packet.ReadString();
                    ProductCount = 1;
                    break;
                }
                default:
                {
                    ClassId = packet.ReadInteger();
                    ExtraData = packet.ReadString();
                    ProductCount = packet.ReadInteger();
                    if (IsLimited = packet.ReadBoolean())
                    {
                        LimitedTotal = packet.ReadInteger();
                        LimitedRemaining = packet.ReadInteger();
                    }
                    break;
                }
            }
        }
    }
}