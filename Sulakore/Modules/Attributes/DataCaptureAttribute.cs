using System;
using System.Reflection;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Sulakore.Modules
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class DataCaptureAttribute : Attribute
    {
        public ushort? Id { get; }
        public string Identifier { get; }
        public abstract bool IsOutgoing { get; }

        public object Target { get; set; }
        public MethodInfo Method { get; set; }

        public DataCaptureAttribute(ushort id)
        {
            Id = id;
        }
        public DataCaptureAttribute(string identifier)
        {
            Identifier = identifier;
        }

        public void Invoke(DataInterceptedEventArgs args)
        {
            object[] parameters = CreateValues(args);
            object result = Method?.Invoke(Target, parameters);

            switch (result)
            {
                case bool isBlocked:
                {
                    args.IsBlocked = isBlocked;
                    break;
                }
                case HMessage packet:
                {
                    args.Packet = packet;
                    break;
                }
                case object[] chunks:
                {
                    HDestination destination = args.Packet.Destination;
                    args.Packet = new HMessage(args.Packet.Header, chunks);
                    args.Packet.Destination = destination;
                    break;
                }
            }
        }
        private object[] CreateValues(DataInterceptedEventArgs args)
        {
            ParameterInfo[] parameters = Method.GetParameters();
            var values = new object[parameters.Length];

            int position = 0;
            for (int i = 0; i < values.Length; i++)
            {
                ParameterInfo parameter = parameters[i];
                switch (Type.GetTypeCode(parameter.ParameterType))
                {
                    case TypeCode.UInt16:
                    {
                        if (parameter.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                        {
                            values[i] = args.Packet.Header;
                        }
                        else
                        {
                            values[i] = args.Packet.ReadShort(ref position);
                        }
                        break;
                    }

                    case TypeCode.Int32:
                    values[i] = args.Packet.ReadInteger(ref position);
                    break;

                    case TypeCode.Boolean:
                    values[i] = args.Packet.ReadBoolean(ref position);
                    break;

                    case TypeCode.Byte:
                    values[i] = args.Packet.ReadBytes(1, ref position)[0];
                    break;

                    case TypeCode.String:
                    values[i] = args.Packet.ReadString(ref position);
                    break;

                    case TypeCode.Object:
                    {
                        if (parameter.ParameterType == typeof(DataInterceptedEventArgs))
                        {
                            values[i] = args;
                        }
                        else if (parameter.ParameterType == typeof(byte[]))
                        {
                            int length = args.Packet.ReadInteger(ref position);
                            values[i] = args.Packet.ReadBytes(length, ref position);
                        }
                        break;
                    }
                }
            }
            return values;
        }

        public bool Equals(DataCaptureAttribute attribute)
        {
            if (Id != attribute.Id) return false;
            if (Identifier != attribute.Identifier) return false;
            if (!Method.Equals(attribute.Method)) return false;
            return true;
        }
    }
}