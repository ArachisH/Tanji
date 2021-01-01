using System;

using Sulakore.Habbo;

namespace Tanji.Habbo
{
    public class UnityGame : TGame
    {
        public UnityGame()
        {
            // Not passing 'this' will initialize the messages by their default unity id values.
            In = new Incoming();
            Out = new Outgoing();
        }

        public override short Resolve(string name, bool isOutgoing)
        {
            throw new NotImplementedException();
        }
        public override MessageInfo GetInformation(HMessage message)
        {
            throw new NotImplementedException();
        }
    }
}