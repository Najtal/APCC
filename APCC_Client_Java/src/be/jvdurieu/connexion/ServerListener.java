package be.jvdurieu.connexion;

import be.jvdurieu.message.MessageParser;
import sun.rmi.runtime.Log;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.Date;

/**
 * Created by jvdur on 02/02/2016.
 */
public class ServerListener extends Thread {

    private final Watcher watcher;
    private final Socket socket;
    InputStream is = null;

    public ServerListener(Watcher watcher, Socket socket) throws Exception {
        this.watcher = watcher;
        this.socket = socket;
    }

    public void run(){

        ByteBuffer buf = ByteBuffer.allocate(2048);

        while (true) {

            //read from socket
            try {

                is = socket.getInputStream(); //open socket streams

                if (is.available() > 0 && buf.remaining() > 0) {
                    while (is.available() > 0 && buf.remaining() > 0)
                        buf.put((byte) is.read());
                } else if (buf.position() > 0) {
                    String msg = new String(buf.array(), "UTF-8");
                    MessageParser.parseMessage(watcher, msg);
                    buf.clear();
                } else {
                    buf.clear();
                }
            } catch (IOException e) {
                e.printStackTrace();
            }

        }

    }


}
