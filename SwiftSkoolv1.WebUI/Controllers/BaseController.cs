using Microsoft.AspNet.Identity;
using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public SwiftSkoolDbContext Db;
        public QueryManager _query;
        private static int myCount = 0;

        public string userSchool;
        public string userId;
        public bool IsExpired;

        public BaseController()
        {
            Db = new SwiftSkoolDbContext();
            _query = new QueryManager();
            userSchool = _query.GetId();
            userId = _query.GetUserId();
            IsExpired = _query.CheckIsExpired(userSchool);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var user = User.Identity.GetUserId();
            userSchool = Db.Users.AsNoTracking().Where(x => x.Id.Equals(user))
                .Select(s => s.SchoolId).FirstOrDefault();

            var school = Db.Schools.Find(userSchool);

            // var model = filterContext.Controller.ViewData.Model as BaseViewModel;
            var model = new BaseViewModel();

            if (school != null)
            {
                model.Alias = school.Alias;
                model.SchoolName = school.Name;
                model.SchoolId = school.SchoolId;
                model.Color = school.Color;
                ViewBag.ImageId = school.SchoolId;
            }
            else
            {
                model.Alias = "SwiftSkool";
                model.SchoolName = "SwiftSkool";
                model.SchoolId = "SwiftSkool";
                model.Color = "";
                IsExpired = false;
            }
            ViewBag.LayoutViewModel = model;
            myCount = myCount + 1;
            bool needToRedirect = myCount > 2;
            if (IsExpired && needToRedirect)
            {
                var url = Url.Action("LogOut", "Account", new { }, protocol: Request.Url.Scheme);
                filterContext.Result = new RedirectResult(url);
                return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Db != null)
                {
                    Db.Dispose();
                    Db = null;
                }
            }

            base.Dispose(disposing);
        }

        public static List<string> YearCategory()
        {
            var yearCategory = new List<string>();
            yearCategory.Add("1999");
            yearCategory.Add("2000");
            yearCategory.Add("2001");
            yearCategory.Add("2002");
            yearCategory.Add("2003");
            yearCategory.Add("2004");
            yearCategory.Add("2005");
            yearCategory.Add("2006");
            yearCategory.Add("2007");
            yearCategory.Add("2008");
            yearCategory.Add("2009");
            yearCategory.Add("2010");
            yearCategory.Add("2011");
            yearCategory.Add("2013");
            yearCategory.Add("2014");
            yearCategory.Add("2015");
            yearCategory.Add("2016");
            yearCategory.Add("2017");
            return yearCategory;
        }

        public static List<string> ExamTypeList()
        {
            var examType = new List<string>();
            examType.Add("JAMB");
            examType.Add("WAEC");
            examType.Add("NECO");
            examType.Add("GCE");
            return examType;
        }

        public Dictionary<string, List<string>> PopulateLga()
        {
            var lgaList = new Dictionary<string, List<string>>();

            lgaList.Add("FCT", new List<string>()
            {
                "Gwagwalada", "Kuje", "Abaji","Abuja Municipal","Bwari","Kwali", "Others"
            });
            lgaList.Add("ABIA", new List<string>()
            {
                "Aba North","Aba South","Arochukwu","Bende","Ikwuano","Isiala Ngwa North","Isiala Ngwa South","Isuikwuato","Obi Ngwa","Ohafia","Ohafia","Ugwunagbo","Ukwa East","Ukwa West","Umuahia North","Umuahia South","Umu Nneochi"
            });
            lgaList.Add("ADAMAWA", new List<string>()
            {
                "Demsa","Fufore","Ganye","Gayuk","Gombi","Hong","Jada","Lamurde","Madagali","Mayo Belwa","Michika","Mubi North","Mubi South","Numan","Shelleng","Song","Toungo","Yola North","Yola South"
            });
            lgaList.Add("AKWAIBOM", new List<string>()
            {
                "Abak","Eastern Obolo","Eket","Esit Eket","Essien Udim","Etim Ekpo","Etinan","Ibeno","Ibesikpo Asutan","Ibiono-Ibom","Ika","Ikono","Ikot Abasi","Ikot Ekpene","Ini","Itu","Mbo","Mkpat-Enin","Nsit-Atai","Nsit-Ibom","Nsit-Ubium","Obot Akara","Okobo","Onna","Oron","Oruk Anam","Udung-Uko","Ukanafun","Uruan","Urue-Offong Oruko","Uyo"
            });
            lgaList.Add("ANAMBRA", new List<string>()
            {
                "Aguata","Anambra East","Anambra West","Anaocha","Awka North","Awka South","Ayamelum","Dunukofia","Ekwusigo","Idemili North","Idemili South","Ihiala","Njikoka","Nnewi North","Nnewi South","Ogbaru","Onitsha North","Onitsha South","Orumba North","Orumba South","Oyi"
            });
            lgaList.Add("BAUCHI", new List<string>()
            {
                "Alkaleri","Bauchi","Bogoro","Damban","Darazo","Dass","Gamawa","Ganjuwa","Giade","Itas Gadau","Jama'are","Katagum","Kirfi","Misau","Ningi","Shira","Tafawa Balewa","Toro","Warji","Zaki"
            });
            lgaList.Add("BAYELSA", new List<string>()
            {
                "Brass","Ekeremor","Kolokuma Opokuma","Nembe","Ogbia","Sagbama","Southern Ijaw","Yenagoa"
            });
            lgaList.Add("BENUE", new List<string>()
            {
                "Apa","Ado","Agatu","Buruku","Gboko","Guma","Gwer East","Gwer West","Katsina-Ala","Konshisha","Kwande","Logo","Makurdi","Obi","Ogbadibo","Ohimini","Oju","Okpokwu","Oturkpo","Tarka","Ukum","Ushongo","Vandeikya"
            });
            lgaList.Add("BORNO", new List<string>()
            {
                "Abadam","Askira/Uba","Bama","Bayo","Biu","Chibok","Damboa","Dikwa","Gubio","Guzamala","Gwoza","Hawul","Jere","Kaga","Kala/Balge","Konduga","Kukawa","Kwaya Kusar","Mafa","Magumeri","Maiduguri","Marte","Mobbar","Monguno","Ngala","Nganzai","Shani"
            });
            lgaList.Add("CROSSRIVER", new List<string>()
            {
                "Akamkpa","Akpabuyo","Bakassi","Bekwarra","Biase","Boki","Calabar Municipal","Calabar South","Etung","Ikom","Obanliku","Obubra","Obudu","Odukpani","Ogoja","Yakuur","Yala"
            });

            lgaList.Add("DELTA", new List<string>()
            {
                "Aniocha South","Bomadi","Burutu","Ethiope East","Ethiope West","Ika North East","Ika South","Isoko North","Isoko South","Ndokwa East","Ndokwa West","Okpe","Oshimili North","Oshimili South","Patani","Sapele","Udu","Ughelli North","Ughelli South","Ukwuani","Uvwie","Warri North","Warri South","Warri South West"
            });
            lgaList.Add("EBONYI", new List<string>()
            {
                "Afikpo North","Afikpo South","Ebonyi","Ezza North","Ezza South","Ikwo","Ishielu","Ivo","Izzi","Ohaozara","Ohaukwu","Onicha"
            });
            lgaList.Add("EDO", new List<string>()
            {
                "Egor","Esan Central","Esan North-East","Esan South-East","Esan West","Etsako Central","Etsako East","Etsako West","Igueben","Ikpoba Okha","Orhionmwon","Oredo","Ovia North-East","Ovia South-West","Owan East","Owan West","Uhunmwonde"
            });
            lgaList.Add("EKITI", new List<string>()
            {
                "Ado","Efon","Ekiti East","Ekiti South-West","Ekiti West","Emure","Gbonyin","Ido/Osi","Ijero","Ikere","Ikole","Ilejemeje","Irepodun/Ifelodun","Ise Orun","Moba","Oye"
            });

            lgaList.Add("ENUGU", new List<string>()
            {
                "Awgu","Enugu East","Enugu North","Enugu South","Ezeagu","Igbo Etiti","Igbo Eze North","Igbo Eze South","Isi Uzo","Nkanu East","Nkanu West","Nsukka","Oji River","Udenu","Udi","Uzo Uwani"
            });
            lgaList.Add("GOMBE", new List<string>()
            {
                "Akko","Balanga","Billiri","Dukku","Funakaye","Gombe","Kaltungo","Kwami","Nafada","Shongom","Yamaltu/Deba"
            });
            lgaList.Add("IMO", new List<string>()
            {
                "Aboh-Mbaise","Ahiazu Mbaise","Ehime Mbano","Ezinihitte","Ideato North","Ideato South","Ihitte/Uboma","Ikeduru","Isiala Mbano","Isu","Mbaitoli","Ngor Okpala","Njaba","Nkwerre","Nwangele","Obowo","Oguta","Ohaji Egbema","Okigwe","Orlu","Orsu","Oru East","Oru West","Owerri Municipal","Owerri North","Owerri West","Unuimo"
            });
            lgaList.Add("JIGAWA", new List<string>()
            {
                "Auyo","Babura","Biriniwa","Birnin Kudu","Buji","Dutse","Gagarawa","Garki","Gumel","Guri","Gwaram","Gwiwa","Hadejia","Jahun","Kafin Hausa","Kazaure","Kiri Kasama","Kiyawa","Kaugama","Maigatari","Malam Madori","Miga","Ringim","Roni","Sule Tankarkar","Taura","Yankwashi"
            });
            lgaList.Add("KADUNA", new List<string>()
            {
                "Birni-Gwari","Chikun","Giwa","Igabi","Ikara","Jaba","Jemaa","Kachia","Kaduna North","Kaduna South","Kagarko","Kajuru","Kaura","Kauru","Kubau","Kudan","Lere","Makarfi","Sabon Gari","Sanga","Soba","Zangon Kataf","Zaria"
            });
            lgaList.Add("KANO", new List<string>()
            {
                "Ajingi","Albasu","Bagwai","Bebeji","Bichi","Bunkure","Dala","Dambatta","Dawakin Kudu","Dawakin Tofa","Doguwa","Fagge","Gabasawa","Garko","Garun Mallam","Gaya","Gezawa","Gwale","Gwarzo","Kabo","Kano Municipal","Karaye","Kibiya","Kiru","Kumbotso","Kunchi","Kura","Madobi","Makoda","Minjibir","Nasarawa","Rano","Rimin Gado","Rogo","Shanono","Sumaila","Takai","Tarauni","Tofa","Tsanyawa","Tudun Wada","Ungogo","Warawa","Wudil"
            });
            lgaList.Add("KATSINA", new List<string>()
            {
                "Bakori","Batagarawa","Batsari","Baure","Bindawa","Charanchi","Dandume","Danja","Dan Musa","Daura","Dutsi","Dutsin Ma","Faskari","Funtua","Ingawa","Jibia","Kafur","Kaita","Kankara","Kankia","Katsina","Kurfi","Kusada","Mai'Adua","Malumfashi","Mani","Mashi","Matazu","Musawa","Rimi","Sabuwa","Safana","Sandamu","Zango"
            });
            lgaList.Add("KEBBI", new List<string>()
            {
                "Aleiro","Arewa Dandi","Argungu","Augie","Bagudo","Birnin Kebbi","Bunza","Dandi","Fakai","Gwandu","Jega","Kalgo","Koko Besse","Maiyama","Ngaski","Sakaba","Shanga","Suru","Wasagu Danko","Yauri","Zuru"
            });
            lgaList.Add("KOGI", new List<string>()
            {
                "Adavi","Ajaokuta","Ankpa","Bassa","Dekina","Ibaji","Idah","Igalamela Odolu","Ijumu","Kabba/Bunu","Kogi","Lokoja","Mopa Muro","Ofu","Ogori Magongo","Okehi","Okene","Olamaboro","Omala","Yagba East","Yagba West"
            });
            lgaList.Add("KWARA", new List<string>()
            {
                "Asa","Baruten","Edu","Ekiti","Ifelodun","Ilorin East","Ilorin South","Ilorin West","Irepodun","Isin","Kaiama","Moro","Offa","Oke Ero","Oyun","Pategi"
            });
            lgaList.Add("LAGOS", new List<string>()
            {
                "Agege","Ajeromi-Ifelodun","Alimosho","Amuwo-Odofin","Apapa","Badagry","Epe","Eti Osa","Ibeju-Lekki","Ifako-Ijaiye","Ikeja","Ikorodu","Kosofe","Lagos Island","Lagos Mainland","Mushin","Ojo","Oshodi-Isolo","Shomolu","Surulere"
            });
            lgaList.Add("NASARAWA", new List<string>()
            {
                "Akwanga","Awe","Doma","Karu","Keana","Keffi","Kokona","Lafia","Nasarawa","Nasarawa Egon","Obi","Toto","Wamba"
            });
            lgaList.Add("NIGER", new List<string>()
            {
                "Agaje","Agwara","Bida","Borgu","Bosso","Chanchaga","Edati","Gbako","Gurara","Katcha","Kontagora","Lapai","Lavun","Magama","Mariga","Mashegu","Mokwa","Moya","Paikoro","Rafi","Rijau","Shiroro","Suleja","Tafa","Wushishi"
            });
            lgaList.Add("OGUN", new List<string>()
            {
                "Abeokuta North","Abeokuta South","Ado-Odo Ota","Egbado North","Egbado South","Ewekoro","Ifo","Ijebu East","Ijebu North","Ijebu North East","Ijebu Ode","Ikenne","Imeko Afon","Ipokia","Obafemi Owode","Odeda","Odogbolu","Ogun Waterside","Remo North","Shagamu"
            });
            lgaList.Add("ONDO", new List<string>()
            {
                "Akoko North-East","Akoko North-West","Akoko South-West","Akoko South-East","Akure North","Akure South","Ese Odo","Idanre","Ifedore","Ilaje","Ile Oluji Okeigbo","Irele","Odigbo","Okitipupa","Ondo East","Ondo West","Ose","Owo"
            });
            lgaList.Add("OSUN", new List<string>()
            {
                "Aiyedade","Aiyedire","Atakumosa East","Atakumosa West","Boluwaduro","Boripe","Ede North","Ede South","Egbedore","Ejigbo","Ife Central","Ife East","Ife North","Ife South","Ifedayo","Ifelodun","Ila","Ilesha East", "Ilesha West", "Irepodun","Irewole","Isokan","Iwo","Obokun","Odo-Otin","Ola-Oluwa","Olarunda","Oriade","Orolu","Osogbo"
            });
            lgaList.Add("OYO", new List<string>()
            {
                "Afijio","Akinyele","Atiba","Atisbo","Egbeda","Ibadan North","Ibadan North-East","Ibadan North-West","Ibadan South-East","Ibadan South-West","Ibarapa Central","Ibarapa East","Ibarapa North","Ido","Irepo","Iseyin","Itesiwaju","Iwajowa","Kajola","Lagelu","Ogbomosho North","Ogbomosho South","Ogo Oluwa","Olorunsogo","Oluyole","Ona Ara","Orelope","Ori Ire","Oyo","Oyo East","Saki East","Saki West","Surulere"
            });
            lgaList.Add("PLATEAU", new List<string>()
            {
                "Barkin Ladi","Bassa","Bokkos","Jos East","Jos North","Jos South","Kanam","Kanke","Langtang South","Langtang North","Mangu","Mikang","Pankshin","Qua'an Pan","Riyom","Shendam","Wase"
            });
            lgaList.Add("RIVERS", new List<string>()
            {
                "Abua/Odual","Ahoada East","Ahoada West","Akuku-Toru","Andoni","Asari-Toru","Bonny","Degema","Eleme","Emuoha","Etche","Gokana","Ikwerre","Khana","Obio Akpor","Ogba Egbema Ndoni","Ogu Bolo","Okrika","Omuma","Opobo Nkoro","Oyigbo","Port Harcourt","Tai"
            });
            lgaList.Add("SOKOTO", new List<string>()
            {
                "Binji","Bodinga","Dange Shuni","Gada","Goronyo","Gudu","Gwadabawa","Illela","Isa","Kebbe","Kware","Rabah","Sabon Birni","Shagari","Silame","Sokoto North","Sokoto South","Tambuwal","Tangaza","Tureta","Wamako","Wurno","Yabo"
            });
            lgaList.Add("TARABA", new List<string>()
            {
                "Ardo-kola","Bali","Donga","Gashaka","Gassol","Ibi","Jalingo","Karim Lamido","Kumi","Lau","Sardauna","Takum","Ussa","Wukari","Yorro","Zing"
            });
            lgaList.Add("YOBE", new List<string>()
            {
                "Bade","Bursari","Damaturu","Fika","Fune","Geidam","Gujba","Gulani","Jakusko","Karasuwa","Machina","Nangere","Nguru","Potiskum","Tarmuwa","Yunusari","Yusufari"
            });
            lgaList.Add("ZAMFARA", new List<string>()
            {
               "Bakura","Birnin Magaji Kiyaw","Bukkuyum","Bungudu","Gummi","Gusau","Kaura Namoda","Maradun","Maru","Shinkafi","Talata Mafara","Tsafe","Zurmi"
            });

            return lgaList;
        }
    }
}