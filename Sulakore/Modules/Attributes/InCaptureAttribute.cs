namespace Sulakore.Modules
{
    public sealed class InDataCaptureAttribute : DataCaptureAttribute
    {
        public override bool IsOutgoing => false;

        public InDataCaptureAttribute(ushort id)
            : base(id)
        { }
        public InDataCaptureAttribute(string hash)
            : base(hash)
        { }
    }
}