using System;
using System.Globalization;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public class HEntityUpdate : HData
    {
        public int Index { get; set; }
        public bool IsController { get; set; }

        public HPoint Tile { get; set; }
        public HPoint MovingTo { get; set; }

        public HSign Sign { get; set; }
        public HStance Stance { get; set; }
        public HAction Action { get; set; }
        public HDirection HeadFacing { get; set; }
        public HDirection BodyFacing { get; set; }

        public HEntityUpdate(HMessage packet)
        {
            Index = packet.ReadInteger();

            Tile = new HPoint(packet.ReadInteger(), packet.ReadInteger(),
                double.Parse(packet.ReadString(), CultureInfo.InvariantCulture));

            HeadFacing = (HDirection)packet.ReadInteger();
            BodyFacing = (HDirection)packet.ReadInteger();

            string[] actionData = packet.ReadString()
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string actionInfo in actionData)
            {
                string[] actionValues = actionInfo.Split(' ');

                if (actionValues.Length < 2) continue;
                if (string.IsNullOrWhiteSpace(actionValues[0])) continue;

                switch (actionValues[0])
                {
                    case "flatctrl":
                    {
                        IsController = true;
                        break;
                    }
                    case "mv":
                    {
                        string[] values = actionValues[1].Split(',');
                        if (values.Length >= 3)
                        {
                            MovingTo = new HPoint(int.Parse(values[0]), int.Parse(values[1]),
                                double.Parse(values[2], CultureInfo.InvariantCulture));
                        }
                        Action = HAction.Move;
                        break;
                    }
                    case "sit":
                    {
                        Action = HAction.Sit;
                        Stance = HStance.Sit;
                        break;
                    }
                    case "lay":
                    {
                        Action = HAction.Lay;
                        Stance = HStance.Lay;
                        break;
                    }
                    case "sign":
                    {
                        Sign = (HSign)int.Parse(actionValues[1]);
                        Action = HAction.Sign;
                        break;
                    }
                }
            }
        }

        public static HEntityUpdate[] Parse(HMessage packet)
        {
            var updates = new HEntityUpdate[packet.ReadInteger()];
            for (int i = 0; i < updates.Length; i++)
            {
                updates[i] = new HEntityUpdate(packet);
            }
            return updates;
        }
    }
}