using System.IO;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Headers
{
    public class Incoming
    {
        private static readonly DataContractJsonSerializer _serializer;

        public static Incoming Global { get; }

        #region Packet Properties/Names
        public ushort AchievementProgressed { get; set; }
        public ushort AchievementScore { get; set; }
        public ushort AchievementUnlocked { get; set; }
        public ushort Achievements { get; set; }
        public ushort Action { get; set; }
        public ushort ActivityPoints { get; set; }
        public ushort AddExperiencePoints { get; set; }
        public ushort AuthenticationOK { get; set; }
        public ushort AvailabilityStatus { get; set; }
        public ushort AvatarEffectActivated { get; set; }
        public ushort AvatarEffectAdded { get; set; }
        public ushort AvatarEffectExpired { get; set; }
        public ushort AvatarEffects { get; set; }
        public ushort AvatarEffect { get; set; }
        public ushort BCBorrowedItems { get; set; }
        public ushort BadgeDefinitions { get; set; }
        public ushort BadgeEditorParts { get; set; }
        public ushort Badges { get; set; }
        public ushort BotInventory { get; set; }
        public ushort BroadcastMessageAlert { get; set; }
        public ushort BuddyList { get; set; }
        public ushort BuddyRequests { get; set; }
        public ushort BuildersClubMembership { get; set; }
        public ushort CampaignCalendarData { get; set; }
        public ushort Campaign { get; set; }
        public ushort CanCreateRoom { get; set; }
        public ushort CantConnect { get; set; }
        public ushort CarryObject { get; set; }
        public ushort CatalogIndex { get; set; }
        public ushort CatalogItemDiscount { get; set; }
        public ushort CatalogOffer { get; set; }
        public ushort CatalogPage { get; set; }
        public ushort CatalogUpdated { get; set; }
        public ushort CfhTopicsInit { get; set; }
        public ushort Chat { get; set; }
        public ushort CheckGnomeName { get; set; }
        public ushort CheckPetName { get; set; }
        public ushort CloseConnection { get; set; }
        public ushort ClubGifts { get; set; }
        public ushort CommunityGoalHallOfFame { get; set; }
        public ushort ConcurrentUsersGoalProgress { get; set; }
        public ushort CreditBalance { get; set; }
        public ushort Dance { get; set; }
        public ushort Doorbell { get; set; }
        public ushort EnforceCategoryUpdate { get; set; }
        public ushort Favourites { get; set; }
        public ushort FigureSetIds { get; set; }
        public ushort FindFriendsProcessResult { get; set; }
        public ushort FlatAccessDenied { get; set; }
        public ushort FlatAccessible { get; set; }
        public ushort FlatControllerAdded { get; set; }
        public ushort FlatControllerRemoved { get; set; }
        public ushort FlatCreated { get; set; }
        public ushort FloodControl { get; set; }
        public ushort FloorHeightMap { get; set; }
        public ushort FloorPlanFloorMap { get; set; }
        public ushort FloorPlanSendDoor { get; set; }
        public ushort FollowFriendFailed { get; set; }
        public ushort ForumData { get; set; }
        public ushort ForumsListData { get; set; }
        public ushort FriendListUpdate { get; set; }
        public ushort FriendNotification { get; set; }
        public ushort FurniListAdd { get; set; }
        public ushort FurniListNotification { get; set; }
        public ushort FurniListRemove { get; set; }
        public ushort FurniListUpdate { get; set; }
        public ushort FurniList { get; set; }
        public ushort FurnitureAliases { get; set; }
        public ushort Game1WeeklyLeaderboard { get; set; }
        public ushort Game2WeeklyLeaderboard { get; set; }
        public ushort Game3WeeklyLeaderboard { get; set; }
        public ushort GameAccountStatus { get; set; }
        public ushort GameAchievementList { get; set; }
        public ushort GameList { get; set; }
        public ushort GenericError { get; set; }
        public ushort GetGuestRoomResult { get; set; }
        public ushort GetRelationships { get; set; }
        public ushort GetRoomBannedUsers { get; set; }
        public ushort GetRoomFilterList { get; set; }
        public ushort GetYouTubePlaylist { get; set; }
        public ushort GetYouTubeVideo { get; set; }
        public ushort GiftWrappingConfiguration { get; set; }
        public ushort GiftWrappingError { get; set; }
        public ushort GnomeBox { get; set; }
        public ushort GroupCreationWindow { get; set; }
        public ushort GroupFurniConfig { get; set; }
        public ushort GroupFurniSettings { get; set; }
        public ushort GroupInfo { get; set; }
        public ushort GroupMemberUpdated { get; set; }
        public ushort GroupMembershipRequested { get; set; }
        public ushort GroupMembers { get; set; }
        public ushort GuestRoomSearchResult { get; set; }
        public ushort HabboActivityPointNotification { get; set; }
        public ushort HabboGroupBadges { get; set; }
        public ushort HabboSearchResult { get; set; }
        public ushort HabboUserBadges { get; set; }
        public ushort HeightMap { get; set; }
        public ushort HelperTool { get; set; }
        public ushort HideWiredConfig { get; set; }
        public ushort IgnoreStatus { get; set; }
        public ushort InitCrypto { get; set; }
        public ushort InstantMessageError { get; set; }
        public ushort ItemAdd { get; set; }
        public ushort ItemRemove { get; set; }
        public ushort ItemUpdate { get; set; }
        public ushort Items { get; set; }
        public ushort JoinQueue { get; set; }
        public ushort LatencyResponse { get; set; }
        public ushort LoadGame { get; set; }
        public ushort LoveLockDialogueClose { get; set; }
        public ushort LoveLockDialogueSetLocked { get; set; }
        public ushort LoveLockDialogue { get; set; }
        public ushort MOTDNotification { get; set; }
        public ushort MaintenanceStatus { get; set; }
        public ushort ManageGroup { get; set; }
        public ushort MarketPlaceOffers { get; set; }
        public ushort MarketPlaceOwnOffers { get; set; }
        public ushort MarketplaceCanMakeOfferResult { get; set; }
        public ushort MarketplaceCancelOfferResult { get; set; }
        public ushort MarketplaceConfiguration { get; set; }
        public ushort MarketplaceItemStats { get; set; }
        public ushort MarketplaceMakeOfferResult { get; set; }
        public ushort MessengerError { get; set; }
        public ushort MessengerInit { get; set; }
        public ushort ModeratorInit { get; set; }
        public ushort ModeratorRoomChatlog { get; set; }
        public ushort ModeratorRoomInfo { get; set; }
        public ushort ModeratorSupportTicketResponse { get; set; }
        public ushort ModeratorSupportTicket { get; set; }
        public ushort ModeratorTicketChatlog { get; set; }
        public ushort ModeratorUserChatlog { get; set; }
        public ushort ModeratorUserInfo { get; set; }
        public ushort ModeratorUserRoomVisits { get; set; }
        public ushort MoodlightConfig { get; set; }
        public ushort Muted { get; set; }
        public ushort NameChangeUpdate { get; set; }
        public ushort NavigatorCollapsedCategories { get; set; }
        public ushort NavigatorFlatCats { get; set; }
        public ushort NavigatorLiftedRooms { get; set; }
        public ushort NavigatorMetaDataParser { get; set; }
        public ushort NavigatorPreferences { get; set; }
        public ushort NavigatorSearchResultSet { get; set; }
        public ushort NavigatorSettings { get; set; }
        public ushort NewBuddyRequest { get; set; }
        public ushort NewConsoleMessage { get; set; }
        public ushort NewGroupInfo { get; set; }
        public ushort NewUserExperienceGiftOffer { get; set; }
        public ushort ObjectAdd { get; set; }
        public ushort ObjectRemove { get; set; }
        public ushort ObjectUpdate { get; set; }
        public ushort Objects { get; set; }
        public ushort OpenBotAction { get; set; }
        public ushort OpenConnection { get; set; }
        public ushort OpenGift { get; set; }
        public ushort OpenHelpTool { get; set; }
        public ushort PetBreeding { get; set; }
        public ushort PetHorseFigureInformation { get; set; }
        public ushort PetInformation { get; set; }
        public ushort PetInventory { get; set; }
        public ushort PetTrainingPanel { get; set; }
        public ushort PlayableGames { get; set; }
        public ushort Pong { get; set; }
        public ushort PopularRoomTagsResult { get; set; }
        public ushort PostUpdated { get; set; }
        public ushort PresentDeliverError { get; set; }
        public ushort ProfileInformation { get; set; }
        public ushort PromoArticles { get; set; }
        public ushort PromotableRooms { get; set; }
        public ushort PurchaseError { get; set; }
        public ushort PurchaseOK { get; set; }
        public ushort QuestAborted { get; set; }
        public ushort QuestCompleted { get; set; }
        public ushort QuestList { get; set; }
        public ushort QuestStarted { get; set; }
        public ushort QuestionParser { get; set; }
        public ushort RecyclerRewards { get; set; }
        public ushort RefreshFavouriteGroup { get; set; }
        public ushort RentableSpacesError { get; set; }
        public ushort RentableSpace { get; set; }
        public ushort RespectNotification { get; set; }
        public ushort RespectPetNotification { get; set; }
        public ushort RoomEntryInfo { get; set; }
        public ushort RoomErrorNotif { get; set; }
        public ushort RoomEvent { get; set; }
        public ushort RoomForward { get; set; }
        public ushort RoomInfoUpdated { get; set; }
        public ushort RoomInvite { get; set; }
        public ushort RoomMuteSettings { get; set; }
        public ushort RoomNotification { get; set; }
        public ushort RoomProperty { get; set; }
        public ushort RoomRating { get; set; }
        public ushort RoomReady { get; set; }
        public ushort RoomRightsList { get; set; }
        public ushort RoomSettingsData { get; set; }
        public ushort RoomSettingsSaved { get; set; }
        public ushort RoomVisualizationSettings { get; set; }
        public ushort SanctionStatus { get; set; }
        public ushort ScrSendUserInfo { get; set; }
        public ushort SecretKey { get; set; }
        public ushort SellablePetBreeds { get; set; }
        public ushort SendBullyReport { get; set; }
        public ushort SendGameInvitation { get; set; }
        public ushort SetGroupId { get; set; }
        public ushort SetUniqueId { get; set; }
        public ushort Shout { get; set; }
        public ushort Sleep { get; set; }
        public ushort SlideObjectBundle { get; set; }
        public ushort SoundSettings { get; set; }
        public ushort StickyNote { get; set; }
        public ushort SubmitBullyReport { get; set; }
        public ushort TalentLevelUp { get; set; }
        public ushort TalentTrackLevel { get; set; }
        public ushort TalentTrack { get; set; }
        public ushort ThreadCreated { get; set; }
        public ushort ThreadData { get; set; }
        public ushort ThreadReply { get; set; }
        public ushort ThreadUpdated { get; set; }
        public ushort ThreadsListData { get; set; }
        public ushort TradingAccept { get; set; }
        public ushort TradingClosed { get; set; }
        public ushort TradingComplete { get; set; }
        public ushort TradingConfirmed { get; set; }
        public ushort TradingError { get; set; }
        public ushort TradingFinish { get; set; }
        public ushort TradingStart { get; set; }
        public ushort TradingUpdate { get; set; }
        public ushort TraxSongInfo { get; set; }
        public ushort UnbanUserFromRoom { get; set; }
        public ushort UnknownCalendar { get; set; }
        public ushort UnknownGroup { get; set; }
        public ushort UpdateFavouriteGroup { get; set; }
        public ushort UpdateFavouriteRoom { get; set; }
        public ushort UpdateFreezeLives { get; set; }
        public ushort UpdateMagicTile { get; set; }
        public ushort UpdateUsername { get; set; }
        public ushort UserChange { get; set; }
        public ushort UserFlatCats { get; set; }
        public ushort UserNameChange { get; set; }
        public ushort UserObject { get; set; }
        public ushort UserPerks { get; set; }
        public ushort UserRemove { get; set; }
        public ushort UserRights { get; set; }
        public ushort UserTags { get; set; }
        public ushort UserTyping { get; set; }
        public ushort UserUpdate { get; set; }
        public ushort Users { get; set; }
        public ushort VideoOffersRewards { get; set; }
        public ushort VoucherRedeemError { get; set; }
        public ushort VoucherRedeemOk { get; set; }
        public ushort Wardrobe { get; set; }
        public ushort Whisper { get; set; }
        public ushort WiredConditionConfig { get; set; }
        public ushort WiredEffectConfig { get; set; }
        public ushort WiredTriggerConfig { get; set; }
        public ushort YouAreController { get; set; }
        public ushort YouAreNotController { get; set; }
        public ushort YouAreOwner { get; set; }
        #endregion

        static Incoming()
        {
            _serializer = new DataContractJsonSerializer(typeof(Incoming));

            Global = new Incoming();
        }

        public void Save(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
                _serializer.WriteObject(fileStream, this);
        }
        public static Incoming Load(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
                return (Incoming)_serializer.ReadObject(fileStream);
        }
    }
}