using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Race
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public delegate List<Car> GameDelegate(ProgressBar pr1, ProgressBar pr2, ProgressBar pr3);
    public delegate int StepDelegate(Car car, ProgressBar pr);
    public delegate void ChangeValuePBDelegate(ProgressBar pr, int value, int speed);
    public delegate void FinishColorDelegate(ProgressBar pr);
    public delegate void MoneyChangeDelegate();

    public partial class MainWindow : Window
    {
        int forRand = 1;
        int money = 1000;
        int rateMoney = 0;
        int rateNum;
        int currentPlace = 1;
        int colorNum = 0;
        public GameDelegate gameDelegate;
        public ChangeValuePBDelegate myDelegate;
        public FinishColorDelegate finishColorDelegate;
        public MoneyChangeDelegate moneyChangeDelegate;
        bool gameProc = false;
        MainWindow mainWindow;
        Brush[] brushes = new Brush[3];

        public MainWindow()
        {
            InitializeComponent();

            cBox.Items.Add("1");
            cBox.Items.Add("2");
            cBox.Items.Add("3");

            gameDelegate = Game;
            myDelegate = ChangeValueProgressBar;
            finishColorDelegate = FinishColor;
            moneyChangeDelegate = MoneyChange;

            label.Content = money.ToString();
            mainWindow = this;
            brushes[0] = Brushes.Green;
            brushes[1] = Brushes.Yellow;
            brushes[2] = Brushes.Red;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!gameProc && int.TryParse(rateTB.Text, out rateMoney) && int.TryParse((string)cBox.SelectedItem, out rateNum))
            {
                //rateMoney = int.Parse(rateTB.Text);
                //rateNum = int.Parse((string)cBox.SelectedItem);

                lc1.Background = null;
                lc2.Background = null;
                lc3.Background = null;
                gameProc = true;
                gameDelegate.BeginInvoke(pr1, pr2, pr3, CallbackMethod, gameDelegate);
                
                Console.WriteLine("kykykykyky");
            }
        }

        public List<Car> Game(ProgressBar pr1, ProgressBar pr2, ProgressBar pr3)
        {
            Console.WriteLine("ky");
            List<Car> cars = new List<Car>();

            StepDelegate stepDelegate1;
            StepDelegate stepDelegate2;
            StepDelegate stepDelegate3;

            cars.Add(new Car(225));
            cars.Add(new Car(225));
            cars.Add(new Car(210));

            stepDelegate1 = Steps;
            stepDelegate2 = Steps;
            stepDelegate3 = Steps;

            IAsyncResult result1 = stepDelegate1.BeginInvoke(cars[0], pr1, null, null);
            IAsyncResult result2 = stepDelegate2.BeginInvoke(cars[1], pr2, null, null);
            IAsyncResult result3 = stepDelegate3.BeginInvoke(cars[2], pr3, null, null);

            int _0 = stepDelegate1.EndInvoke(result1);
            int _1 = stepDelegate2.EndInvoke(result2);
            int _2 = stepDelegate3.EndInvoke(result3);

            Console.WriteLine(string.Format("{0} - {1} - {2}", _0, _1, _2));

            return cars;
        }

        public int Steps(Car car, ProgressBar pr)
        {
            Console.WriteLine("kyky");
            Random rand = new Random(forRand++);
            int speed;
            int stepsCount = 0;
            while (car.Distance < 100)
            {
                speed = rand.Next() % 20;

                if (rand.Next() % 2 == 0)
                    car.Speed = car.MaxSpeed + speed;
                else
                    car.Speed = car.MaxSpeed - speed;

                Console.WriteLine(string.Format("{0} {1}", car.Speed.ToString(), car.MaxSpeed.ToString()));

                car.Distance += (double)((double)car.Speed / 120);

                int toPrBar = (int)car.Distance;

                object[] objs = new object[3];
                objs[0] = pr;
                objs[1] = toPrBar;
                objs[2] = car.Speed;

                pr.Dispatcher.Invoke(mainWindow.myDelegate, objs);

                stepsCount++;

                Thread.Sleep(100);
            }

            car.Place = currentPlace++;

            pr.Dispatcher.Invoke(mainWindow.finishColorDelegate, pr);
            Console.WriteLine("kykyky");
            return stepsCount;
        }

        void ChangeValueProgressBar(ProgressBar pr, int val, int speed)
        {
            pr.Value = val;
            if (pr.Name == "pr1")
                l1.Content = speed.ToString();
            if (pr.Name == "pr2")
                l2.Content = speed.ToString();
            if (pr.Name == "pr3")
                l3.Content = speed.ToString();
        }

        void FinishColor(ProgressBar pr)
        {
            if (pr.Name == "pr1")
                lc1.Background = brushes[colorNum];
            if (pr.Name == "pr2")
                lc2.Background = brushes[colorNum];
            if (pr.Name == "pr3")
                lc3.Background = brushes[colorNum];
            colorNum++;
        }

        void MoneyChange()
        {
            label.Content = money.ToString();
        }

        void CallbackMethod(IAsyncResult ar)
        {
            GameDelegate del = (GameDelegate)ar.AsyncState;
            if (del != null)
            {
                List<Car> cars = del.EndInvoke(ar);
                Console.WriteLine(cars[0].Place.ToString());
                Console.WriteLine(cars[1].Place.ToString());
                Console.WriteLine(cars[2].Place.ToString());

                if (cars[rateNum - 1].Place == 1)
                    money += rateMoney;
                else
                    money -= rateMoney;

                label.Dispatcher.Invoke(mainWindow.moneyChangeDelegate, null);

                gameProc = false;
                colorNum = 0;
                currentPlace = 1;
            }
        }

    }


}
