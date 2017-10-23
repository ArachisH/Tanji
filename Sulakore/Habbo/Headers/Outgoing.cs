using System.IO;
using System.Runtime.Serialization.Json;

namespace Sulakore.Habbo.Headers
{
    public class Outgoing
    {
        private static readonly DataContractJsonSerializer _serializer;

        public static Outgoing Global { get; }

        #region Packet Properties/Names
        public ushort AcceptBuddy { get; set; }
        public ushort AcceptGroupMembership { get; set; }
        public ushort Action { get; set; }
        public ushort AddFavouriteRoom { get; set; }
        public ushort AddStickyNote { get; set; }
        public ushort ApplyDecoration { get; set; }
        public ushort ApplyHorseEffect { get; set; }
        public ushort ApplySign { get; set; }
        public ushort AssignRights { get; set; }
        public ushort AvatarEffectActivated { get; set; }
        public ushort AvatarEffectSelected { get; set; }
        public ushort BanUser { get; set; }
        public ushort BuyOffer { get; set; }
        public ushort CanCreateRoom { get; set; }
        public ushort CancelOffer { get; set; }
        public ushort CancelQuest { get; set; }
        public ushort CancelTyping { get; set; }
        public ushort ChangeMotto { get; set; }
        public ushort ChangeName { get; set; }
        public ushort Chat { get; set; }
        public ushort CheckGnomeName { get; set; }
        public ushort CheckPetName { get; set; }
        public ushort CheckValidName { get; set; }
        public ushort ClientVariables { get; set; }
        public ushort CloseTicketMesageEvent { get; set; }
        public ushort ConfirmLoveLock { get; set; }
        public ushort CraftSecret { get; set; }
        public ushort CreateFlat { get; set; }
        public ushort CreditFurniRedeem { get; set; }
        public ushort Dance { get; set; }
        public ushort DeclineBuddy { get; set; }
        public ushort DeclineGroupMembership { get; set; }
        public ushort DeleteFavouriteRoom { get; set; }
        public ushort DeleteGroup { get; set; }
        public ushort DeleteGroupPost { get; set; }
        public ushort DeleteGroupThread { get; set; }
        public ushort DeleteRoom { get; set; }
        public ushort DeleteStickyNote { get; set; }
        public ushort DiceOff { get; set; }
        public ushort Disconnection { get; set; }
        public ushort DropHandItem { get; set; }
        public ushort EditRoomPromotion { get; set; }
        public ushort EventTracker { get; set; }
        public ushort FindNewFriends { get; set; }
        public ushort FindRandomFriendingRoom { get; set; }
        public ushort FloorPlanEditorRoomProperties { get; set; }
        public ushort FollowFriend { get; set; }
        public ushort ForceOpenCalendarBox { get; set; }
        public ushort FriendListUpdate { get; set; }
        public ushort Game2GetWeeklyLeaderboard { get; set; }
        public ushort GenerateSecretKey { get; set; }
        public ushort GetAchievements { get; set; }
        public ushort GetBadgeEditorParts { get; set; }
        public ushort GetBadges { get; set; }
        public ushort GetBotInventory { get; set; }
        public ushort GetBuddyRequests { get; set; }
        public ushort GetCatalogIndex { get; set; }
        public ushort GetCatalogMode { get; set; }
        public ushort GetCatalogOffer { get; set; }
        public ushort GetCatalogPage { get; set; }
        public ushort GetCatalogRoomPromotion { get; set; }
        public ushort GetClientVersion { get; } = 4000;
        public ushort GetClubGifts { get; set; }
        public ushort GetCraftingRecipesAvailable { get; set; }
        public ushort GetCreditsInfo { get; set; }
        public ushort GetCurrentQuest { get; set; }
        public ushort GetDailyQuest { get; set; }
        public ushort GetEventCategories { get; set; }
        public ushort GetForumStats { get; set; }
        public ushort GetForumUserProfile { get; set; }
        public ushort GetForumsListData { get; set; }
        public ushort GetFurnitureAliases { get; set; }
        public ushort GetGameListing { get; set; }
        public ushort GetGiftWrappingConfiguration { get; set; }
        public ushort GetGroupCreationWindow { get; set; }
        public ushort GetGroupFurniConfig { get; set; }
        public ushort GetGroupFurniSettings { get; set; }
        public ushort GetGroupInfo { get; set; }
        public ushort GetGroupMembers { get; set; }
        public ushort GetGuestRoom { get; set; }
        public ushort GetHabboClubWindow { get; set; }
        public ushort GetHabboGroupBadges { get; set; }
        public ushort GetMarketplaceCanMakeOffer { get; set; }
        public ushort GetMarketplaceConfiguration { get; set; }
        public ushort GetMarketplaceItemStats { get; set; }
        public ushort GetModeratorRoomChatlog { get; set; }
        public ushort GetModeratorRoomInfo { get; set; }
        public ushort GetModeratorTicketChatlogs { get; set; }
        public ushort GetModeratorUserChatlog { get; set; }
        public ushort GetModeratorUserInfo { get; set; }
        public ushort GetModeratorUserRoomVisits { get; set; }
        public ushort GetMoodlightConfig { get; set; }
        public ushort GetOffers { get; set; }
        public ushort GetOwnOffers { get; set; }
        public ushort GetPetInformation { get; set; }
        public ushort GetPetInventory { get; set; }
        public ushort GetPetTrainingPanel { get; set; }
        public ushort GetPlayableGames { get; set; }
        public ushort GetPromoArticles { get; set; }
        public ushort GetPromotableRooms { get; set; }
        public ushort GetQuestList { get; set; }
        public ushort GetRecipeConfig { get; set; }
        public ushort GetRecyclerRewards { get; set; }
        public ushort GetRelationships { get; set; }
        public ushort GetRentableSpace { get; set; }
        public ushort GetRoomBannedUsers { get; set; }
        public ushort GetRoomEntryData { get; set; }
        public ushort GetRoomFilterList { get; set; }
        public ushort GetRoomRights { get; set; }
        public ushort GetRoomSettings { get; set; }
        public ushort GetSanctionStatus { get; set; }
        public ushort GetSelectedBadges { get; set; }
        public ushort GetSellablePetBreeds { get; set; }
        public ushort GetSongInfo { get; set; }
        public ushort GetStickyNote { get; set; }
        public ushort GetTalentTrack { get; set; }
        public ushort GetThreadData { get; set; }
        public ushort GetThreadsListData { get; set; }
        public ushort GetUserFlatCats { get; set; }
        public ushort GetUserTags { get; set; }
        public ushort GetWardrobe { get; set; }
        public ushort GetYouTubeTelevision { get; set; }
        public ushort GiveAdminRights { get; set; }
        public ushort GiveHandItem { get; set; }
        public ushort GiveRoomScore { get; set; }
        public ushort GoToFlat { get; set; }
        public ushort GoToHotelView { get; set; }
        public ushort HabboSearch { get; set; }
        public ushort IgnoreUser { get; set; }
        public ushort InfoRetrieve { get; set; }
        public ushort InitCrypto { get; set; }
        public ushort InitTrade { get; set; }
        public ushort InitializeFloorPlanSession { get; set; }
        public ushort InitializeGameCenter { get; set; }
        public ushort InitializeNewNavigator { get; set; }
        public ushort JoinGroup { get; set; }
        public ushort JoinPlayerQueue { get; set; }
        public ushort KickUser { get; set; }
        public ushort LatencyTest { get; set; }
        public ushort LetUserIn { get; set; }
        public ushort LookTo { get; set; }
        public ushort MakeOffer { get; set; }
        public ushort ManageGroup { get; set; }
        public ushort MemoryPerformance { get; set; }
        public ushort MessengerInit { get; set; }
        public ushort ModerateRoom { get; set; }
        public ushort ModerationBan { get; set; }
        public ushort ModerationCaution { get; set; }
        public ushort ModerationKick { get; set; }
        public ushort ModerationMsg { get; set; }
        public ushort ModerationMute { get; set; }
        public ushort ModerationTradeLock { get; set; }
        public ushort ModeratorAction { get; set; }
        public ushort ModifyRoomFilterList { get; set; }
        public ushort ModifyWhoCanRideHorse { get; set; }
        public ushort MoodlightUpdate { get; set; }
        public ushort MoveAvatar { get; set; }
        public ushort MoveObject { get; set; }
        public ushort MoveWallItem { get; set; }
        public ushort MuteUser { get; set; }
        public ushort NewNavigatorSearch { get; set; }
        public ushort OnBullyClick { get; set; }
        public ushort OpenBotAction { get; set; }
        public ushort OpenCalendarBox { get; set; }
        public ushort OpenFlatConnection { get; set; }
        public ushort OpenGift { get; set; }
        public ushort OpenHelpTool { get; set; }
        public ushort OpenPlayerProfile { get; set; }
        public ushort PickTicket { get; set; }
        public ushort PickUpBot { get; set; }
        public ushort PickUpPet { get; set; }
        public ushort PickupObject { get; set; }
        public ushort Ping { get; set; }
        public ushort PlaceBot { get; set; }
        public ushort PlaceObject { get; set; }
        public ushort PlacePet { get; set; }
        public ushort PostGroupContent { get; set; }
        public ushort PurchaseFromCatalog { get; set; }
        public ushort PurchaseFromCatalogAsGift { get; set; }
        public ushort PurchaseGroup { get; set; }
        public ushort PurchaseRoomPromotion { get; set; }
        public ushort RedeemOfferCredits { get; set; }
        public ushort RedeemVoucher { get; set; }
        public ushort RefreshCampaign { get; set; }
        public ushort ReleaseTicket { get; set; }
        public ushort RemoveAllRights { get; set; }
        public ushort RemoveBuddy { get; set; }
        public ushort RemoveGroupFavourite { get; set; }
        public ushort RemoveGroupMember { get; set; }
        public ushort RemoveMyRights { get; set; }
        public ushort RemoveRights { get; set; }
        public ushort RemoveSaddleFromHorse { get; set; }
        public ushort RequestBuddy { get; set; }
        public ushort RequestFurniInventory { get; set; }
        public ushort RespectPet { get; set; }
        public ushort RespectUser { get; set; }
        public ushort RideHorse { get; set; }
        public ushort SSOTicket { get; set; }
        public ushort SaveBotAction { get; set; }
        public ushort SaveBrandingItem { get; set; }
        public ushort SaveEnforcedCategorySettings { get; set; }
        public ushort SaveFloorPlanModel { get; set; }
        public ushort SaveRoomSettings { get; set; }
        public ushort SaveWardrobeOutfit { get; set; }
        public ushort SaveWiredConditionConfig { get; set; }
        public ushort SaveWiredEffectConfig { get; set; }
        public ushort SaveWiredTriggerConfig { get; set; }
        public ushort ScrGetUserInfo { get; set; }
        public ushort SendBullyReport { get; set; }
        public ushort SendMsg { get; set; }
        public ushort SendRoomInvite { get; set; }
        public ushort SetActivatedBadges { get; set; }
        public ushort SetChatPreference { get; set; }
        public ushort SetFriendBarState { get; set; }
        public ushort SetGroupFavourite { get; set; }
        public ushort SetMannequinFigure { get; set; }
        public ushort SetMannequinName { get; set; }
        public ushort SetMessengerInviteStatus { get; set; }
        public ushort SetRelationship { get; set; }
        public ushort SetSoundSettings { get; set; }
        public ushort SetToner { get; set; }
        public ushort SetUserFocusPreferenceEvent { get; set; }
        public ushort SetUsername { get; set; }
        public ushort Shout { get; set; }
        public ushort Sit { get; set; }
        public ushort StartQuest { get; set; }
        public ushort StartTyping { get; set; }
        public ushort SubmitBullyReport { get; set; }
        public ushort SubmitNewTicket { get; set; }
        public ushort TakeAdminRights { get; set; }
        public ushort ThrowDice { get; set; }
        public ushort ToggleMoodlight { get; set; }
        public ushort ToggleMuteTool { get; set; }
        public ushort ToggleYouTubeVideo { get; set; }
        public ushort TradingAccept { get; set; }
        public ushort TradingCancel { get; set; }
        public ushort TradingCancelConfirm { get; set; }
        public ushort TradingConfirm { get; set; }
        public ushort TradingModify { get; set; }
        public ushort TradingOfferItem { get; set; }
        public ushort TradingOfferItems { get; set; }
        public ushort TradingRemoveItem { get; set; }
        public ushort UnIgnoreUser { get; set; }
        public ushort UnbanUserFromRoom { get; set; }
        public ushort UniqueID { get; set; }
        public ushort UpdateFigureData { get; set; }
        public ushort UpdateForumSettings { get; set; }
        public ushort UpdateGroupBadge { get; set; }
        public ushort UpdateGroupColours { get; set; }
        public ushort UpdateGroupIdentity { get; set; }
        public ushort UpdateGroupSettings { get; set; }
        public ushort UpdateMagicTile { get; set; }
        public ushort UpdateNavigatorSettings { get; set; }
        public ushort UpdateStickyNote { get; set; }
        public ushort UpdateThread { get; set; }
        public ushort UseFurniture { get; set; }
        public ushort UseHabboWheel { get; set; }
        public ushort UseOneWayGate { get; set; }
        public ushort UseSellableClothing { get; set; }
        public ushort UseWallItem { get; set; }
        public ushort Whisper { get; set; }
        public ushort YouTubeGetNextVideo { get; set; }
        public ushort YouTubeVideoInformation { get; set; }
        #endregion

        static Outgoing()
        {
            _serializer = new DataContractJsonSerializer(typeof(Outgoing));

            Global = new Outgoing();
        }

        public void Save(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create))
                _serializer.WriteObject(fileStream, this);
        }
        public static Outgoing Load(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
                return (Outgoing)_serializer.ReadObject(fileStream);
        }
    }
}