using BuisnessLayer.Interface;
using BuisnessLayer.Services;
using Speaker.leison.Business_layer.Interface;
using Speaker.leison.Business_layer.Services;
using Speaker.leison.Kontext;
using System;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Security.AccessControl;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Speaker.leison.Forms
{
    public partial class Main : Form
    {
        private readonly Isound makeSound;
        private readonly IUdp udp;
        private readonly Ichanell channels;
        private readonly IInfo info;
        private readonly ITranscoder transcoder;
        private readonly SpeakerDb db;
        public Main()
        {
            makeSound = new SounServices();
            udp = new UdpServices();
            channels = new ChanellServices();
            info= new InfoServices();
            transcoder = new TranscoderServices();
            db= new SpeakerDb();    
            InitializeComponent();
        }

        private   void Main_Load(object sender, EventArgs e)
        {
            var listsongs = new List<string>()
            {
                "C:\\Users\\musics\\Song1.wav",
                "C:\\Users\\musics\\song2.wav"
            };
            Random rand = new Random();
        mods:
            try
            {
                

                Console.WriteLine("Version V1.8.2");
                Visible = false;
                int count = 0;
                int countsayelse = 0;
                while (true)
                {
                   
                    MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
                    MMDevice defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                    float level = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
                    DateTime currentTime = DateTime.Now;

                    Console.WriteLine( currentTime.Hour);
                    if (currentTime!=null&&currentTime.Hour >= 10 && currentTime.Hour <= 21)
                    {
                        countsayelse = 0;
                        if (level <= 74)
                        {
                            makeSound.SpeakNow("ყურადღება,გადავდივარ დღის რეჟიმზე");
                            count++;
                            makeSound.SpeakNow("მე ვარ ნათია ჯანდაგიშვილი, იმისათვის, რომ ვიმუშაო  სრულყოფილად, მესაჭიროება ვისაუბრო ხმამაღლა, გხოვთ ნუ შემიზღუდავთ უფლებებს, ნუ დაუწევთ ხმას ან  ნუ გამორთავთ დინამიკს, მადლობა ყურადღებისთვის.");
                            makeSound.SpeakNow("ვაყენებ ხმას ჩემთვის მისაღებ ნორმაზე");
                            defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = 0.75f;
                            if (count >= 3)
                            {
                                makeSound.SpeakNow($"მე საშინლად გაბრაზებული ვარ,  რომ არ ითვალისწინებ ჩემს თხოვნას. უკვე {count}-ჯერ დაუწიე ხმას , გთხოვ სამუშაო საათებში ნუ დაუწევ ხმას.");
                                count = 0;
                            }
                        }
                    }
                    else
                    {
                        if (countsayelse==0||level >= 20 || level <= 10)
                        {
                            makeSound.SpeakNow("ღამის საათებში. ხმას ვაყენებ შედარებით  დაბალზე, იყავით ყურადღებით. მადლობა.");
                            defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = 0.15f;
                            count = 0;
                            countsayelse++;
                        }
                    }
                    try
                    {
                        var getallarms = udp.Receive();
                        var ports = getallarms.Split(',');
                        if (ports.Length > 18)
                        {
                            makeSound.SpeakNow($"ყურადღება! ყურადღება! გაგვეთიშა : {ports.Length} არხი. ვრთავ განგაშის სიგნალს.");
                            SoundPlayer player = new SoundPlayer("C:\\Users\\marjanishvili\\source\\repos\\Speaker.RobotForGLobal-master\\AlarmsAndSounds\\war-alarm-fx_132bpm.wav");
                            try
                            {
                                player.Play();
                                Thread.Sleep(8000);
                                player.Play();
                                Thread.Sleep(8000);
                                player.Play();
                                Thread.Sleep(8000);
                                player.Play();
                                Thread.Sleep(8000);
                                player.Play();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("An error occurred: " + ex.Message);
                            }
                            finally
                            {
                                player.Dispose();
                            }

                        }
                        else
                        {
                            foreach (var port in ports)
                            {

                                if (port.Contains("200"))
                                {
                                    var splited = port.Split('-');

                                    if (splited.Length > 1)
                                    {
                                        int card = int.Parse(splited[1]);
                                        int porti = int.Parse(splited[2]);
                                        var chanell = db.Transcoders.Where(io => io.Card == card && io.Port == porti).FirstOrDefault();
                                        if (chanell != null)
                                        {
                                            var chan = db.Chanells.Where(io => io.Id == chanell.ChanellId).FirstOrDefault();
                                            if (chan != null && chan.Name != null)
                                            {
                                                makeSound.SpeakNow($"ორასში {chan.Name}-ზე გვქონდა ვიდეო  გადასატვირთი და გადავტვირთე , ქარდ{card}. პორტ {porti}.");

                                            }
                                        }
                                        else
                                        {
                                            makeSound.SpeakNow($"ორასში გვქონდა ვიდეო  გადასატვირთი და გადავტვირთე , ქარდ{card}. პორტ {porti}.");
                                        }
                                    }

                                }
                                else
                                {
                                    int portparsed;
                                    bool res = int.TryParse(port, out portparsed);
                                    if (res)
                                    {
                                        Console.WriteLine(portparsed);
                                        if (portparsed == 1500)
                                        {
                                            makeSound.SpeakNow("შუადღე მშვიდობისა , გისურვებთ ბედნიერ დღეს! ");
                                        }
                                        else
                                        if (portparsed == 2000)
                                        {
                                            makeSound.SpeakNow("საღამო მშვიდობისა! ");
                                            makeSound.SpeakNow("ავიმაღლოთ განწყობა");
                                            SoundPlayer player1 = new SoundPlayer(listsongs[rand.Next(0,listsongs.Count)]);
                                            try
                                            {
                                                player1.Play();
                                                Thread.Sleep(10000);
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine("Shecdoma");
                                            }
                                            finally
                                            {
                                                player1.Dispose();
                                            }
                                            makeSound.SpeakNow("ბედნიერი დღე");
                                        }
                                        else
                                        if (portparsed == 2500)
                                        {
                                            makeSound.SpeakNow("ღამე მშვიდობისა , გისურვებთ ბედნიერ ღამეს! ");
                                        }
                                        else
                                        if (portparsed == 3000)
                                        {
                                            makeSound.SpeakNow("დილა მშვიდობისა ,ნათია გისურვებთ ბედნიერ დღეს!");
                                            makeSound.SpeakNow("ავიმაღლოთ განწყობა");
                                            SoundPlayer player1 = new SoundPlayer(listsongs[rand.Next(0, listsongs.Count)]);
                                            try
                                            {
                                                player1.Play();
                                                Thread.Sleep(10000);
                                            }
                                            catch (Exception)
                                            {
                                                Console.WriteLine("Shecdoma");
                                            }
                                            finally
                                            {
                                                player1.Dispose();
                                            }
                                        }
                                        if (portparsed == 1310)
                                        {
                                            makeSound.SpeakNow("ყურადღება, ყურადღება, განგაში.  გორში გაითიშა ოპტიკა, შეატყობინე შესაბამის პირს");

                                        }
                                        if (portparsed == 1410)
                                        {
                                            makeSound.SpeakNow("ყურადღება, ყურადღება, განგაში.  ფოთში გაითიშა ოპტიკა, შეატყობინე შესაბამის პირს");

                                        }
                                        if (portparsed == 1510)
                                        {
                                            makeSound.SpeakNow("ყურადღება, ყურადღება, განგაში.  ქუთაისში გაითიშა ოპტიკა, შეატყობინე შესაბამის პირს");

                                        }
                                        if (portparsed == 2510)
                                        {
                                            makeSound.SpeakNow("ყურადღება, ყურადღება, განგაში.  თელავში გაითიშა ოპტიკა, შეატყობინე შესაბამის პირს");

                                        }
                                        if (portparsed == 333)
                                        {
                                            Console.WriteLine("qutaisi  speaking....");
                                            makeSound.SpeakNow("ქუთაისში, გაითიშა სარელეო სადგური, რეზერვი. გთხოვ გადაამოწმო ან შეატყობინე, შესაბამის პირს.");
                                        }
                                        if (portparsed == 444)
                                        {
                                            makeSound.SpeakNow("ფოთში, გაითიშა სარელეო სადგური, რეზერვი. გთხოვ გადაამოწმო ან შეატყობინე, შესაბამის პირს.");
                                        }
                                        if (portparsed == 555)
                                        {
                                            makeSound.SpeakNow("თელავში, გაითიშა სარელეო სადგური, რეზერვი. გთხოვ გადაამოწმო ან შეატყობინე, შესაბამის პირს.");
                                        }
                                        if (portparsed == 666)
                                        {
                                            makeSound.SpeakNow("გორში, გაითიშა სარელეო სადგური, რეზერვი. გთხოვ გადაამოწმო ან შეატყობინე, შესაბამის პირს.");
                                        }
                                        else
                                        {
                                            speake(portparsed);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(exp.StackTrace);
                        Console.WriteLine("gvaqvs shecdoma");
                    }
                }
            }
            catch (Exception)
            {

                goto mods;
            }
        }

        private void speake(int port)
        {
            Console.WriteLine( port);
            var chan = channels.GetChanellByPort(port);
            int chanellid = chan.Id;
            var Info = info.GeTInfoByCHanellID(chanellid);
            var trans = transcoder.GetTranscoderInfoByCHanellId(chanellid);
            Console.WriteLine("Aq modis ");
            if (chan != null && chan.Name != null)
            {
                if ((port >= 129 && port <= 133) || (port >= 137 && port <= 143))
                {
                    makeSound.SpeakNow($"გაითიშა {chan.Name}. შემდეგი სიხშირე: {chan.ChanellFormat}");
                }
                else if (port >= 134 && port <= 136)
                {
                    makeSound.SpeakNow($"გაითიშა ტე ორი , გადაამოწმე {chan.Name} სიხშირე. {chan.ChanellFormat}.");
                }
                else if (port == 144)//jeoselis rezervi
                {
                    makeSound.SpeakNow("გაითიშა  ჯეოსელის , სარეზერვო , გთოვთ გადაამოწმოთ. იემერ 240 .");
                }
                else if (port == 145)
                {
                    //silkis optika
                    makeSound.SpeakNow("ყურადღება, ყურადღება, გაითიშა მთავარი ოპტიკა, ირთვება განგაში.");
                    SoundPlayer player = new SoundPlayer("C:\\Users\\marjanishvili\\source\\repos\\Speaker.RobotForGLobal-master\\AlarmsAndSounds\\war-alarm-fx_132bpm.wav");
                    player.Play();
                    Thread.Sleep(8000);
                    player.Play();
                    Thread.Sleep(8000);
                    player.Play();
                    Thread.Sleep(8000);
                    player.Play();
                    Thread.Sleep(8000);
                    player.Play();

                }
                else if (port == 146)
                {
                    makeSound.SpeakNow("გაითიშა , სილქის რესივერები , გადააამოწმე  , ენკოდერ 2  და  3, აგრეთე  შეამოწმე , იემერ 230 , ქარდ 1 პორტ 1");
                }
                else if (port == 147)//icone recievers
                {
                    makeSound.SpeakNow("გაითიშა , აიქონის რესივერები , გადაამოწმე  , ენკოდერ 4, აგრეთე  შეამოწმე , იემერ 230 , ქარდ 3 პორტ 1");
                }
                else if (port == 148)
                {
                    //t2 recievers
                    makeSound.SpeakNow("გაითიშა , ტე ორის  რესივერები , გადაამოწმე  , ენკოდერ 2  და  3, აგრეთე  შეამოწმე , იემერ 230 , ქარდ 3 პორტ 2");
                }
                else
                {
                    makeSound.SpeakNow($"{chan.Name}ზე გვაქვს  ხარვეზი");
                    if (Info != null)
                    {
                        makeSound.SpeakNow(Info.AlarmMessage);
                    }

                    if (trans != null && trans.Card != 0 && trans.Port != 0)
                    {
                        makeSound.SpeakNow($"არხი გატარებულია ტრანსკოდერში , {trans.Emr_Number}, ქარდ{trans.Card}, პორტ{trans.Port}, გადაამოწმე.");
                    }

                    if (chan.ChanellFormat == "MPEG2" && chan.FromOptic == true)
                    {
                        makeSound.SpeakNow("არხი მოდის მუხიანის ბლეიდის ტრანსკოდერიდან, სავარაუდოთ გაიჭედა, გადაურეკე რომ გადააამოწმონ.");
                    }

                    else if (chan.FromOptic == true)
                    {
                        makeSound.SpeakNow($"არხი მოდის მუხიანიდან , გადაამოწმე ან გადაურეკე!");
                    }

                }

            }

                Thread.Sleep(1200);
        }
    }
}
