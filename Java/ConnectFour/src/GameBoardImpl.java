public class GameBoardImpl implements GameBoard {
    int spalte;
    int zeile;
    PlayerId[][] players = null;

    public GameBoardImpl(int columns, int rows) {
        this.spalte = columns;
        this.zeile = rows;
        this.players = new PlayerId[columns][rows];
    }

    @Override
    public int getNumColumns() {
        return spalte;
    }

    @Override
    public int getNumRows() {
        return zeile;
    }

    @Override
    public PlayerId getField(int column, int row) {
        return players[column][row];
    }

    @Override
    public void putCoin(int column, PlayerId playerId) throws IllegalMoveException {
        if (column < 0 || column >= spalte) {
            throw new IllegalMoveException("");
        }
        else for(int i=0; i<zeile;i++) {
            if (players[column][i]== null){
                players[column][i]= playerId;
                break;
            } else if (i== zeile-1){
                throw new IllegalMoveException("");
            }
        }
    }
    }
