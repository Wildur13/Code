import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Utils {
    public static String readLine() throws IOException {
        return new BufferedReader(new InputStreamReader(System.in)).readLine();
    }

    public static int promptInt(String prompt) throws IOException {
        while (true) {
            System.out.print(prompt);
            try {
                return Integer.parseInt(readLine());
            } catch (NumberFormatException e) {
                System.out.println("Not a number!");
            }
        }
    }

    public static int promptIntBetween(String prompt, int min, int max) throws IOException {
        while (true) {
            int input = promptInt(prompt);
            if (input >= min && input <= max) {
                return input;
            } else {
                System.out.println("Out of range (" + min + " to " + max + ")!");
            }
        }
    }

    public static PlayerId promptPlayerId(String prompt) throws IOException {
        while (true) {
            System.out.print(prompt);
            String input = readLine();
            if ("#".equals(input)) return PlayerId.HASH;
            if ("*".equals(input)) return PlayerId.STAR;
            System.out.println("Illegal Input!");
        }
    }

    public static Player promptPlayerType(String prompt) throws IOException {
        while (true) {
            System.out.print(prompt);
            String input = readLine();
            if ("h".equals(input)) return new HumanPlayer();
            if ("c".equals(input)) return new ComputerPlayer();
            System.out.println("Illegal Input!");
        }
    }
}
