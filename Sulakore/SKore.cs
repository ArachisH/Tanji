using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Habbo;
using Sulakore.Habbo.Web;

namespace Sulakore
{
    /// <summary>
    /// Provides static methods for extracting public information from a hotel.
    /// </summary>
    public static class SKore
    {
        public const string USER_API_FORMAT = "{0}/api/public/users?name={1}";
        public const string PROFILE_API_FORMAT = "{0}/api/public/users/{1}/profile";
        public const string ChromeAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";

        private static readonly HRequest _hRequest;
        private static readonly IDictionary<HHotel, IDictionary<string, string>> _uniqueIds;

        static SKore()
        {
            _hRequest = new HRequest();
            _uniqueIds = new Dictionary<HHotel, IDictionary<string, string>>();
        }

        /// <summary>
        /// Returns the avatar image of a specified size generated with the given figured id in an asynchronous operation.
        /// </summary>
        /// <param name="figureId">The figured id of the avatar.</param>
        /// <param name="size">The output size of the avatar image.</param>
        public static async Task<Bitmap> GetAvatarAsync(string figureId, HSize size)
        {
            using (var client = new WebClient())
            {
                client.Proxy = null;
                client.Headers[HttpRequestHeader.UserAgent] = ChromeAgent;

                byte[] bitmapData = await client.DownloadDataTaskAsync(
                    $"https://www.habbo.com/habbo-imaging/avatarimage?size={(char)size}&figure={figureId}");

                using (var bitmapStream = new MemoryStream(bitmapData))
                    return (Bitmap)Image.FromStream(bitmapStream);
            }
        }
        /// <summary>
        /// Returns the user's basic information associated with the given name using the specified <see cref="HHotel"/> in an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="hotel">The hotel where the target user is from.</param>
        public static async Task<HUser> GetUserAsync(string name, HHotel hotel)
        {
            string userJson = await _hRequest.DownloadStringAsync(
                string.Format(USER_API_FORMAT, hotel.ToUrl(true), name)).ConfigureAwait(false);

            if (userJson.StartsWith("{\"error"))
                return null;

            var user = HUser.Create(userJson);

            if (!_uniqueIds.ContainsKey(hotel))
                _uniqueIds[hotel] = new Dictionary<string, string>();

            _uniqueIds[hotel][user.Name] = user.UniqueId;
            return user;
        }
        /// <summary>
        /// Returns the user's unique identifier associated with the given name using the specified <see cref="HHotel"/> in an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="hotel">The hotel where the target user is from.</param>
        public static async Task<string> GetUniqueIdAsync(string name, HHotel hotel)
        {
            if (_uniqueIds.ContainsKey(hotel) &&
                _uniqueIds[hotel].ContainsKey(name))
            {
                return _uniqueIds[hotel][name];
            }

            HUser user = await GetUserAsync(
                name, hotel).ConfigureAwait(false);

            return user?.UniqueId;
        }
        /// <summary>
        /// Returns the user's public profile information associated with the given name using the specified <see cref="HHotel"/> in an asynchronous operation.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="hotel">The hotel where the target user is from.</param>
        public static async Task<HProfile> GetProfileAsync(string name, HHotel hotel)
        {
            string uniqueId = await GetUniqueIdAsync(name, hotel).ConfigureAwait(false);
            return await GetProfileByUniqueIdAsync(uniqueId, hotel);
        }
        /// <summary>
        /// Returns the user's public profile information associated with the given unique indentifier using the specified <see cref="HHotel"/> in an asynchronous operation.
        /// </summary>
        /// <param name="uniqueId">The unique identifier of the user.</param>
        /// <param name="hotel">The hotel where the target user is from.</param>
        public static async Task<HProfile> GetProfileByUniqueIdAsync(string uniqueId, HHotel hotel)
        {
            string profileJson = await _hRequest.DownloadStringAsync(
                string.Format(PROFILE_API_FORMAT, hotel.ToUrl(true), uniqueId)).ConfigureAwait(false);

            if (profileJson.StartsWith("{\"error"))
                return null;

            return HProfile.Create(profileJson);
        }

        /// <summary>
        /// Returns the primitive value for the specified <see cref="HBan"/>.
        /// </summary>
        /// <param name="ban">The <see cref="HBan"/> you wish to retrieve the primitive value from.</param>
        /// <returns></returns>
        public static string Juice(this HBan ban)
        {
            switch (ban)
            {
                default:
                case HBan.Day: return "RWUAM_BAN_USER_DAY";

                case HBan.Hour: return "RWUAM_BAN_USER_HOUR";
                case HBan.Permanent: return "RWUAM_BAN_USER_PERM";
            }
        }

        /// <summary>
        /// Returns the domain associated with the specified <see cref="HHotel"/>.
        /// </summary>
        /// <param name="hotel">The <see cref="HHotel"/> that is associated with the wanted domain.</param>
        /// <returns></returns>
        public static string ToDomain(this HHotel hotel)
        {
            string value = hotel.ToString().ToLower();
            return value.Length != 5 ? value : value.Insert(3, ".");
        }
        /// <summary>
        /// Returns the full URL representation of the specified <seealso cref="HHotel"/>.
        /// </summary>
        /// <param name="hotel">The <seealso cref="HHotel"/> you wish to retrieve the full URL from.</param>
        /// <returns></returns>
        public static string ToUrl(this HHotel hotel, bool isHttps)
        {
            string protocol = (isHttps ? "https" : "http");
            return $"{protocol}://www.Habbo.{hotel.ToDomain()}";
        }

