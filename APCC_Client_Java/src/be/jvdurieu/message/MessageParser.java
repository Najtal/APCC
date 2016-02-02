package be.jvdurieu.message;

import be.jvdurieu.connexion.Watcher;
import be.jvdurieu.model.Model;

/**
 * Created by jvdur on 02/02/2016.
 */
public class MessageParser {

    public static void parseMessage(Watcher watcher, String message) {

        //String employee = "Smith,Katie,3014,,8.25,6.5,,,10.75,8.5";
        String[] tokens = message.split("\\[");
        tokens = tokens[1].split("\\]");
        tokens = tokens[0].split(",");

        if (tokens[0].equals("action")) {
            if (tokens[1].equals("setScale")) {
                int newLevel = Integer.parseInt(tokens[2]);
                watcher.setScaleLevel(newLevel);
            } else if (tokens[1].equals("speedup")) {
                watcher.speedUp();
            } else if (tokens[1].equals("slowdown")) {
                watcher.slowDown();
            }
        }

    }

    public static String getConnexionMessage(Model model) {
        String connexionMessage = "sub;";
        connexionMessage += (model.isRealTime()) ? "1" : "0";
        connexionMessage += ";" + (model.getPriority() + ";"
                    + model.getScale() + ";"
                    + model.getProcessName() + ";"
                    + model.getProcessDescrition());
        System.out.println("Message de connexion: " + connexionMessage);
        return connexionMessage;
    }
}
    