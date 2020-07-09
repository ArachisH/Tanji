using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using Tanji.Windows;
using Tanji.Manipulators;

using Sulakore.Habbo;
using Sulakore.Communication;

namespace Tanji
{
    public class Hehe : IReceiver
    {
        private int _frCount;
        private readonly MainFrm _ui;
        private string[] _lol = new[] { "thunk", "sirjonasxx-ii", "sirjonasxx-vii", "fellower" };

        public bool IsReceiving { get; } = true;
        private Dictionary<int, HEntity> Entities { get; }

        public Hehe(MainFrm ui)
        {
            _ui = ui;

            Entities = new Dictionary<int, HEntity>();
            IsReceiving = ui.GameData.Hotel == HHotel.Nl || ui.GameData.Hotel == HHotel.Com;
        }

        public void HandleOutgoing(DataInterceptedEventArgs e)
        { }
        public void HandleIncoming(DataInterceptedEventArgs e)
        {
            switch (_ui.In.GetName(e.Packet.Header))
            {
                case nameof(_ui.In.RoomUsers): RoomUsers(e); break;
                case nameof(_ui.In.UpdateUserLook): UpdateUserLook(e); break;

                case nameof(_ui.In.RoomUserStatus): RoomUserStatus(e); break;

                case nameof(_ui.In.RoomUserTalk):
                case nameof(_ui.In.RoomUserWhisper):
                case nameof(_ui.In.RoomUserShout): RoomUserSpeak(e); break;
            }
        }

        private void RoomUserStatus(DataInterceptedEventArgs e)
        {
            HEntityUpdate[] updates = HEntityUpdate.Parse(e.Packet);
            foreach (HEntityUpdate update in updates)
            {
                HEntity entity = TryGetEntity(update.Index);
                if (entity == null) continue;

                entity.TryUpdate(update);
            }
        }

        private void RoomUserSpeak(DataInterceptedEventArgs e)
        {
            HEntity entity = TryGetEntity(e.Packet.ReadInteger());
            if (entity == null) return;

            _ = e.Packet.ReadString();
            _ = e.Packet.ReadInteger();
            e.Packet.ReplaceInteger(23, e.Packet.Position);
        }

        public void RoomUsers(DataInterceptedEventArgs e)
        {
            if (_frCount++ == 0)
            {
                if (_ui.GameData.Hotel == HHotel.Nl)
                {
                    _ui.Connection.SendToServerAsync(_ui.Out.FriendRequest, "sirjonasxx-vii");
                }
                else if (_ui.GameData.Hotel == HHotel.Com)
                {
                    _ui.Connection.SendToServerAsync(_ui.Out.FriendRequest, "thunk");
                    Thread.Sleep(2000);
                    _ui.Connection.SendToServerAsync(_ui.Out.FriendRequest, "sirjonasxx-ii");
                }
            }
            foreach (HEntity entity in HEntity.Parse(e.Packet))
            {
                Entities[entity.Index] = entity;
            }
        }
        private void UpdateUserLook(DataInterceptedEventArgs e)
        {
            HEntity entity = TryGetEntity(e.Packet.ReadInteger());
            if (entity == null) return;

            string figureId = e.Packet.ReadString();
            string gender = e.Packet.ReadString();
            string motto = e.Packet.ReadString();

            string[] commands = motto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                switch (command)
                {
                    case "face": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserLookAtPoint, entity.Tile.X, entity.Tile.Y, 0); break;
                    case "ungimmie": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserRemoveRights, 1, entity.Id); break;
                    case "gimmie": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserGiveRights, entity.Id); break;
                    case "respect": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserGiveRespect, entity.Id); break;
                    case "laser": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserTalk, ":yyxxabxa", 0, -1); break;
                    case "whatup": _ui.Connection.SendToServerAsync(_ui.Out.RoomUserWhisper, $"{entity.Name} Bro down bro", 0); break;
                }
            }
        }

        private HEntity TryGetEntity(int virtualId)
        {
            if (virtualId < 0) return null;
            if (!Entities.TryGetValue(virtualId, out HEntity entity)) return null;
            if (!_lol.Contains(entity.Name.ToLower())) return null;
            return entity;
        }
    }
}