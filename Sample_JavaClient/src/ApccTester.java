import be.jvdurieu.Apcc;
import be.jvdurieu.WatchListener;
import be.jvdurieu.connexion.Watcher;

/**
 * Created by jvdur on 02/02/2016.
 */
public class ApccTester implements WatchListener {

    private Apcc apcc;
    private Watcher watcher;

    public static void main(String[] args) {
        new ApccTester().test();
    }


    public void test() {
        // Instanciate
        apcc = Apcc.getInstance();
        watcher = apcc.Init(13000, 9999, 2, 6, false, "Da bling bling test prog !", "Woop woop !", this);
    }

    @Override
    public void adjustCpuUsage(int scaleLevel) {
        System.out.println(">>>>>>>>>>>>>>> TESTER : " + scaleLevel);
    }
}
