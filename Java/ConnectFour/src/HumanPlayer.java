import java.io.IOException;

public class HumanPlayer implements Player {
    @Override
    public int chooseColumn(ReadOnlyGameBoard board, PlayerId currentPlayerId, int connectToWin) throws IOException {
        return Utils.promptInt("[" + PlayerId.toChar(currentPlayerId) + "] Which column do you want to play? ");
    }
}