        /// <summary>
        /// Returns the <see cref="HBan"/> associated with the specified value.
        /// </summary>
        /// <param name="ban">The string representation of the <see cref="HBan"/> object.</param>
        /// <returns></returns>
        public static HBan ToBan(string ban)
        {
            switch (ban)
            {
                default:
                case "RWUAM_BAN_USER_DAY": return HBan.Day;

                case "RWUAM_BAN_USER_HOUR": return HBan.Hour;
                case "RWUAM_BAN_USER_PERM": return HBan.Permanent;
            }
        }
        /// <summary>
        /// Returns the <see cref="HHotel"/> associated with the specified value.
        /// </summary>
        /// <param name="host">The string representation of the <see cref="HHotel"/> object.</param>
        /// <returns></returns>
        public static HHotel ToHotel(string host)
        {
            HHotel hotel = HHotel.Com;
            string identifier = host.GetChild("game-", '.')
                .Replace("us", "com");

            if (string.IsNullOrWhiteSpace(identifier))
            {
                if (host.StartsWith("."))
                    host = ("habbo" + host);

                identifier = host.GetChild("habbo.", '/')
                    .Replace(".", string.Empty);
            }
            else if (identifier == "tr" || identifier == "br")
                identifier = "com" + identifier;

            if (!Enum.TryParse(identifier, true, out hotel))
            {
                throw new ArgumentException(
                    "The specified host is not a valid hotel.", nameof(host));
            }
            return hotel;
        }
        /// <summary>
        /// Returns the <see cref="HGender"/> associated with the specified value.
        /// </summary>
        /// <param name="gender">The string representation of the <see cref="HGender"/> object.</param>
        /// <returns></returns>
        public static HGender ToGender(string gender)
        {
            return (HGender)gender.ToUpper()[0];
        }

        /// <summary>
        /// Iterates through an event's list of subscribed delegates, and begins to unsubscribe them from the event.
        /// </summary>
        /// <typeparam name="T">The type of the event handler.</typeparam>
        /// <param name="eventHandler">The event handler to unsubscribe the subscribed delegates from.</param>
        public static void Unsubscribe<T>(ref EventHandler<T> eventHandler) where T : EventArgs
        {
            if (eventHandler == null) return;
            Delegate[] subscriptions = eventHandler.GetInvocationList();

            foreach (Delegate subscription in subscriptions)
                eventHandler -= (EventHandler<T>)subscription;
        }

        /// <summary>
        /// Returns a new string that begins where the parent ends in the source.
        /// </summary>
        /// <param name="source">The string that contains the child.</param>
        /// <param name="parent">The string that comes before the child.</param>
        /// <returns></returns>
        public static string GetChild(this string source, string parent)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            int sourceIndex = source
                .IndexOf(parent ?? string.Empty, StringComparison.OrdinalIgnoreCase);

            return sourceIndex >= 0 ?
                source.Substring(sourceIndex + parent.Length) : string.Empty;
        }
        /// <summary>
        /// Returns a new string that ends where the child begins in the source.
        /// </summary>
        /// <param name="source">The string that contains the parent.</param>
        /// <param name="child">The string that comes after the parent.</param>
        /// <returns></returns>
        public static string GetParent(this string source, string child)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            int sourceIndex = source
                .IndexOf(child ?? string.Empty, StringComparison.OrdinalIgnoreCase);

            return sourceIndex >= 0 ?
                source.Remove(sourceIndex) : string.Empty;
        }
        /// <summary>
        /// Returns a new string that is between the delimiters, and the child in the source.
        /// </summary>
        /// <param name="source">The string that contains the parent.</param>
        /// <param name="child">The string that comes after the parent.</param>
        /// <param name="delimiters">The Unicode characters that will be used to split the parent, returning the last split value.</param>
        /// <returns></returns>
        public static string GetParent(this string source, string child, params char[] delimiters)
        {
            string parentSource = source.GetParent(child);
            if (!string.IsNullOrWhiteSpace(parentSource))
            {
                string[] childSplits = parentSource.Split(delimiters,
                    StringSplitOptions.RemoveEmptyEntries);

                return childSplits[childSplits.Length - 1];
            }
            else return string.Empty;
        }
        /// <summary>
        /// Returns a new string that is between the parent, and the delimiters in the source.
        /// </summary>
        /// <param name="source">The string that contains the child.</param>
        /// <param name="parent">The string that comes before the child.</param>
        /// <param name="delimiters">The Unicode characters that will be used to split the child, returning the first split value.</param>
        /// <returns></returns>
        public static string GetChild(this string source, string parent, params char[] delimiters)
        {
            string childSource = source.GetChild(parent);

            return !string.IsNullOrWhiteSpace(childSource) ?
                childSource.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)[0] : string.Empty;
        }
    }
}