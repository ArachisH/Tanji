using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public abstract class HData
    {
        protected object[] ReadData(HMessage packet, int category)
        {
            var values = new List<object>();
            switch (category & 0xFF)
            {
                case 0:
                {
                    values.Add(packet.ReadString());
                    break;
                }
                case 1: /* MapStuffData */
                {
                    int count = packet.ReadInteger();
                    values.Add(count);

                    for (int j = 0; j < count; j++)
                    {
                        values.Add(packet.ReadString());
                        values.Add(packet.ReadString());
                    }
                    break;
                }
                case 2: /* StringArrayStuffData */
                {
                    int count = packet.ReadInteger();
                    values.Add(count);

                    for (int j = 0; j < count; j++)
                    {
                        values.Add(packet.ReadString());
                    }
                    break;
                }
                case 3:
                {
                    values.Add(packet.ReadString());
                    values.Add(packet.ReadInteger());
                    break;
                }
                case 5: /* IntArrayStuffData */
                {
                    int count = packet.ReadInteger();
                    values.Add(count);

                    for (int j = 0; j < count; j++)
                    {
                        values.Add(packet.ReadInteger());
                    }
                    break;
                }
                case 6: /* HighScoreStuffData */
                {
                    values.Add(packet.ReadString());
                    values.Add(packet.ReadInteger());
                    values.Add(packet.ReadInteger());

                    int count = packet.ReadInteger();
                    values.Add(count);

                    for (int j = 0; j < count; j++)
                    {
                        int score = packet.ReadInteger();
                        values.Add(score);

                        int subCount = packet.ReadInteger();
                        values.Add(subCount);

                        for (int k = 0; k < subCount; k++)
                        {
                            values.Add(packet.ReadString());
                        }
                    }
                    break;
                }
                case 7:
                {
                    values.Add(packet.ReadString());
                    values.Add(packet.ReadInteger());
                    values.Add(packet.ReadInteger());
                    break;
                }
            }
            if (((category & 0xFF00) & 0x100) > 0)
            {
                values.Add(packet.ReadInteger());
                values.Add(packet.ReadInteger());
            }
            return values.ToArray();
        }
    }
}