using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{

    public enum Duration
    {
        _8am, _9am, _10am, _11am, _12noon, _1pm, _2pm, _3pm, _4pm, _5pm, _6pm
    }
    public enum Salutation
    {
        Dr = 1, Nurse, Mr, Mrs, Miss, Engr, Pastor
    }
    public enum Relationship
    {
        Father = 1, Mother, Sister, Brother, Friend, Others
    }
    public enum Gender
    {
        Male = 1, Female
    }
    public enum ThemeColor
    {
        DeepRed = 1, LightBlue, NavyBlue, ArmyGreen, LightRed
    }
    public enum PMode
    {
        Cash = 1, Cheque, Teller
    }
    public enum Status
    {
        GivenOut = 1, Returned
    }

    public enum QuestionType
    {
        SingleChoice, MultiChoice, BlankChoice
    }


    public enum Religion
    {
        Christianity = 1, Muslim, Others
    }

    public enum ClassType
    {
        UNITAS = 1, VERITAS, CARRITAS
    }
    public enum Maritalstatus
    {
        Single = 1, Married, Divorced
    }

    //public enum LGA
    //{
    //    Ado = 1, Agatu, Apa, Buruku, Gboko, Guma, Gwer_East, Gwer_West, Kastina_Ala, Konshisha,
    //    Kwande, Logo, Makurdi, Obi, Ogbadibo, Ohimini, Oju, Okpokwu, Otukpo, Tarka, Ukum,
    //    Ushongo, Vandeikya
    //}
    public enum LGA
    {
        Jos_North = 1, Jos_South, Barkin_Ladi, Bassa, Bokkos, Jos_East, Kanam, Kanke, Langtang_North,
        Langtang_South, Mangu, Mikang, Pankshin, Quaan_Pan, Ryom, Shendam, Wase
    }

    public enum OwershipType
    {
        Government = 1, Private, Christian_Based, Muslim_Based, NGO, Others
    }

    public enum StudentType
    {
        Fresher, Returning
    }

    public enum FeeCategory
    {
        School_Fee
    }
    public enum StudentStatus
    {
        Fresher = 1, Returning
    }

    public enum Qualifications
    {
        SSCE,
        NCE,
        OND,
        HND,
        Degree,
        Masters

    }


    public enum State
    {
        Abia = 1, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta, Ebonyi, Edo, Ekiti,
        Abuja, Gombe, Imo, Jigawa, Kaduna, Kano, Katsina, Kebbi, Kogi, Kwara, Lagos, Nasarawa, Niger, Ogun, Ondo, Osun,
        Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara
    }



    public enum GradeStatus
    {
        A1 = 1, B2, B3, C4, C5, C6, D7, E8, F9
    }

    public enum SchoolName
    {
        JSS = 1, SS, PRY, KG
    }

    //public enum ContactGroup
    //{
    //    JSS1 = 1, JSS2, JSS3, SS1, SS2, SS3, Staffs, FormTeacher, All_Student, Past_Students
    //}
    public enum TruckStatus : byte
    {
        [Display(Name = "Planned")]
        orange,
        [Display(Name = "Confirmed")]
        green,
        [Display(Name = "Changed")]
        red,
        [Display(Name = "Loaded")]
        darkcyan
    }

    public enum Extra
    {
        A = 1, B, C
    }

    public enum MyClass
    {
        JSS1 = 1, JSS2, JSS3, SS1, SS2, SS3
    }

    public enum FileType
    {
        PDf = 1, MP4, MP3
    }

    public enum ReportCardType
    {
        WithoutPositionPrimary, WithPositionPrimary, WithoutPositionSecondary, WithPositionSecondary, BigFont
    }

    public enum ServiceType
    {
        School_Fee = 1, Accepatance_Fee, Supplementary_List_Payment, Hostel_Application_Fee, Acommodation_Payment_Fee, Faculty_Fee, Department_Fee
    }

}
