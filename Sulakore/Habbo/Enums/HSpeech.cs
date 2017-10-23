namespace Sulakore.Habbo
{
    /// <summary>
    /// Specifies the different types of speech modes that players can communicate with each other in-game.
    /// </summary>
    public enum HSpeech
    {
        /// <summary>
        /// Represents a speech mode that makes the message publicly visible within a determined range in the room.
        /// </summary>
        Say = 0,
        /// <summary>
        /// Represents a speech mode that makes the message public to everyone in the room.
        /// </summary>
        Shout = 1,
        /// <summary>
        /// Represents a speech mode that only makes the message visible to the specified target regardless of the range.
        /// </summary>
        Whisper = 2
    }
}