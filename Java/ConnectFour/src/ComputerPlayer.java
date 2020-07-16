import java.util.Random;

public class ComputerPlayer implements Player {
    private final Random rand = new Random();

    @Override
    public int chooseColumn(ReadOnlyGameBoard board, PlayerId currentPlayerId, int connectToWin) {
        return rand.nextInt(board.getNumColumns());
    }
}
