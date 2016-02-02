package be.jvdurieu.test;

import be.jvdurieu.Apcc;
import be.jvdurieu.WatchListener;
import be.jvdurieu.connexion.Watcher;

/**
 * Created by jvdur on 02/02/2016.
 */
public class Tester implements WatchListener {

    private Apcc apcc;
    private Watcher watcher;

    public static void main(String[] args) {
        new Tester().test();
    }


    public void test() {
        // Instanciate
        apcc = Apcc.getInstance();
        watcher = apcc.Init(13000, 9999, 2, 6, false, "yo no sé", "da description !", this);
    }


    @Override
    public void adjustCpuUsage(int scaleLevel) {
        System.out.println(">>>>>>>>>>>>>>> TESTER : " + scaleLevel);
    }
}
