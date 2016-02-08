package be.jvdurieu;

import be.jvdurieu.connexion.Watcher;
import be.jvdurieu.model.Model;

import java.util.Observer;

/**
 * Created by jvdur on 02/02/2016.
 */
public class Apcc {

    private static Apcc instance;
    private static Watcher sWatcher;
    private static Model sModel;

    static {
         instance = new Apcc();
    }

    private Model model;

    /**
     * Hidden Constructeur
     */
    private Apcc() {
    }

    public Watcher
    Init(int clientPort, int serverPort, int priority, int scale, boolean isRealTime, String processName, String processDescription, WatchListener obs) {

        if (clientPort < 0 || clientPort > 16000) throw new IllegalArgumentException("[APCC][ERROR] Invalid priority: must be between 1 and 12000");
        if (priority > 3 || priority < 1) throw new IllegalArgumentException("[APCC][ERROR] Invalid priority: must be between 1 and 3");
        if (scale < 2) throw  new IllegalArgumentException("[APCC][ERROR] Scale must be greater than 1");

        sModel = new Model(clientPort, serverPort, priority, scale, isRealTime, processName, processDescription);
        sWatcher = new Watcher(sModel, obs);
        return sWatcher;
    }

    public static Apcc getInstance() {
        return instance;
    }

    public Watcher getWatcher() {
        return sWatcher;
    }


}
