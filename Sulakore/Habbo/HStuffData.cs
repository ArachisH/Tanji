using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    public static class HStuffData
    {
        public static void ReadStuffData(int category, HMessage packet)
        {
            switch (category & 255)
            {
                case 0:
                {
                    packet.ReadString();
                    break;
                }
                case 1: /* MapStuffData */
                {
                    int count = packet.ReadInteger();
                    for (int j = 0; j < count; j++)
                    {
                        packet.ReadString();
                        packet.ReadString();
                    }
                    break;
                }
                case 2: /* StringArrayStuffData */
                {
                    int count = packet.ReadInteger();
                    for (int j = 0; j < count; j++)
                    {
                        packet.ReadString();
                    }
                    break;
                }
                case 3:
                {
                    packet.ReadString();
                    packet.ReadInteger();
                    break;
                }
                case 5: /* IntArrayStuffData */
                {
                    int count = packet.ReadInteger();
                    for (int j = 0; j < count; j++)
                    {
                        packet.ReadInteger();
                    }
                    break;
                }
                case 6: /* HighScoreStuffData */
                {
                    packet.ReadString();
                    packet.ReadInteger();
                    packet.ReadInteger();

                    int count = packet.ReadInteger();
                    for (int j = 0; j < count; j++)
                    {
                        int score = packet.ReadInteger();
                        int subCount = packet.ReadInteger();
                        for (int k = 0; k < subCount; k++)
                        {
                            packet.ReadString();
                        }
                    }
                    break;
                }
                case 7:
                {
                    packet.ReadString();
                    packet.ReadInteger();
                    packet.ReadInteger();
                    break;
                }
            }
        }
    }
}