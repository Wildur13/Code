public interface ReadOnlyGameBoard {
    /**
     * @return the number of columns in the game board
     */
    int getNumColumns();

    /**
     * @return the number of rows in the game board
     */
    int getNumRows();

    /**
     * Get the player who owns the given field.
     *
     * @param column the column (counted from left to right) of the field to check
     * @param row    the row (counted from bottom to top) of the field to check
     * @return the player who owns the field, or null if the field is empty
     */
    PlayerId getField(int column, int row);
}
