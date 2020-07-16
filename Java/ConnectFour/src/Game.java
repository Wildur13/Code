import java.io.IOException;

public class Game {
    private final Player[] players = new Player[2];
    private final GameBoard board;
    private final int connectToWin;
    private PlayerId currentPlayerId;

    private Game(int columns, int rows, int connectToWin, PlayerId firstPlayerId, Player playerHash, Player playerStar) {
        this.board = new GameBoardImpl(columns, rows);
        this.connectToWin = connectToWin;
        this.currentPlayerId = firstPlayerId;
        this.players[PlayerId.HASH.ordinal()] = playerHash;
        this.players[PlayerId.STAR.ordinal()] = playerStar;
    }

    public static void main(String[] args) throws IOException {
        int columns = Utils.promptIntBetween("Enter the number of columns in the board (default is 7): ", 3, 25);
        int rows = Utils.promptIntBetween("Enter the number of rows in the board (default is 6): ", 3, 20);
        int connectToWin = Utils.promptIntBetween("Enter the number of coins that need to be connected for winning the game (default is 4): ", 3, Math.max(columns, rows));
        PlayerId firstPlayerId = Utils.promptPlayerId("Which Player should start (enter # or *): ");
        Player playerHash = Utils.promptPlayerType("Type of player # (enter h for human or c for computer): ");
        Player playerStar = Utils.promptPlayerType("Type of player * (enter h for human or c for computer): ");
        new Game(columns, rows, connectToWin, firstPlayerId, playerHash, playerStar).start();
    }

    private void start() throws IOException {
        showBoard();
        ReadOnlyGameBoard readOnlyGameBoard = getReadOnlyGameBoard();
        GameOutcome outcome;
        do {
            System.out.println("It's player " + PlayerId.toChar(currentPlayerId) + "'s turn!");
            Player player = players[currentPlayerId.ordinal()];
            while (true) {
                int column = player.chooseColumn(readOnlyGameBoard, currentPlayerId, connectToWin);
                if (!(player instanceof HumanPlayer)) {
                    System.out.println("[" + PlayerId.toChar(currentPlayerId) + "] Computer chose column " + column);
                }
                try {
                    board.putCoin(column, currentPlayerId);
                    break;
                } catch (IllegalMoveException e) {
                    System.out.println("Illegal move: " + e.getMessage());
                }
            }
            showBoard();
            currentPlayerId = PlayerId.inverse(currentPlayerId);
            outcome = GameOutcomeSolver.calculateOutcome(readOnlyGameBoard, connectToWin);
        } while (outcome == GameOutcome.IN_PROGRESS);

        switch (outcome) {
            case TIE:
                System.out.println("The game is a tie!");
                break;
            case WIN_HASH:
                System.out.println("Player # wins!");
                break;
            case WIN_STAR:
                System.out.println("Player * wins!");
                break;
        }
    }

    private void showBoard() {
        System.out.println();
        int columns = board.getNumColumns();
        int rows = board.getNumRows();
        for (int column = 0; column < columns; column++) {
            System.out.print('|');
            if (columns > 10) {
                System.out.printf("%02d", column);
            } else {
                System.out.print(column);
            }
        }
        System.out.println("|");
        for (int row = rows - 1; row >= 0; row--) {
            for (int column = 0; column < columns; column++) {
                System.out.print("|" + PlayerId.toChar(board.getField(column, row)));
                if (columns > 10) {
                    System.out.print(' ');
                }
            }
            System.out.println("|");
        }
        System.out.println("\u203E".repeat((columns > 10 ? 3 : 2) * columns + 1));
        System.out.println();
    }

    private ReadOnlyGameBoard getReadOnlyGameBoard() {
        // Do not hand out the this.board reference such that the consumer cannot downcast and use the putCoin method.
        return new ReadOnlyGameBoard() {
            @Override
            public int getNumColumns() {
                return board.getNumColumns();
            }

            @Override
            public int getNumRows() {
                return board.getNumRows();
            }

            @Override
            public PlayerId getField(int column, int row) {
                return board.getField(column, row);
            }
        };
    }
}
