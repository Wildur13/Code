public class GameOutcomeSolver {
    public static GameOutcome calculateOutcome(ReadOnlyGameBoard board, int connectToWin) {
        // In der Vorlage behaupten wir einfach immer, dass das Spiel noch im Gange ist.
        // Damit ist das Spiel schon bneutzbar, nachdem Aufgabenteil a) gelöst wurde.
        // Diese Zeile müssen Sie löschen und durch Ihre Lösung für c) ersetzen.

        int giveCol = board.getNumColumns();
        int giveRow = board.getNumRows();

        // Wir überprüfen einen möglichen Gewinn bei den Spalten
        int n = 0;
        while (n < giveCol) {
            for (int i = 0; i < giveRow; i++) {
                PlayerId give = board.getField(n, i);
                int sum = (give != null ? 1 : 0);

                if (sum == 0)
                    break;

                int rest = connectToWin - 1; // Anzahl der verbleibenden Züge zum Gewinn für den daran spielenden Spieler

                if (n + rest < giveCol) {
                    for (int m = n + 1; m < n + rest + 1; m++) {

                        if (board.getField(m, i) == null)
                            break;

                        if (board.getField(m, i) == give) {
                            sum++;
                        } else {
                            break;
                        }
                    }
                }

                if (sum == connectToWin)
                    return GameOutcome.winOf(give);

                // Wir überprüfen einen möglichen Gewinn bei den Zeilen
                sum = 1;
                if (i + rest < giveRow) {
                    for (int k = i + 1; k < i + rest + 1; k++) {
                        if (board.getField(n, k) == null)
                            break;

                        if (board.getField(n, k) == give) {
                            sum++;
                        } else {
                            break;
                        }
                    }
                }

                if (sum == connectToWin)
                    return GameOutcome.winOf(give);

                // Wir überprüfen jetzt einen möglichen Gewinn bei den Diagonalen:

                // hier in der rechten Richtung der Diagonale
                sum = 1;
                int rDiagCol = n + 1;
                int rDiagRow = i + 1;
                while (rDiagCol < giveCol && rDiagRow < giveRow) {
                    if (board.getField(rDiagCol, rDiagRow) == null)
                        break;

                    if (board.getField(rDiagCol, rDiagRow) == give) {
                        sum++;
                    } else {
                        break;
                    }
                    if (sum == connectToWin)
                        return GameOutcome.winOf(give);
                    rDiagCol++;
                    rDiagRow++;
                }

                // hier in der linken Richtung der Diagonale
                sum = 1;
                int giveDiagCol = n - 1;
                int giveDiagRow = i + 1;
                while (giveDiagCol >= 0 && giveDiagRow < giveRow) {
                    if (board.getField(giveDiagCol, giveDiagRow) == null)
                        break;

                    if (board.getField(giveDiagCol, giveDiagRow) == give) {
                        sum++;
                    } else {
                        break;
                    }
                    if (sum == connectToWin)
                        return GameOutcome.winOf(give);
                    giveDiagCol--;
                    giveDiagRow++;
                }

            }
            n++;
        }
        // Wir überprüfen, ob der Spiel noch in Progress ist
        // Sonst heißt es ,dass das Spiel ohne Gewinner beendet wurde
        for (int i = 0; i < giveRow; i++) {
            for (int k = 0; k < giveCol; k++) {
                if (board.getField(k, i) == null) {
                    return GameOutcome.IN_PROGRESS;
                }
            }
        }
        return GameOutcome.TIE;

        // Denken Sie daran, Ihren Code zu kommentieren! (Deutsch oder Englisch)
    }
}
