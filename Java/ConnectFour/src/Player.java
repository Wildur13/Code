import java.io.IOException;

public interface Player {
    int chooseColumn(ReadOnlyGameBoard board, PlayerId currentPlayerId, int connectToWin) throws IOException;
}
