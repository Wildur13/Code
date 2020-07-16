import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import java.util.Arrays;
import java.util.Objects;
import java.util.Random;
import java.util.stream.IntStream;
import java.util.stream.Stream;

import static org.junit.jupiter.api.Assertions.*;

public class GameBoardImplTest {
    private static Stream<Arguments> getBoardDimensionsToTest() {
        return IntStream.of(3, 5, 6, 8, 15, 20).boxed().flatMap(columns ->
                IntStream.of(3, 4, 7, 9, 13, 15).mapToObj(rows -> Arguments.of(columns, rows)));
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void getNumColumns_getNumRows(int columns, int rows) {
        GameBoard board = new GameBoardImpl(columns, rows);
        assertEquals(columns, board.getNumColumns(), "Number of columns is wrong!");
        assertEquals(rows, board.getNumRows(), "Number of rows is wrong!");
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void getFieldWhenAllIsEmpty(int columns, int rows) {
        GameBoard board = new GameBoardImpl(columns, rows);
        for (int column = 0; column < columns; column++) {
            for (int row = 0; row < rows; row++) {
                assertNull(board.getField(column, row), "Das Feld an Spalte " + column + " Zeile " + row + " sollte leer sein!");
            }
        }
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void putCoin_getField(int columns, int rows) throws IllegalMoveException {
        GameBoard board = new GameBoardImpl(columns, rows);
        int seed = Objects.hash(columns, rows);

        // First fill up the board with random coins
        Random rand = new Random(seed);
        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                board.putCoin(column, PlayerId.values()[rand.nextInt(2)]);
            }
        }

        // Now verify that each field has the correct coin
        rand = new Random(seed);
        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                assertEquals(PlayerId.values()[rand.nextInt(2)], board.getField(column, row));
            }
        }
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void putCoinNegative(int columns, int rows) {
        GameBoard board = new GameBoardImpl(columns, rows);
        IntStream.of(-1, -2, -100).forEach(column ->
                Arrays.stream(PlayerId.values()).forEach(playerId ->
                        assertThrows(IllegalMoveException.class,
                                () -> board.putCoin(column, playerId)
                        )
                )
        );
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void putCoinOutOfBounds(int columns, int rows) {
        GameBoard board = new GameBoardImpl(columns, rows);
        IntStream.of(0, 1, 99).forEach(offset ->
                Arrays.stream(PlayerId.values()).forEach(playerId ->
                        assertThrows(IllegalMoveException.class,
                                () -> board.putCoin(columns + offset, playerId)
                        )
                )
        );
    }

    @ParameterizedTest
    @MethodSource("getBoardDimensionsToTest")
    void putCoinInFullColumn(int columns, int rows) throws IllegalMoveException {
        GameBoard board = new GameBoardImpl(columns, rows);
        int[] columnSizes = new int[columns];
        Random rand = new Random(Objects.hash(columns, rows));
        for (int i = 0; i < 10 * columns * rows; i++) {
            int column = rand.nextInt(columns);
            if (columnSizes[column] >= rows) {
                assertThrows(IllegalMoveException.class, () -> board.putCoin(column, PlayerId.values()[rand.nextInt(2)]));
            } else {
                board.putCoin(column, PlayerId.values()[rand.nextInt(2)]);
                columnSizes[column]++;
            }
        }
    }
}
