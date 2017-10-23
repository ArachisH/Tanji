namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents an in-game object that provides a unique identifier relative to the room.
    /// </summary>
    public interface IHEntity
    {
        /// <summary>
        /// Gets or sets the room index value of the <see cref="IHEntity"/>.
        /// </summary>
        int Index { get; }
    }
}