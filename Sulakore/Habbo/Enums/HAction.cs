namespace Sulakore.Habbo
{
    /// <summary>
    /// Specifies a set of actions a player can perform in-game
    /// </summary>
    public enum HAction
    {
        /// <summary>
        /// Represents a player not performing any actions.
        /// </summary>
        None = 0,
        /// <summary>
        /// Represents a moving player.
        /// </summary>
        Move = 1,
        /// <summary>
        /// Represents a player that has sat down.
        /// </summary>
        Sit = 2,
        /// <summary>
        /// Represents a player that has laid down.
        /// </summary>
        Lay = 3,
        /// <summary>
        /// Represents a player holding up a <see cref="HSign"/>.
        /// </summary>
        Sign = 4
    }
}