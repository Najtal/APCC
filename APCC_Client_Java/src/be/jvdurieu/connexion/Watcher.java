package be.jvdurieu.connexion;

import be.jvdurieu.WatchListener;
import be.jvdurieu.message.MessageParser;
import be.jvdurieu.model.Model;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.util.ArrayList;
import java.util.List;
import java.util.Observable;
import java.util.Observer;

/**
 * Created by jvdur on 02/02/2016.
 */
public class Watcher extends Observable {

    private final Model sModel;
    private Socket socket;

    private BufferedReader in;
    private PrintWriter out;

    private List<WatchListener> listeners = new ArrayList<WatchListener>();

    public static Watcher instance;


    public Watcher(Model sModel, WatchListener obs) {
        this.sModel = sModel;
        this.instance = this;
        this.addListener(obs);

        // Initiate connexion and message receiver
        establishConnexion();

        // Send connexion message to the server
        sendMessage(MessageParser.getConnexionMessage(sModel));
    }

    public void addListener(WatchListener toAdd) {
        listeners.add(toAdd);
    }


    public void update() {
        // Notify everybody that scale level has change
        for (WatchListener wl : listeners)
            wl.adjustCpuUsage(sModel.getScaleLevel());
    }


    private void establishConnexion() {

        try {
            // Make connection and initialize streams
            socket = new Socket("localhost", sModel.getClientPort());

            // Prepare to send data
            out = new PrintWriter(socket.getOutputStream(), true);

            // Instanciate listener
            Thread sl = new ServerListener(this, socket);
            sl.start();

        } catch (IOException ioe) {
            ioe.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void sendMessage(String connexionMessage) {
        out.println(connexionMessage);
    }

    public void setScaleLevel(int scaleLevel) {
        this.sModel.setScaleLevel(scaleLevel);
        System.out.println("Notify observer to level to:" + scaleLevel);
        update();
    }

    public void slowDown() {
        System.out.println("SLOW DOWN");
        if (sModel.getScale() > sModel.getScaleLevel()) {
            sModel.setScaleLevel(sModel.getScaleLevel()+1);
            update();
        }
    }

    public void speedUp() {
        System.out.println("SPEED UP");
        if (sModel.getScaleLevel() > 1) {
            sModel.setScaleLevel(sModel.getScaleLevel()-1);
            update();
        }
    }

    public int getScaleLevel() {
        return sModel.getScaleLevel();
    }
}
