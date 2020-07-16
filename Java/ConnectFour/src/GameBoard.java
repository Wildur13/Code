public interface GameBoard extends ReadOnlyGameBoard {
    /**
     * Put a coin of the given player into the given column.
     *
     * @param column   the column to put the coin into
     * @param playerId the player who should own the coin
     * @throws IllegalMoveException if the column is invalid or full
     */
    void putCoin(int column, PlayerId playerId) throws IllegalMoveException;
}
