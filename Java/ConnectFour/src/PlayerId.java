public enum PlayerId {
    HASH, STAR;

    public static char toChar(PlayerId player) {
        if (player == HASH) return '#';
        if (player == STAR) return '*';
        return ' ';
    }

    public static PlayerId inverse(PlayerId player) {
        if (player == null) throw new NullPointerException();
        return player == HASH ? STAR : HASH;
    }
}
