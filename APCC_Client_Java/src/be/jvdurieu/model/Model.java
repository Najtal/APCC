package be.jvdurieu.model;

/**
 * Created by jvdur on 02/02/2016.
 */
public class Model {


    private final boolean isRealTime;
    private final int clientPort;
    private final int serverPort;
    private final int priority;
    private final int scale;

    private int scaleLevel;
    private String processName;
    private String processDescrition;


    /**
     * Constructor
     * @param clientPort the connexion clientPort
     * @param priority the priority of the client
     * @param scale the scale set
     * @param isRealTime let the server know if the client is a real-time app
     */
    public Model(int clientPort, int serverPort, int priority, int scale, boolean isRealTime, String processName, String processDescription) {
        this.clientPort = clientPort;
        this.serverPort = serverPort;
        this.priority = priority;
        this.scale = scale;
        this.isRealTime = isRealTime;
        this.processName = processName;
        this.processDescrition = processDescription;
    }



    /*
     *  GETTERS
     */
    public int getScale() {
        return scale;
    }

    public int getPriority() {
        return priority;
    }

    public int getClientPort() {
        return clientPort;
    }

    public boolean isRealTime() {
        return isRealTime;
    }

    public int getScaleLevel() {
        return scaleLevel;
    }

    public String getProcessName() {
        return processName;
    }

    public String getProcessDescrition() {
        return processDescrition;
    }

    /*
     *  SETTERS
     */
    public void setScaleLevel(int scaleLevel) {
        this.scaleLevel = scaleLevel;
    }

}

