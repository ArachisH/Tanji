using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Sulakore.Habbo
{
    public static class HExtensions
    {
        private static readonly Random _rng;
        private const BindingFlags BINDINGS = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        static HExtensions()
        {
            _rng = new Random();
        }

        public static HSign GetRandomSign()
        {
            return (HSign)_rng.Next(0, 19);
        }
        public static HTheme GetRandomTheme()
        {
            return (HTheme)((_rng.Next(1, 7) + 2) & 7);
        }

        public static HDirection ToLeft(this HDirection facing)
        {
            return (HDirection)(((int)facing - 1) & 7);
        }
        public static HDirection ToRight(this HDirection facing)
        {
            return (HDirection)(((int)facing + 1) & 7);
        }

        public static Uri ToUri(this HHotel hotel, string subdomain = "www")
        {
            return new Uri($"https://{subdomain}.habbo.{hotel.ToDomain()}");
        }
        public static string ToDomain(this HHotel hotel)
        {
            if (hotel == HHotel.Unknown)
            {
                throw new ArgumentException("Hotel cannot be 'Unknown'.", nameof(hotel));
            }
            string value = hotel.ToString().ToLower();
            return (value.Length != 5 ? value : value.Insert(3, "."));
        }

        public static IEnumerable<MemberInfo> GetAllMembers(this Type type)
        {
            return type.Excavate(t => t.GetMembers(BINDINGS));
        }
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            return type.Excavate(t => t.GetMethods(BINDINGS));
        }
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            return type.Excavate(t => t.GetProperties(BINDINGS));
        }
        public static IEnumerable<T> Excavate<T>(this Type type, Func<Type, IEnumerable<T>> excavator)
        {
            IEnumerable<T> excavated = null;
            while (type != null)
            {
                IEnumerable<T> batch = excavator(type);
                excavated = (excavated?.Concat(batch) ?? batch);
                type = type.BaseType;
            }
            return excavated;
        }
    }
}