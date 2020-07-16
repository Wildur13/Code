public enum GameOutcome {
    IN_PROGRESS, WIN_HASH, WIN_STAR, TIE;

    public static GameOutcome winOf(PlayerId player) {
        if (player == null) throw new NullPointerException();
        return player == PlayerId.HASH ? WIN_HASH : WIN_STAR;
    }
}
