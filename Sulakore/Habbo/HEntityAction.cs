using System;
using System.Globalization;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Habbo
{
    /// <summary>
    /// Represents an <see cref="IHEntity"/> performing actions.
    /// </summary>
    public class HEntityAction : IHEntity
    {
        /// <summary>
        /// Gets or sets the room index value of the <see cref="IHEntity"/>.
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Gets or sets a value that determines whether the <see cref="IHEntity"/> has privileges.
        /// </summary>
        public bool IsEmpowered { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HPoint"/> of where the <see cref="IHEntity"/> is currently on.
        /// </summary>
        public HPoint Tile { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="HPoint"/> of where the <see cref="IHEntity"/> will move to next.
        /// </summary>
        public HPoint MovingTo { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HSign"/> that the <see cref="IHEntity"/> has raised.
        /// </summary>
        public HSign Sign { get; set; }
        /// <summary>
        /// Gets or sets the current <see cref="HStance"/> of the <see cref="IHEntity"/>.
        /// </summary>
        public HStance Stance { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="HAction"/> that the <see cref="IHEntity"/> has recently done.
        /// </summary>
        public HAction LastAction { get; set; }
        /// <summary>
        /// Gets or sets the head <see cref="HDirection"/> of the <see cref="IHEntity"/>.
        /// </summary>
        public HDirection HeadDirection { get; set; }
        /// <summary>
        /// Gets or sets the body <see cref="HDirection"/> of the <see cref="IHEntity"/>.
        /// </summary>
        public HDirection BodyDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HEntityAction"/> class with the specified information.
        /// </summary>
        /// <param name="isEmpowered">The value that determines whether the <see cref="IHEntity"/> has privileges.</param>
        /// <param name="index">The room index value of the <see cref="IHEntity"/>.</param>
        /// <param name="tile">The <see cref="HPoint"/> of where the <see cref="IHEntity"/> is currently on.</param>
        /// <param name="movingTo">The <see cref="HPoint"/> of where the <see cref="IHEntity"/> will move to next.</param>
        /// <param name="sign">The <see cref="HSign"/> that the <see cref="IHEntity"/> has raised.</param>
        /// <param name="stance">The current <see cref="HStance"/> of the <see cref="IHEntity"/>.</param>
        /// <param name="headDirection">The <see cref="HDirection"/> of the <see cref="IHEntity"/>'s head.</param>
        /// <param name="bodyDirection">The <see cref="HDirection"/> of the <see cref="IHEntity"/>'s body.</param>
        /// <param name="lastAction">The <see cref="HAction"/> that the <see cref="IHEntity"/> has recently done.</param>
        public HEntityAction(bool isEmpowered, int index, HPoint tile, HPoint movingTo,
            HSign sign, HStance stance, HDirection headDirection, HDirection bodyDirection, HAction lastAction)
        {
            Index = index;
            IsEmpowered = isEmpowered;

            Tile = tile;
            MovingTo = movingTo;

            Sign = sign;
            Stance = stance;

            HeadDirection = headDirection;
            BodyDirection = bodyDirection;

            LastAction = lastAction;
        }

        /// <summary>
        /// Returns a <see cref="IReadOnlyList{T}"/> of type <see cref="HEntityAction"/> found in the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The <see cref="HMessage"/> that contains the <see cref="HEntityAction"/> data to parse.</param>
        /// <returns></returns>
        public static IReadOnlyList<HEntityAction> Parse(HMessage packet)
        {
            int entityActionCount = packet.ReadInteger();
            var entityActionList = new List<HEntityAction>(entityActionCount);

            for (int i = 0; i < entityActionList.Capacity; i++)
            {
                int index = packet.ReadInteger();
                int x = packet.ReadInteger();
                int y = packet.ReadInteger();
                var z = double.Parse(packet.ReadString(), CultureInfo.InvariantCulture);

                var headDirection = (HDirection)packet.ReadInteger();
                var bodyDirection = (HDirection)packet.ReadInteger();

                string actionString = packet.ReadString();
                string[] actionData = actionString.Split(new[] { '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                HSign sign = HSign.One;
                HAction action = HAction.None;
                HStance stance = HStance.Stand;

                double movingToZ = 0.0;
                bool isEmpowered = false;
                int movingToX = 0, movingToY = 0;

                foreach (string actionInfo in actionData)
                {
                    string[] actionValues = actionInfo.Split(' ');

                    if (actionValues.Length < 2) continue;
                    if (string.IsNullOrWhiteSpace(actionValues[0])) continue;
                    #region Switch: actionValues
                    switch (actionValues[0])
                    {
                        case "flatctrl":
                        {
                            isEmpowered = true;
                            break;
                        }
                        case "mv":
                        {
                            string[] movingToValues = actionValues[1].Split(',');
                            if (movingToValues.Length >= 3)
                            {
                                movingToX = int.Parse(movingToValues[0]);
                                movingToY = int.Parse(movingToValues[1]);
                                movingToZ = double.Parse(movingToValues[2], CultureInfo.InvariantCulture);
                            }
                            action = HAction.Move;
                            break;
                        }
                        case "sit":
                        {
                            action = HAction.Sit;
                            stance = HStance.Sit;
                            break;
                        }
                        case "lay":
                        {
                            action = HAction.Lay;
                            stance = HStance.Lay;
                            break;
                        }
                        case "sign":
                        {
                            sign = (HSign)int.Parse(actionValues[1]);
                            action = HAction.Sign;
                            break;
                        }
                    }
                    #endregion
                }

                var entityAction = new HEntityAction(isEmpowered, index, new HPoint(x, y, z),
                    new HPoint(movingToX, movingToY, movingToZ), sign, stance, headDirection, bodyDirection, action);

                entityActionList.Add(entityAction);
            }
            return entityActionList;
        }

        /// <summary>
        /// Converts the <see cref="HEntityAction"/> to a human-readable string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(IsEmpowered)}: {IsEmpowered}, {nameof(Index)}: {Index}, {nameof(Tile)}: {Tile}, " +
                $"{nameof(MovingTo)}: {MovingTo}, {nameof(HeadDirection)}: {HeadDirection}, " +
                $"{nameof(BodyDirection)}: {BodyDirection}, {nameof(LastAction)}: {LastAction}";
        }
    }
}