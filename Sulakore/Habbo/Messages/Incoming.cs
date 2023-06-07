﻿namespace Sulakore.Habbo.Messages;

public sealed class Incoming : Identifiers
{
    public ushort AchievementList { get; set; }
    public ushort AchievementProgress { get; set; }
    public ushort AchievementUnlocked { get; set; }
    public ushort AchievementsConfiguration { get; set; }
    public ushort AddBot { get; set; }
    public ushort AddFloorItem { get; set; }
    public ushort AddHabboItem { get; set; }
    public ushort AddPet { get; set; }
    public ushort AddUserBadge { get; set; }
    public ushort AddWallItem { get; set; }
    public ushort AdventCalendarData { get; set; }
    public ushort AdventCalendarProduct { get; set; }
    public ushort AlertLimitedSoldOut { get; set; }
    public ushort AlertMessage { get; set; }
    public ushort AlertPurchaseFailed { get; set; }
    public ushort AlertPurchaseUnavailable { get; set; }
    public ushort BaseJumpJoinQueue { get; set; }
    public ushort BaseJumpLeaveQueue { get; set; }
    public ushort BaseJumpLoadGame { get; set; }
    public ushort BaseJumpLoadGameURL { get; set; }
    public ushort BaseJumpUnloadGame { get; set; }
    public ushort BonusRare { get; set; }
    public ushort BotError { get; set; }
    public ushort BotSettings { get; set; }
    public ushort BubbleAlert { get; set; }
    public ushort BuildersClubExpired { get; set; }
    public ushort BullyReportClosed { get; set; }
    public ushort BullyReportRequest { get; set; }
    public ushort BullyReportedMessage { get; set; }
    public ushort CameraCompetitionStatus { get; set; }
    public ushort CameraPrice { get; set; }
    public ushort CameraPublishWaitMessage { get; set; }
    public ushort CameraPurchaseSuccesfull { get; set; }
    public ushort CameraRoomThumbnailSaved { get; set; }
    public ushort CameraURL { get; set; }
    public ushort CanCreateEvent { get; set; }
    public ushort CanCreateRoom { get; set; }
    public ushort CantScratchPetNotOldEnough { get; set; }
    public ushort CatalogMode { get; set; }
    public ushort CatalogPage { get; set; }
    public ushort CatalogPagesList { get; set; }
    public ushort CatalogSearchResult { get; set; }
    public ushort CatalogUpdated { get; set; }
    public ushort CfhTopicsMessage { get; set; }
    public ushort ChangeNameUpdate { get; set; }
    public ushort CloseWebPage { get; set; }
    public ushort ClubCenterData { get; set; }
    public ushort ClubData { get; set; }
    public ushort ClubGiftReceived { get; set; }
    public ushort ClubGifts { get; set; }
    public ushort CompetitionEntrySubmitResult { get; set; }
    public ushort ConnectionError { get; set; }
    public ushort ConvertedForwardToRoom { get; set; }
    public ushort CraftableProducts { get; set; }
    public ushort CraftingComposerFour { get; set; }
    public ushort CraftingRecipe { get; set; }
    public ushort CraftingResult { get; set; }
    public ushort CustomNotification { get; set; }
    public ushort DailyQuest { get; set; }
    public ushort DebugConsole { get; set; }
    public ushort Discount { get; set; }
    public ushort DoorbellAddUser { get; set; }
    public ushort EffectsListAdd { get; set; }
    public ushort EffectsListEffectEnable { get; set; }
    public ushort EffectsListRemove { get; set; }
    public ushort EpicPopupFrame { get; set; }
    public ushort ErrorLogin { get; set; }
    public ushort ExtendClubMessage { get; set; }
    public ushort FavoriteRoomChanged { get; set; }
    public ushort FavoriteRoomsCount { get; set; }
    public ushort FloodCounter { get; set; }
    public ushort FloorItemUpdate { get; set; }
    public ushort FloorPlanEditorBlockedTiles { get; set; }
    public ushort FloorPlanEditorDoorSettings { get; set; }
    public ushort ForwardToRoom { get; set; }
    public ushort FreezeLives { get; set; }
    public ushort FriendChatMessage { get; set; }
    public ushort FriendFindingRoom { get; set; }
    public ushort FriendRequest { get; set; }
    public ushort FriendRequestError { get; set; }
    public ushort FriendRequests { get; set; }
    public ushort FriendToolbarNotification { get; set; }
    public ushort Friends { get; set; }
    public ushort FriendsInterface { get; set; }
    public ushort Game2WeeklyLeaderboard { get; set; }
    public ushort Game2WeeklySmallLeaderboard { get; set; }
    public ushort GameAchievementsList { get; set; }
    public ushort GameCenterAccountInfo { get; set; }
    public ushort GameCenterFeaturedPlayers { get; set; }
    public ushort GameCenterGame { get; set; }
    public ushort GameCenterGameList { get; set; }
    public ushort GenerateSecretKey { get; set; }
    public ushort GenericAlert { get; set; }
    public ushort GenericErrorMessages { get; set; }
    public ushort GiftConfiguration { get; set; }
    public ushort GiftReceiverNotFound { get; set; }
    public ushort GroupParts { get; set; }
    public ushort GuardianNewReportReceived { get; set; }
    public ushort GuardianVotingRequested { get; set; }
    public ushort GuardianVotingResult { get; set; }
    public ushort GuardianVotingTimeEnded { get; set; }
    public ushort GuardianVotingVotes { get; set; }
    public ushort GuideSessionAttached { get; set; }
    public ushort GuideSessionDetached { get; set; }
    public ushort GuideSessionEnded { get; set; }
    public ushort GuideSessionError { get; set; }
    public ushort GuideSessionInvitedToGuideRoom { get; set; }
    public ushort GuideSessionMessage { get; set; }
    public ushort GuideSessionPartnerIsPlaying { get; set; }
    public ushort GuideSessionPartnerIsTyping { get; set; }
    public ushort GuideSessionRequesterRoom { get; set; }
    public ushort GuideSessionStarted { get; set; }
    public ushort GuideTools { get; set; }
    public ushort GuildAcceptMemberError { get; set; }
    public ushort GuildBought { get; set; }
    public ushort GuildBuyRooms { get; set; }
    public ushort GuildConfirmRemoveMember { get; set; }
    public ushort GuildEditFail { get; set; }
    public ushort GuildFavoriteRoomUserUpdate { get; set; }
    public ushort GuildForumAddComment { get; set; }
    public ushort GuildForumComments { get; set; }
    public ushort GuildForumData { get; set; }
    public ushort GuildForumList { get; set; }
    public ushort GuildForumThreadMessages { get; set; }
    public ushort GuildForumThreads { get; set; }
    public ushort GuildForumsUnreadMessagesCount { get; set; }
    public ushort GuildFurniWidget { get; set; }
    public ushort GuildInfo { get; set; }
    public ushort GuildJoinError { get; set; }
    public ushort GuildList { get; set; }
    public ushort GuildManage { get; set; }
    public ushort GuildMemberUpdate { get; set; }
    public ushort GuildMembers { get; set; }
    public ushort GuildRefreshMembersList { get; set; }
    public ushort HabboMall { get; set; }
    public ushort HabboWayQuizComposer1 { get; set; }
    public ushort HabboWayQuizComposer2 { get; set; }
    public ushort HallOfFame { get; set; }
    public ushort HelperRequestDisabled { get; set; }
    public ushort HideDoorbell { get; set; }
    public ushort HotelClosedAndOpens { get; set; }
    public ushort HotelClosesAndWillOpenAt { get; set; }
    public ushort HotelView { get; set; }
    public ushort HotelViewBadgeButtonConfig { get; set; }
    public ushort HotelViewCatalogPageExpiring { get; set; }
    public ushort HotelViewCommunityGoal { get; set; }
    public ushort HotelViewConcurrentUsers { get; set; }
    public ushort HotelViewCustomTimer { get; set; }
    public ushort HotelViewData { get; set; }
    public ushort HotelViewExpiringCatalogPageCommposer { get; set; }
    public ushort HotelViewHideCommunityVoteButton { get; set; }
    public ushort HotelViewNextLTDAvailable { get; set; }
    public ushort HotelWillCloseInMinutes { get; set; }
    public ushort HotelWillCloseInMinutesAndBackIn { get; set; }
    public ushort InventoryAchievements { get; set; }
    public ushort InventoryAddEffect { get; set; }
    public ushort InventoryBadges { get; set; }
    public ushort InventoryBots { get; set; }
    public ushort InventoryItemUpdate { get; set; }
    public ushort InventoryItems { get; set; }
    public ushort InventoryPets { get; set; }
    public ushort InventoryRefresh { get; set; }
    public ushort ItemExtraData { get; set; }
    public ushort ItemState { get; set; }
    public ushort ItemStateComposer2 { get; set; }
    public ushort ItemsDataUpdate { get; set; }
    public ushort JukeBoxMySongs { get; set; }
    public ushort JukeBoxNowPlayingMessage { get; set; }
    public ushort JukeBoxPlayList { get; set; }
    public ushort JukeBoxPlayListAddSong { get; set; }
    public ushort JukeBoxPlayListUpdated { get; set; }
    public ushort JukeBoxPlaylistFull { get; set; }
    public ushort JukeBoxTrackCode { get; set; }
    public ushort JukeBoxTrackData { get; set; }
    public ushort LatencyResponse { get; set; }
    public ushort LeprechaunStarterBundle { get; set; }
    public ushort LoveLockFurniFinished { get; set; }
    public ushort LoveLockFurniFriendConfirmed { get; set; }
    public ushort LoveLockFurniStart { get; set; }
    public ushort MachineID { get; set; }
    public ushort MarketplaceBuyError { get; set; }
    public ushort MarketplaceCancelSale { get; set; }
    public ushort MarketplaceConfig { get; set; }
    public ushort MarketplaceItemInfo { get; set; }
    public ushort MarketplaceItemPosted { get; set; }
    public ushort MarketplaceOffers { get; set; }
    public ushort MarketplaceOwnItems { get; set; }
    public ushort MarketplaceSellItem { get; set; }
    public ushort MeMenuSettings { get; set; }
    public ushort MessagesForYou { get; set; }
    public ushort MessengerError { get; set; }
    public ushort MessengerInit { get; set; }
    public ushort MinimailCount { get; set; }
    public ushort MinimailNewMessage { get; set; }
    public ushort ModTool { get; set; }
    public ushort ModToolComposerOne { get; set; }
    public ushort ModToolComposerTwo { get; set; }
    public ushort ModToolIssueChatlog { get; set; }
    public ushort ModToolIssueHandled { get; set; }
    public ushort ModToolIssueHandlerDimensions { get; set; }
    public ushort ModToolIssueInfo { get; set; }
    public ushort ModToolIssueResponseAlert { get; set; }
    public ushort ModToolIssueUpdate { get; set; }
    public ushort ModToolReportReceivedAlert { get; set; }
    public ushort ModToolRoomChatlog { get; set; }
    public ushort ModToolRoomInfo { get; set; }
    public ushort ModToolSanctionData { get; set; }
    public ushort ModToolSanctionInfo { get; set; }
    public ushort ModToolUserChatlog { get; set; }
    public ushort ModToolUserInfo { get; set; }
    public ushort ModToolUserRoomVisits { get; set; }
    public ushort MoodLightData { get; set; }
    public ushort MutedWhisper { get; set; }
    public ushort MysticBoxClose { get; set; }
    public ushort MysticBoxPrize { get; set; }
    public ushort MysticBoxStartOpen { get; set; }
    public ushort NewNavigatorCategoryUserCount { get; set; }
    public ushort NewNavigatorCollapsedCategories { get; set; }
    public ushort NewNavigatorEventCategories { get; set; }
    public ushort NewNavigatorLiftedRooms { get; set; }
    public ushort NewNavigatorMetaData { get; set; }
    public ushort NewNavigatorRoomEvent { get; set; }
    public ushort NewNavigatorSavedSearches { get; set; }
    public ushort NewNavigatorSearchResults { get; set; }
    public ushort NewNavigatorSettings { get; set; }
    public ushort NewUserGift { get; set; }
    public ushort NewUserIdentity { get; set; }
    public ushort NewYearResolution { get; set; }
    public ushort NewYearResolutionCompleted { get; set; }
    public ushort NewYearResolutionProgress { get; set; }
    public ushort NewsWidgets { get; set; }
    public ushort NotEnoughPointsType { get; set; }
    public ushort NuxAlert { get; set; }
    public ushort ObjectOnRoller { get; set; }
    public ushort OldPublicRooms { get; set; }
    public ushort OpenRoomCreationWindow { get; set; }
    public ushort OtherTradingDisabled { get; set; }
    public ushort PetBoughtNotification { get; set; }
    public ushort PetBreedingCompleted { get; set; }
    public ushort PetBreedingFailed { get; set; }
    public ushort PetBreedingResult { get; set; }
    public ushort PetBreedingStart { get; set; }
    public ushort PetBreedingStartFailed { get; set; }
    public ushort PetBreeds { get; set; }
    public ushort PetError { get; set; }
    public ushort PetInfo { get; set; }
    public ushort PetLevelUp { get; set; }
    public ushort PetLevelUpdated { get; set; }
    public ushort PetNameError { get; set; }
    public ushort PetPackageNameValidation { get; set; }
    public ushort PetStatusUpdate { get; set; }
    public ushort PetTrainingPanel { get; set; }
    public ushort PickMonthlyClubGiftNotification { get; set; }
    public ushort Ping { get; set; }
    public ushort PollQuestions { get; set; }
    public ushort PollStart { get; set; }
    public ushort PostItData { get; set; }
    public ushort PostItStickyPoleOpen { get; set; }
    public ushort PresentItemOpened { get; set; }
    public ushort PrivateRooms { get; set; }
    public ushort ProfileFriends { get; set; }
    public ushort PromoteOwnRoomsList { get; set; }
    public ushort PublicRooms { get; set; }
    public ushort PurchaseOK { get; set; }
    public ushort QuestCompleted { get; set; }
    public ushort QuestExpired { get; set; }
    public ushort ReceiveInvitation { get; set; }
    public ushort ReceivePrivateMessage { get; set; }
    public ushort RecyclerComplete { get; set; }
    public ushort RecyclerLogic { get; set; }
    public ushort RedeemVoucherError { get; set; }
    public ushort RedeemVoucherOK { get; set; }
    public ushort ReloadRecycler { get; set; }
    public ushort RemoveBot { get; set; }
    public ushort RemoveFloorItem { get; set; }
    public ushort RemoveFriend { get; set; }
    public ushort RemoveGuildFromRoom { get; set; }
    public ushort RemoveHabboItem { get; set; }
    public ushort RemovePet { get; set; }
    public ushort RemoveRoomEvent { get; set; }
    public ushort RemoveWallItem { get; set; }
    public ushort RentableItemBuyOutPrice { get; set; }
    public ushort RentableSpaceInfo { get; set; }
    public ushort ReportRoomForm { get; set; }
    public ushort RoomAccessDenied { get; set; }
    public ushort RoomAdError { get; set; }
    public ushort RoomAddRightsList { get; set; }
    public ushort RoomBannedUsers { get; set; }
    public ushort RoomCategories { get; set; }
    public ushort RoomCategoryUpdateMessage { get; set; }
    public ushort RoomChatSettings { get; set; }
    public ushort RoomCreated { get; set; }
    public ushort RoomData { get; set; }
    public ushort RoomEditSettingsError { get; set; }
    public ushort RoomEnterError { get; set; }
    public ushort RoomEntryInfo { get; set; }
    public ushort RoomEventMessage { get; set; }
    public ushort RoomFilterWords { get; set; }
    public ushort RoomFloorItems { get; set; }
    public ushort RoomFloorThicknessUpdated { get; set; }
    public ushort RoomHeightMap { get; set; }
    public ushort RoomInvite { get; set; }
    public ushort RoomInviteError { get; set; }
    public ushort RoomMessagesPostedCount { get; set; }
    public ushort RoomModel { get; set; }
    public ushort RoomMuted { get; set; }
    public ushort RoomNoRights { get; set; }
    public ushort RoomOpen { get; set; }
    public ushort RoomOwner { get; set; }
    public ushort RoomPaint { get; set; }
    public ushort RoomPetExperience { get; set; }
    public ushort RoomPetHorseFigure { get; set; }
    public ushort RoomPetRespect { get; set; }
    public ushort RoomRelativeMap { get; set; }
    public ushort RoomRemoveRightsList { get; set; }
    public ushort RoomRights { get; set; }
    public ushort RoomRightsList { get; set; }
    public ushort RoomScore { get; set; }
    public ushort RoomSettings { get; set; }
    public ushort RoomSettingsSaved { get; set; }
    public ushort RoomSettingsUpdated { get; set; }
    public ushort RoomThickness { get; set; }
    public ushort RoomUnitIdle { get; set; }
    public ushort RoomUserAction { get; set; }
    public ushort RoomUserDance { get; set; }
    public ushort RoomUserData { get; set; }
    public ushort RoomUserEffect { get; set; }
    public ushort RoomUserHandItem { get; set; }
    public ushort RoomUserIgnored { get; set; }
    public ushort RoomUserNameChanged { get; set; }
    public ushort RoomUserReceivedHandItem { get; set; }
    public ushort RoomUserRemove { get; set; }
    public ushort RoomUserRemoveRights { get; set; }
    public ushort RoomUserRespect { get; set; }
    public ushort RoomUserShout { get; set; }
    public ushort RoomUserStatus { get; set; }
    public ushort RoomUserTags { get; set; }
    public ushort RoomUserTalk { get; set; }
    public ushort RoomUserTyping { get; set; }
    public ushort RoomUserUnbanned { get; set; }
    public ushort RoomUserWhisper { get; set; }
    public ushort RoomUsers { get; set; }
    public ushort RoomUsersGuildBadges { get; set; }
    public ushort RoomWallItems { get; set; }
    public ushort SecureLoginOK { get; set; }
    public ushort SessionRights { get; set; }
    public ushort SimplePollAnswer { get; set; }
    public ushort SimplePollAnswers { get; set; }
    public ushort SimplePollStart { get; set; }
    public ushort StaffAlertAndOpenHabboWay { get; set; }
    public ushort StaffAlertWIthLinkAndOpenHabboWay { get; set; }
    public ushort StaffAlertWithLink { get; set; }
    public ushort StalkError { get; set; }
    public ushort SubmitCompetitionRoom { get; set; }
    public ushort Tags { get; set; }
    public ushort TalentLevelUpdate { get; set; }
    public ushort TalentTrack { get; set; }
    public ushort TalentTrackEmailFailed { get; set; }
    public ushort TalentTrackEmailVerified { get; set; }
    public ushort TargetedOffer { get; set; }
    public ushort TradeAccepted { get; set; }
    public ushort TradeCloseWindow { get; set; }
    public ushort TradeComplete { get; set; }
    public ushort TradeStart { get; set; }
    public ushort TradeStartFail { get; set; }
    public ushort TradeStopped { get; set; }
    public ushort TradeUpdate { get; set; }
    public ushort TradingWaitingConfirm { get; set; }
    public ushort UpdateFailed { get; set; }
    public ushort UpdateFriend { get; set; }
    public ushort UpdateStackHeight { get; set; }
    public ushort UpdateStackHeightTileHeight { get; set; }
    public ushort UpdateUserLook { get; set; }
    public ushort UserAchievementScore { get; set; }
    public ushort UserBadges { get; set; }
    public ushort UserCitizenShip { get; set; }
    public ushort UserClothes { get; set; }
    public ushort UserClub { get; set; }
    public ushort UserCredits { get; set; }
    public ushort UserCurrency { get; set; }
    public ushort UserData { get; set; }
    public ushort UserEffectsList { get; set; }
    public ushort UserHomeRoom { get; set; }
    public ushort UserPerks { get; set; }
    public ushort UserPermissions { get; set; }
    public ushort UserPoints { get; set; }
    public ushort UserProfile { get; set; }
    public ushort UserRequestedData { get; set; }
    public ushort UserSearchResult { get; set; }
    public ushort UserWardrobe { get; set; }
    public ushort VerifyMobileNumber { get; set; }
    public ushort VerifyMobilePhoneCodeWindow { get; set; }
    public ushort VerifyMobilePhoneDone { get; set; }
    public ushort VerifyMobilePhoneWindow { get; set; }
    public ushort VerifyPrimes { get; set; }
    public ushort VipTutorialsStart { get; set; }
    public ushort WallItemUpdate { get; set; }
    public ushort WatchAndEarnReward { get; set; }
    public ushort WelcomeGift { get; set; }
    public ushort WelcomeGiftError { get; set; }
    public ushort WiredConditionData { get; set; }
    public ushort WiredEffectData { get; set; }
    public ushort WiredRewardAlert { get; set; }
    public ushort WiredSaved { get; set; }
    public ushort WiredTriggerData { get; set; }
    public ushort YouTradingDisabled { get; set; }
    public ushort YoutubeDisplayList { get; set; }
    public ushort YoutubeMessageComposer2 { get; set; }
    public ushort YoutubeMessageComposer3 { get; set; }

    public Incoming()
    { }
    public Incoming(HGame game, string identifiersPath)
        : base(game, identifiersPath)
    { }
}